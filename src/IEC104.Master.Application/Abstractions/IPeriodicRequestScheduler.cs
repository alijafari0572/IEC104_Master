using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.Abstractions
{
    public interface IPeriodicRequestScheduler
    {
        /// <summary>
        /// شروع زمان‌بندی برای همه‌ی درخواست‌های فعال
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// توقف همه‌ی زمان‌بندی‌ها
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// بازخوانی مجدد درخواست‌ها از دیتابیس (بعد از تغییرات)
        /// </summary>
        Task ReloadAsync();

        /// <summary>
        /// رویدادی که هنگام ارسال هر درخواست فراخوانی می‌شود (برای لاگ یا UI)
        /// </summary>
        event EventHandler<PeriodicRequestExecutedEventArgs>? RequestExecuted;
    }

    public class PeriodicRequestExecutedEventArgs : EventArgs
    {
        public int RequestId { get; set; }
        public string RequestName { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}