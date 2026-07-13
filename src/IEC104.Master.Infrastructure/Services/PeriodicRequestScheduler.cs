using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Infrastructure.Services
{
    public class PeriodicRequestScheduler : IPeriodicRequestScheduler, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<int, Timer> _timers = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool _isDisposed;
        private int _commonAddress = 1;

        public event EventHandler<PeriodicRequestExecutedEventArgs>? RequestExecuted;

        public PeriodicRequestScheduler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                await ReloadAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task ReloadAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPeriodicRequestRepository>();

            // توقف همه‌ی تایمرهای قبلی
            await StopAsync();

            // دریافت درخواست‌های فعال
            var requests = await repository.GetActiveRequestsAsync();

            foreach (var request in requests)
            {
                StartTimerForRequest(request);
            }
        }

        public Task StopAsync()
        {
            foreach (var timer in _timers.Values)
            {
                timer?.Dispose();
            }
            _timers.Clear();
            return Task.CompletedTask;
        }

        private void StartTimerForRequest(PeriodicRequest request)
        {
            var interval = TimeSpan.FromMinutes(request.IntervalMinutes);
            var timer = new Timer(
                callback: _ => ExecuteRequest(request),
                state: null,
                dueTime: TimeSpan.Zero,      // اولین اجرا بلافاصله
                period: interval
            );

            _timers[request.Id] = timer;
        }

        private async void ExecuteRequest(PeriodicRequest request)
        {
            // جلوگیری از اجرای همزمان درخواست‌ها
            using var scope = _serviceProvider.CreateScope();
            var transport = scope.ServiceProvider.GetRequiredService<IIec104Transport>();
            var repository = scope.ServiceProvider.GetRequiredService<IPeriodicRequestRepository>();

            var args = new PeriodicRequestExecutedEventArgs
            {
                RequestId = request.Id,
                RequestName = request.Name,
                ExecutedAt = DateTime.Now
            };

            try
            {
                // ارسال درخواست بر اساس نوع
                await SendRequestAsync(transport, request);

                args.Success = true;
                args.Message = "Executed successfully";

                // به‌روزرسانی زمان آخرین اجرا
                request.LastExecutedAt = DateTime.Now;
                request.ErrorMessage = null;
            }
            catch (Exception ex)
            {
                args.Success = false;
                args.Message = ex.Message;

                request.LastExecutedAt = DateTime.Now;
                request.ErrorMessage = ex.Message;
            }

            // ذخیره‌سازی تغییرات در دیتابیس
            try
            {
                repository.Update(request);
                await repository.SaveChangesAsync();
            }
            catch { /* خطای ذخیره‌سازی را نادیده بگیرید */ }

            // اطلاع‌رسانی به UI
            RequestExecuted?.Invoke(this, args);
        }

        // در Infrastructure/Services/PeriodicRequestScheduler.cs

        private async Task SendRequestAsync(IIec104Transport transport, PeriodicRequest request)
        {
            switch (request.Type)
            {
                case RequestType.GeneralInterrogation:
                    var qoi = byte.TryParse(request.Parameter, out var parsedQoi) ? parsedQoi : (byte)20;
                    await transport.SendGeneralInterrogationAsync(qoi);
                    break;

                case RequestType.GroupInterrogation:
                    var groupQoi = byte.TryParse(request.Parameter, out var parsedGroupQoi) ? parsedGroupQoi : (byte)20;
                    await transport.SendGeneralInterrogationAsync(groupQoi);
                    break;

                case RequestType.SinglePoint:
                    // پارامتر باید IOA باشد
                    if (!int.TryParse(request.Parameter, out var ioa))
                    {
                        throw new ArgumentException("پارامتر برای SinglePoint باید IOA معتبر باشد.");
                    }
                    await transport.SendSinglePointReadAsync(ioa, request.CommonAddress);
                    break;

                default:
                    throw new NotSupportedException($"نوع درخواست {request.Type} پشتیبانی نمی‌شود.");
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            StopAsync().Wait();
            _semaphore.Dispose();
        }
    }
}