using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.DTOs;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IEC104.Master.WinForms.Form
{
    public partial class SendRequestForm : System.Windows.Forms.Form
    {
        private readonly IIec104MasterAppService _appService;

        public SendRequestForm(IIec104MasterAppService appService)
        {
            InitializeComponent();
            _appService = appService;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("لطفاً نوع درخواست را انتخاب کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtParameter.Text))
            {
                MessageBox.Show("لطفاً پارامتر را وارد کنید.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtParameter.Focus();
                return;
            }

            var selectedType = cmbType.SelectedItem.ToString();
            btnSend.Enabled = false;
            btnSend.Text = "⏳ در حال ارسال...";
            txtResult.Text = "⏳ در حال ارسال درخواست...";

            try
            {
                switch (selectedType)
                {
                    case "GeneralInterrogation":
                        var qoi = byte.TryParse(txtParameter.Text, out var parsedQoi) ? parsedQoi : (byte)20;
                        await _appService.SendGeneralInterrogationAsync(new GeneralInterrogationRequestDto(qoi));
                        txtResult.Text = $"✅ درخواست بازجویی عمومی با QOI={qoi} با موفقیت ارسال شد.\nمنتظر دریافت داده‌ها از RTU باشید...";
                        break;

                    case "GroupInterrogation":
                        var groupQoi = byte.TryParse(txtParameter.Text, out var parsedGroupQoi) ? parsedGroupQoi : (byte)20;
                        await _appService.SendGeneralInterrogationAsync(new GeneralInterrogationRequestDto(groupQoi));
                        txtResult.Text = $"✅ درخواست بازجویی گروهی با QOI={groupQoi} با موفقیت ارسال شد.\nمنتظر دریافت داده‌ها از RTU باشید...";
                        break;

                    // ★ بخش جدید برای SinglePoint ★
                    case "SinglePoint":
                        if (!int.TryParse(txtParameter.Text, out var ioa))
                        {
                            txtResult.Text = "❌ پارامتر باید یک عدد صحیح (IOA) باشد.";
                            return;
                        }
                        await _appService.SendSinglePointReadAsync(ioa);
                        txtResult.Text = $"✅ درخواست خواندن نقطه‌ی IOA={ioa} با موفقیت ارسال شد.\nمنتظر پاسخ از RTU باشید...";
                        break;

                    default:
                        txtResult.Text = "❌ نوع درخواست پشتیبانی نمی‌شود.";
                        break;
                }
            }
            catch (Exception ex)
            {
                txtResult.Text = $"❌ خطا در ارسال درخواست: {ex.Message}";
                MessageBox.Show($"خطا: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSend.Enabled = true;
                btnSend.Text = "📤 ارسال";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // به‌روزرسانی نتیجه در صورت دریافت داده از سمت RTU (اختیاری)
        public void UpdateResult(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateResult(message)));
                return;
            }
            txtResult.Text = $"✅ {message}";
        }
    }
}