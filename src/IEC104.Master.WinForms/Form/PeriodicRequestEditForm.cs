using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IEC104.Master.WinForms.Form
{
    public partial class PeriodicRequestEditForm : System.Windows.Forms.Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly int? _editId;
        private PeriodicRequest _entity;

        public PeriodicRequestEditForm(IServiceProvider serviceProvider, IPeriodicRequestScheduler scheduler, int? editId = null)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _editId = editId;
            _entity = new PeriodicRequest();
        }

        private async void PeriodicRequestEditForm_Load(object sender, EventArgs e)
        {
            if (_editId.HasValue)
            {
                this.Text = "✏️ ویرایش درخواست";
                await LoadForEditAsync();
            }
            else
            {
                this.Text = "➕ افزودن درخواست جدید";
                cmbType.SelectedIndex = 0;
            }
        }

        private async Task LoadForEditAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPeriodicRequestRepository>();

            _entity = await repository.GetByIdAsync(_editId.Value);
            if (_entity == null)
            {
                MessageBox.Show("درخواست مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            txtName.Text = _entity.Name;
            cmbType.SelectedItem = _entity.Type.ToString();
            txtParameter.Text = _entity.Parameter;
            nudInterval.Value = _entity.IntervalMinutes;
            chkActive.Checked = _entity.IsActive;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // ===== اعتبارسنجی اولیه =====
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("لطفاً نام درخواست را وارد کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("لطفاً نوع درخواست را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbType.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtParameter.Text))
            {
                MessageBox.Show("لطفاً پارامتر را وارد کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtParameter.Focus();
                return;
            }

            // ===== تبدیل نوع درخواست به Enum =====
            if (!Enum.TryParse<RequestType>(cmbType.SelectedItem.ToString(), out var requestType))
            {
                MessageBox.Show("نوع درخواست نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ===== ★ اعتبارسنجی ویژه برای SinglePoint ★ =====
            if (requestType == RequestType.SinglePoint)
            {
                if (!int.TryParse(txtParameter.Text, out _))
                {
                    MessageBox.Show("برای SinglePoint، پارامتر باید یک عدد (IOA) باشد.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtParameter.Focus();
                    return;
                }
            }

            // ===== ادامه کد ذخیره‌سازی در دیتابیس =====
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPeriodicRequestRepository>();

            try
            {
                if (_editId.HasValue)
                {
                    // ویرایش
                    _entity.Name = txtName.Text.Trim();
                    _entity.Type = requestType;
                    _entity.Parameter = txtParameter.Text.Trim();
                    _entity.IntervalMinutes = (int)nudInterval.Value;
                    _entity.IsActive = chkActive.Checked;
                    _entity.UpdatedAt = DateTime.Now;
                    repository.Update(_entity);
                }
                else
                {
                    // افزودن جدید
                    var newRequest = new PeriodicRequest
                    {
                        Name = txtName.Text.Trim(),
                        Type = requestType,
                        Parameter = txtParameter.Text.Trim(),
                        IntervalMinutes = (int)nudInterval.Value,
                        IsActive = chkActive.Checked,
                        CreatedAt = DateTime.Now
                    };
                    await repository.AddAsync(newRequest);
                }

                await repository.SaveChangesAsync();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره‌سازی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}