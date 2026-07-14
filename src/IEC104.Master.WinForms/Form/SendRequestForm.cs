using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.DTOs;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// فضای نام مربوط به فرم‌های ویندوزی برنامه
/// </summary>
/// <remarks>
/// Responsibility:
/// - شامل پیاده‌سازی رابط کاربری (UI) با استفاده از Windows Forms.
/// - فرم‌های این فضای نام به عنوان نقطه‌ی ورود برای تعامل کاربر با سیستم هستند.
///
/// Design Notes:
/// - از الگوی MVP (Model-View-Presenter) به صورت غیررسمی استفاده می‌کند.
/// - فرم‌ها از طریق تزریق وابستگی (DI) سرویس‌های مورد نیاز خود را دریافت می‌کنند.
/// - ارتباط با لایه‌های پایین‌تر از طریق اینترفیس‌های تعریف شده در لایه‌ی Application انجام می‌شود.
///
/// Dependencies:
/// - وابسته به لایه‌های Application و Infrastructure.
/// - از کتابخانه‌های `System.Windows.Forms` و `Microsoft.Extensions.DependencyInjection` استفاده می‌کند.
/// </remarks>
namespace IEC104.Master.WinForms.Form
{
    /// <summary>
    /// فرم ارسال دستی درخواست به RTU
    /// </summary>
    /// <remarks>
    /// Role:
    /// - این فرم به کاربر امکان می‌دهد تا به‌صورت دستی و بدون نیاز به تنظیمات دوره‌ای، یک درخواست به RTU ارسال کند.
    /// - کاربر می‌تواند نوع درخواست (بازجویی عمومی، بازجویی گروهی، خواندن یک نقطه‌ی خاص) و پارامتر مربوطه (QOI یا IOA) را انتخاب کند.
    /// - نتیجه‌ی ارسال درخواست در یک TextBox نمایش داده می‌شود.
    ///
    /// Collaboration:
    /// - توسط `MainForm` از طریق دکمه‌ی "ارسال دستی" باز می‌شود.
    /// - از `IIec104MasterAppService` برای ارسال درخواست‌ها به RTU استفاده می‌کند.
    /// - نتیجه‌ی ارسال در فرم نمایش داده می‌شود و در صورت موفقیت، داده‌ها از طریق رویداد `MessagePublished` به `MainForm` ارسال می‌شوند.
    ///
    /// Lifecycle:
    /// - این فرم به صورت Transient (موقت) از طریق DI Container ساخته می‌شود.
    /// - طول عمر آن تا زمانی که کاربر آن را ببندد ادامه دارد.
    ///
    /// Constraints:
    /// - این فرم باید به صورت Modal (حالت دیالوگ) نمایش داده شود تا کاربر قبل از ادامه کار، درخواست را ارسال یا لغو کند.
    /// - پارامتر ورودی باید معتبر باشد؛ در غیر این صورت، پیام خطا نمایش داده می‌شود.
    /// - این فرم وابسته به اتصال فعال به RTU است؛ در صورت عدم اتصال، ارسال با خطا مواجه می‌شود.
    /// </remarks>
    public partial class SendRequestForm : System.Windows.Forms.Form
    {
        private readonly IIec104MasterAppService _appService;

        /// <summary>
        /// سازنده‌ی کلاس SendRequestForm
        /// </summary>
        /// <remarks>
        /// Purpose:
        /// - مقداردهی اولیه فرم و دریافت وابستگی‌ها.
        ///
        /// Preconditions:
        /// - پارامتر `appService` نباید null باشد.
        /// - `IIec104MasterAppService` باید قبلاً در DI Container به عنوان یک سرویس Singleton ثبت شده باشد.
        ///
        /// Workflow:
        /// 1. فراخوانی `InitializeComponent()` برای بارگذاری طراحی فرم.
        /// 2. ذخیره‌سازی `appService` در فیلد خصوصی.
        ///
        /// Side Effects:
        /// - هیچ اثر جانبی خاصی ندارد؛ فقط مقداردهی اولیه انجام می‌شود.
        ///
        /// Limitations:
        /// - این سازنده نباید شامل عملیات سنگین باشد، زیرا فرم باید سریع باز شود.
        /// </remarks>
        /// <param name="appService">سرویس اصلی برنامه برای ارسال درخواست‌ها به RTU</param>
        /// <exception cref="ArgumentNullException">در صورتی که `appService` null باشد.</exception>
        public SendRequestForm(IIec104MasterAppService appService)
        {
            InitializeComponent();
            _appService = appService;
        }

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی ارسال (Send)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - ارسال یک درخواست به RTU بر اساس نوع و پارامتر انتخاب‌شده توسط کاربر.
        ///
        /// Workflow:
        /// 1. **اعتبارسنجی ورودی‌ها**:
        ///    a. بررسی اینکه نوع درخواست انتخاب شده باشد.
        ///    b. بررسی اینکه پارامتر خالی نباشد.
        /// 2. غیرفعال کردن دکمه‌ی ارسال و تغییر متن آن به "⏳ در حال ارسال...".
        /// 3. تنظیم متن `txtResult` به "⏳ در حال ارسال درخواست...".
        /// 4. **ارسال درخواست بر اساس نوع**:
        ///    a. `GeneralInterrogation`:
        ///       - تبدیل پارامتر به `byte` (پیش‌فرض ۲۰).
        ///       - فراخوانی `_appService.SendGeneralInterrogationAsync`.
        ///    b. `GroupInterrogation`:
        ///       - تبدیل پارامتر به `byte` (پیش‌فرض ۲۰).
        ///       - فراخوانی `_appService.SendGeneralInterrogationAsync`.
        ///    c. `SinglePoint`:
        ///       - اعتبارسنجی اینکه پارامتر یک عدد صحیح (IOA) باشد.
        ///       - فراخوانی `_appService.SendSinglePointReadAsync`.
        ///    d. سایر موارد: نمایش پیام "نوع درخواست پشتیبانی نمی‌شود".
        /// 5. نمایش نتیجه‌ی موفقیت یا خطا در `txtResult`.
        /// 6. در صورت بروز خطا، نمایش پیام خطا در MessageBox.
        /// 7. در نهایت، فعال کردن مجدد دکمه و بازگرداندن متن آن.
        ///
        /// Side Effects:
        /// - ارسال درخواست به RTU و دریافت پاسخ (از طریق رویداد `MessagePublished` در `MainForm`).
        /// - به‌روزرسانی `txtResult` با نتیجه‌ی ارسال.
        ///
        /// Limitations:
        /// - پارامتر `QOI` برای `GeneralInterrogation` و `GroupInterrogation` به صورت عددی تفسیر می‌شود.
        /// - برای `SinglePoint`، پارامتر باید یک عدد صحیح باشد؛ اعتبارسنجی محدوده‌ی IOA انجام نمی‌شود.
        /// - این متد به اتصال فعال به RTU وابسته است؛ در صورت عدم اتصال، خطا دریافت می‌شود.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnSend)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
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

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی بستن (Close)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - بستن فرم ارسال دستی درخواست.
        ///
        /// Workflow:
        /// 1. فراخوانی `Close()` برای بستن فرم.
        ///
        /// Side Effects:
        /// - فرم بسته می‌شود و کنترل به فرم فراخوان (معمولاً `MainForm`) بازمی‌گردد.
        ///
        /// Limitations:
        /// - هیچ عملیات پاک‌سازی خاصی انجام نمی‌شود.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnClose)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// به‌روزرسانی متن نتیجه‌ی ارسال از بیرون (مثلاً پس از دریافت پاسخ از RTU)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - امکان به‌روزرسانی `txtResult` از خارج از فرم (مثلاً از `MainForm` یا سرویس‌ها) فراهم می‌کند.
        ///
        /// Workflow:
        /// 1. بررسی اینکه آیا در ترد اصلی هستیم (اگر نه، از `BeginInvoke` استفاده می‌کند).
        /// 2. تنظیم `txtResult.Text` به پیام دریافتی با پیشوند "✅ ".
        ///
        /// Side Effects:
        /// - تغییر متن `txtResult` در UI.
        ///
        /// Limitations:
        /// - این متد فقط متن را به‌روز می‌کند و هیچ عملیات دیگری انجام نمی‌دهد.
        /// - برای استفاده از این متد، باید فرم هنوز باز باشد و بسته نشده باشد.
        /// </remarks>
        /// <param name="message">پیامی که باید در نتیجه نمایش داده شود</param>
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