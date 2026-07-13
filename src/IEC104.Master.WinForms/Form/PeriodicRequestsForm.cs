using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Application.DTOs;
using IEC104.Master.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IEC104.Master.WinForms.Form
{
    public partial class PeriodicRequestsForm : System.Windows.Forms.Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPeriodicRequestScheduler _scheduler;
        private readonly BindingSource _bindingSource = new BindingSource();
        private List<PeriodicRequestDto> _requests = new();

        public PeriodicRequestsForm(IServiceProvider serviceProvider, IPeriodicRequestScheduler scheduler)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _scheduler = scheduler;
            _scheduler.RequestExecuted += OnSchedulerRequestExecuted;
        }

        private async void PeriodicRequestsForm_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            await LoadRequestsAsync();
        }

        private void ConfigureDataGridView()
        {
            dgvRequests.AutoGenerateColumns = false;
            dgvRequests.Columns.Clear();

            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 40
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "نام",
                DataPropertyName = "Name",
                Width = 150
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Type",
                HeaderText = "نوع",
                DataPropertyName = "Type",
                Width = 120
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Parameter",
                HeaderText = "پارامتر",
                DataPropertyName = "Parameter",
                Width = 80
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IntervalMinutes",
                HeaderText = "بازه (دقیقه)",
                DataPropertyName = "IntervalMinutes",
                Width = 80
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "وضعیت",
                DataPropertyName = "Status",
                Width = 80
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LastExecution",
                HeaderText = "آخرین اجرا",
                DataPropertyName = "LastExecution",
                Width = 100
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ErrorMessage",
                HeaderText = "خطا",
                DataPropertyName = "ErrorMessage",
                Width = 150
            });

            dgvRequests.DataSource = _bindingSource;
        }

        private async Task LoadRequestsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPeriodicRequestRepository>();

            var entities = await repository.GetAllAsync();
            _requests = entities.Select(e => new PeriodicRequestDto
            {
                Id = e.Id,
                Name = e.Name,
                Type = e.Type.ToString(),
                Parameter = e.Parameter,
                IntervalMinutes = e.IntervalMinutes,
                IsActive = e.IsActive,
                LastExecutedAt = e.LastExecutedAt,
                ErrorMessage = e.ErrorMessage
            }).ToList();

            _bindingSource.DataSource = _requests;
            _bindingSource.ResetBindings(false);
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            using var form = new PeriodicRequestEditForm(_serviceProvider, _scheduler);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                await LoadRequestsAsync();
                await _scheduler.ReloadAsync();
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRequests.CurrentRow == null)
            {
                MessageBox.Show("لطفاً یک ردیف را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = (PeriodicRequestDto)dgvRequests.CurrentRow.DataBoundItem;
            using var form = new PeriodicRequestEditForm(_serviceProvider, _scheduler, dto.Id);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                await LoadRequestsAsync();
                await _scheduler.ReloadAsync();
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRequests.CurrentRow == null)
            {
                MessageBox.Show("لطفاً یک ردیف را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = (PeriodicRequestDto)dgvRequests.CurrentRow.DataBoundItem;

            if (MessageBox.Show($"آیا از حذف درخواست \"{dto.Name}\" مطمئن هستید؟", "تأیید حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPeriodicRequestRepository>();

            var entity = await repository.GetByIdAsync(dto.Id);
            if (entity != null)
            {
                repository.Delete(entity);
                await repository.SaveChangesAsync();
                await LoadRequestsAsync();
                await _scheduler.ReloadAsync();
            }
        }

        private async void btnToggleActive_Click(object sender, EventArgs e)
        {
            if (dgvRequests.CurrentRow == null)
            {
                MessageBox.Show("لطفاً یک ردیف را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = (PeriodicRequestDto)dgvRequests.CurrentRow.DataBoundItem;

            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPeriodicRequestRepository>();

            var entity = await repository.GetByIdAsync(dto.Id);
            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                repository.Update(entity);
                await repository.SaveChangesAsync();

                await LoadRequestsAsync();
                await _scheduler.ReloadAsync();
            }
        }

        private async void btnSendNow_Click(object sender, EventArgs e)
        {
            if (dgvRequests.CurrentRow == null)
            {
                MessageBox.Show("لطفاً یک ردیف را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = (PeriodicRequestDto)dgvRequests.CurrentRow.DataBoundItem;

            using var scope = _serviceProvider.CreateScope();
            var transport = scope.ServiceProvider.GetRequiredService<IIec104Transport>();
            var repository = scope.ServiceProvider.GetRequiredService<IPeriodicRequestRepository>();

            var entity = await repository.GetByIdAsync(dto.Id);
            if (entity == null)
                return;

            try
            {
                btnSendNow.Enabled = false;
                btnSendNow.Text = "⏳ در حال ارسال...";

                await SendRequestAsync(transport, entity);

                entity.LastExecutedAt = DateTime.Now;
                entity.ErrorMessage = null;
                repository.Update(entity);
                await repository.SaveChangesAsync();

                await LoadRequestsAsync();

                MessageBox.Show($"درخواست \"{dto.Name}\" با موفقیت ارسال شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                entity.LastExecutedAt = DateTime.Now;
                entity.ErrorMessage = ex.Message;
                repository.Update(entity);
                await repository.SaveChangesAsync();

                await LoadRequestsAsync();

                MessageBox.Show($"خطا در ارسال درخواست: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSendNow.Enabled = true;
                btnSendNow.Text = "📤 ارسال فوری";
            }
        }

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
                    var pointQoi = byte.TryParse(request.Parameter, out var parsedPointQoi) ? parsedPointQoi : (byte)21;
                    await transport.SendSinglePointReadAsync(pointQoi, 1);
                    break;

                default:
                    throw new NotSupportedException($"نوع درخواست {request.Type} پشتیبانی نمی‌شود.");
            }
        }

        private void OnSchedulerRequestExecuted(object? sender, PeriodicRequestExecutedEventArgs e)
        {
            // به‌روزرسانی لیست پس از اجرای خودکار زمان‌بند
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnSchedulerRequestExecuted(sender, e)));
                return;
            }

            // به‌روزرسانی ردیف مربوطه
            var item = _requests.FirstOrDefault(r => r.Id == e.RequestId);
            if (item != null)
            {
                item.LastExecutedAt = e.ExecutedAt;
                item.ErrorMessage = e.Success ? null : e.Message;
                _bindingSource.ResetBindings(false);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}