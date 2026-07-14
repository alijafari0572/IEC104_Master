using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
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
/// - وابسته به لایه‌های Application, Infrastructure و Domain.
/// - از کتابخانه‌های System.Windows.Forms و Microsoft.Extensions.DependencyInjection استفاده می‌کند.
/// </remarks>
namespace IEC104.Master.WinForms.Form
{
    /// <summary>
    /// فرم افزودن/ویرایش یک درخواست دوره‌ای
    /// </summary>
    /// <remarks>
    /// Role:
    /// - این فرم به کاربر اجازه می‌دهد تا یک درخواست دوره‌ای جدید ایجاد کند یا یک درخواست موجود را ویرایش کند.
    /// - پارامترهای درخواست شامل نام، نوع (General Interrogation, Group Interrogation, SinglePoint)، پارامتر (QOI یا IOA)، بازه‌ی زمانی (دقیقه) و وضعیت فعال/غیرفعال می‌باشد.
    /// - پس از تأیید کاربر، درخواست در دیتابیس ذخیره می‌شود و زمان‌بند (Scheduler) به‌روز می‌شود.
    ///
    /// Collaboration:
    /// - توسط PeriodicRequestsForm برای افزودن/ویرایش درخواست‌ها استفاده می‌شود.
    /// - از IPeriodicRequestRepository برای عملیات دیتابیس استفاده می‌کند.
    /// - از IServiceProvider برای ایجاد Scope و دریافت Repository استفاده می‌کند.
    /// - خروجی این فرم (از طریق DialogResult.OK) به PeriodicRequestsForm اطلاع می‌دهد که زمان‌بند باید Reload شود.
    ///
    /// Lifecycle:
    /// - این فرم به صورت Transient (موقت) از طریق DI Container ساخته می‌شود.
    /// - طول عمر آن تنها به مدت زمان نمایش به کاربر محدود است و پس از بسته شدن، نابود می‌شود.
    ///
    /// Constraints:
    /// - این فرم باید به صورت Modal (حالت دیالوگ) نمایش داده شود تا کاربر قبل از ادامه کار، تنظیمات را تأیید یا لغو کند.
    /// - تمام فیلدها باید مقادیر معتبر داشته باشند؛ در غیر این صورت، پیام خطا نمایش داده می‌شود.
    /// - برای نوع SinglePoint، پارامتر باید یک عدد صحیح (IOA) باشد.
    /// </remarks>
    public partial class PeriodicRequestEditForm : System.Windows.Forms.Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly int? _editId;
        private PeriodicRequest _entity;

        /// <summary>
        /// سازنده‌ی کلاس PeriodicRequestEditForm
        /// </summary>
        /// <remarks>
        /// Purpose:
        /// - مقداردهی اولیه فرم و دریافت وابستگی‌ها.
        ///
        /// Preconditions:
        /// - پارامتر serviceProvider نباید null باشد.
        /// - پارامتر scheduler در حال حاضر استفاده نمی‌شود، اما برای هماهنگی با DI Container حفظ شده است.
        ///
        /// Workflow:
        /// 1. فراخوانی InitializeComponent() برای بارگذاری طراحی فرم.
        /// 2. ذخیره‌سازی serviceProvider و editId در فیلدهای خصوصی.
        /// 3. مقداردهی اولیه _entity به یک نمونه‌ی جدید از PeriodicRequest.
        ///
        /// Side Effects:
        /// - هیچ اثر جانبی خاصی ندارد؛ فقط مقداردهی اولیه انجام می‌شود.
        ///
        /// Limitations:
        /// - این سازنده نباید شامل عملیات سنگین باشد، زیرا فرم باید سریع باز شود.
        /// - پارامتر scheduler در حال حاضر استفاده نمی‌شود و فقط برای تزریق وابستگی نگهداری می‌شود.
        /// </remarks>
        /// <param name="serviceProvider">سرویس‌دهنده برای ایجاد Scope و دریافت Repository</param>
        /// <param name="scheduler">زمان‌بند درخواست‌های دوره‌ای (در حال حاضر استفاده نمی‌شود)</param>
        /// <param name="editId">شناسه‌ی درخواست در صورت ویرایش (در غیر این صورت null)</param>
        /// <exception cref="ArgumentNullException">در صورتی که serviceProvider null باشد.</exception>
        public PeriodicRequestEditForm(IServiceProvider serviceProvider, IPeriodicRequestScheduler scheduler, int? editId = null)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _editId = editId;
            _entity = new PeriodicRequest();
        }

        /// <summary>
        /// رویداد بارگذاری فرم افزودن/ویرایش درخواست
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - تنظیم عنوان فرم و بارگذاری داده‌های درخواست در صورت ویرایش.
        ///
        /// Workflow:
        /// 1. اگر _editId مقدار داشته باشد (حالت ویرایش):
        /// a. تنظیم عنوان فرم به "✏️ ویرایش درخواست".
        /// b. فراخوانی LoadForEditAsync برای بارگذاری داده‌های درخواست.
        /// 2. اگر _editId مقدار نداشته باشد (حالت افزودن):
        /// a. تنظیم عنوان فرم به "➕ افزودن درخواست جدید".
        /// b. تنظیم cmbType.SelectedIndex به ۰ (اولین گزینه).
        ///
        /// Side Effects:
        /// - تغییر عنوان فرم و پر کردن فیلدها (در حالت ویرایش).
        /// - اگر در حالت ویرایش، درخواست در دیتابیس جستجو می‌شود و فیلدها با مقادیر آن پر می‌شوند.
        ///
        /// Limitations:
        /// - اگر در حالت ویرایش، درخواست در دیتابیس پیدا نشود، فرم با پیام خطا بسته می‌شود.
        /// - این رویداد به صورت async void است و خطاهای احتمالی باید در متد LoadForEditAsync مدیریت شوند.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (فرم)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
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

        /// <summary>
        /// بارگذاری داده‌های یک درخواست موجود برای ویرایش
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - دریافت اطلاعات یک درخواست از دیتابیس و نمایش آن در فیلدهای فرم.
        ///
        /// Workflow:
        /// 1. ایجاد یک Scope جدید از IServiceProvider.
        /// 2. دریافت IPeriodicRequestRepository از Scope.
        /// 3. جستجوی درخواست با شناسه‌ی _editId در دیتابیس.
        /// 4. اگر درخواست یافت نشد:
        /// a. نمایش پیام خطا به کاربر.
        /// b. تنظیم DialogResult به Cancel و بستن فرم.
        /// 5. اگر درخواست یافت شد:
        /// a. پر کردن فیلدهای فرم با مقادیر موجودیت.
        ///
        /// Side Effects:
        /// - تغییر مقادیر فیلدهای فرم.
        /// - در صورت عدم یافتن درخواست، فرم بسته می‌شود.
        ///
        /// Limitations:
        /// - این متد از async/await استفاده می‌کند، بنابراین فراخوانی آن باید با await همراه باشد.
        /// - اگر درخواست یافت نشود، فرم بسته می‌شود و کاربر باید دوباره اقدام کند.
        /// </remarks>
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

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی ذخیره (Save)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - اعتبارسنجی ورودی‌های کاربر و ذخیره‌سازی درخواست در دیتابیس.
        ///
        /// Workflow:
        /// 1. اعتبارسنجی اولیه:
        /// a. بررسی نام (نباید خالی باشد).
        /// b. بررسی نوع (نباید خالی باشد).
        /// c. بررسی پارامتر (نباید خالی باشد).
        /// 2. تبدیل نوع درخواست به Enum:
        /// a. تبدیل cmbType.SelectedItem به RequestType با استفاده از Enum.TryParse.
        /// 3. اعتبارسنجی ویژه برای SinglePoint:
        /// a. اگر نوع SinglePoint باشد، پارامتر باید یک عدد صحیح (IOA) باشد.
        /// 4. ذخیره‌سازی در دیتابیس:
        /// a. اگر _editId مقدار داشته باشد (حالت ویرایش):
        /// - به‌روزرسانی موجودیت _entity با مقادیر جدید.
        /// - فراخوانی repository.Update.
        /// b. اگر _editId مقدار نداشته باشد (حالت افزودن):
        /// - ایجاد یک PeriodicRequest جدید.
        /// - فراخوانی repository.AddAsync.
        /// 5. ذخیره‌سازی تغییرات با repository.SaveChangesAsync.
        /// 6. تنظیم DialogResult به OK و بستن فرم.
        ///
        /// Side Effects:
        /// - ذخیره‌سازی داده‌ها در دیتابیس.
        /// - در صورت بروز خطا، نمایش پیام خطا به کاربر.
        ///
        /// Limitations:
        /// - اعتبارسنجی پارامتر برای SinglePoint فقط بررسی می‌کند که پارامتر یک عدد باشد؛ اعتبارسنجی محدوده‌ی IOA انجام نمی‌شود.
        /// - خطاهای دیتابیس (مانند محدودیت یکتایی) با پیام عمومی به کاربر نمایش داده می‌شوند.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnSave)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
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

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی انصراف (Cancel)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - لغو عملیات و بستن فرم با نتیجه‌ی Cancel.
        ///
        /// Workflow:
        /// 1. تنظیم DialogResult به DialogResult.Cancel.
        /// 2. بستن فرم با فراخوانی Close().
        ///
        /// Side Effects:
        /// - فرم بسته می‌شود و کنترل به فرم فراخوان (معمولاً PeriodicRequestsForm) بازمی‌گردد.
        /// - DialogResult به Cancel تنظیم می‌شود تا فرم فراخوان بداند که کاربر عملیات را لغو کرده است.
        ///
        /// Limitations:
        /// - هیچ اعتبارسنجی یا عملیات پاک‌سازی خاصی انجام نمی‌شود، زیرا کاربر قصد لغو را داشته است.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnCancel)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}