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
/// - از کتابخانه‌های `System.Windows.Forms` و `Microsoft.Extensions.DependencyInjection` استفاده می‌کند.
/// </remarks>
namespace IEC104.Master.WinForms.Form
{
    /// <summary>
    /// فرم مدیریت درخواست‌های دوره‌ای (Polling)
    /// </summary>
    /// <remarks>
    /// Role:
    /// - این فرم به کاربر امکان مدیریت کامل درخواست‌های دوره‌ای را می‌دهد.
    /// - کاربر می‌تواند درخواست‌ها را مشاهده، افزودن، ویرایش، حذف، فعال/غیرفعال و ارسال فوری کند.
    /// - این فرم با زمان‌بند (Scheduler) هماهنگ است و تغییرات را به آن اعمال می‌کند.
    ///
    /// Collaboration:
    /// - توسط `MainForm` از طریق دکمه‌ی "زمان‌بندی" باز می‌شود.
    /// - از `IPeriodicRequestRepository` برای عملیات دیتابیس استفاده می‌کند.
    /// - از `IPeriodicRequestScheduler` برای اعمال تغییرات و دریافت رویدادهای اجرا استفاده می‌کند.
    /// - از `PeriodicRequestEditForm` برای افزودن/ویرایش درخواست‌ها استفاده می‌کند.
    /// - از `IIec104Transport` برای ارسال فوری درخواست‌ها استفاده می‌کند.
    ///
    /// Lifecycle:
    /// - این فرم به صورت Transient (موقت) از طریق DI Container ساخته می‌شود.
    /// - طول عمر آن تا زمانی که کاربر آن را ببندد ادامه دارد.
    /// - در رویداد `Load`، لیست درخواست‌ها از دیتابیس بارگذاری می‌شود.
    ///
    /// Constraints:
    /// - این فرم باید به صورت Modal (حالت دیالوگ) نمایش داده شود تا کاربر قبل از ادامه کار، تنظیمات را تأیید یا لغو کند.
    /// - تمام عملیات‌های طولانی (مانند بارگذاری، ذخیره‌سازی، ارسال) باید به صورت غیرهمگام (Async) انجام شوند تا UI قفل نشود.
    /// - تغییرات در دیتابیس باید با `ReloadAsync` به زمان‌بند اعمال شوند.
    /// </remarks>
    public partial class PeriodicRequestsForm : System.Windows.Forms.Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPeriodicRequestScheduler _scheduler;
        private readonly BindingSource _bindingSource = new BindingSource();
        private List<PeriodicRequestDto> _requests = new();

        /// <summary>
        /// سازنده‌ی کلاس PeriodicRequestsForm
        /// </summary>
        /// <remarks>
        /// Purpose:
        /// - مقداردهی اولیه فرم و دریافت وابستگی‌ها.
        /// - اشتراک در رویداد `RequestExecuted` از زمان‌بند.
        ///
        /// Preconditions:
        /// - پارامتر `serviceProvider` نباید null باشد.
        /// - پارامتر `scheduler` نباید null باشد.
        ///
        /// Workflow:
        /// 1. فراخوانی `InitializeComponent()` برای بارگذاری طراحی فرم.
        /// 2. ذخیره‌سازی `serviceProvider` و `scheduler` در فیلدهای خصوصی.
        /// 3. اشتراک در رویداد `RequestExecuted` از `_scheduler`.
        ///
        /// Side Effects:
        /// - رویداد `RequestExecuted` به متد `OnSchedulerRequestExecuted` متصل می‌شود.
        ///
        /// Limitations:
        /// - این سازنده نباید شامل عملیات سنگین باشد، زیرا فرم باید سریع باز شود.
        /// </remarks>
        /// <param name="serviceProvider">سرویس‌دهنده برای ایجاد Scope و دریافت Repository</param>
        /// <param name="scheduler">زمان‌بند درخواست‌های دوره‌ای</param>
        /// <exception cref="ArgumentNullException">در صورتی که `serviceProvider` یا `scheduler` null باشند.</exception>
        public PeriodicRequestsForm(IServiceProvider serviceProvider, IPeriodicRequestScheduler scheduler)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _scheduler = scheduler;
            _scheduler.RequestExecuted += OnSchedulerRequestExecuted;
        }

        /// <summary>
        /// رویداد بارگذاری فرم مدیریت درخواست‌های دوره‌ای
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - پیکربندی DataGridView و بارگذاری لیست درخواست‌ها از دیتابیس.
        ///
        /// Workflow:
        /// 1. فراخوانی `ConfigureDataGridView()` برای تنظیم ستون‌ها.
        /// 2. فراخوانی `LoadRequestsAsync()` برای دریافت داده‌ها از دیتابیس.
        ///
        /// Side Effects:
        /// - تغییر تنظیمات DataGridView و پر شدن آن با داده‌ها.
        ///
        /// Limitations:
        /// - این رویداد به صورت `async void` است و خطاهای احتمالی باید در متد `LoadRequestsAsync` مدیریت شوند.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (فرم)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
        private async void PeriodicRequestsForm_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            await LoadRequestsAsync();
        }

        /// <summary>
        /// پیکربندی ظاهری DataGridView برای نمایش درخواست‌های دوره‌ای
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - تنظیم ستون‌های DataGridView به صورت دستی با عنوان‌های فارسی و اتصال به BindingSource.
        ///
        /// Workflow:
        /// 1. غیرفعال کردن `AutoGenerateColumns`.
        /// 2. پاک کردن ستون‌های موجود.
        /// 3. اضافه کردن ستون‌ها با `DataPropertyName` مناسب برای اتصال به `PeriodicRequestDto`.
        /// 4. تنظیم `DataSource` به `_bindingSource`.
        ///
        /// Side Effects:
        /// - تغییر ساختار ستون‌های DataGridView.
        ///
        /// Limitations:
        /// - این متد فرض می‌کند که `dgvRequests` قبلاً مقداردهی شده است.
        /// </remarks>
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

        /// <summary>
        /// بارگذاری لیست درخواست‌های دوره‌ای از دیتابیس
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - دریافت تمام درخواست‌ها از دیتابیس و تبدیل آنها به `PeriodicRequestDto` برای نمایش در UI.
        ///
        /// Workflow:
        /// 1. ایجاد یک Scope جدید از `IServiceProvider`.
        /// 2. دریافت `IPeriodicRequestRepository` از Scope.
        /// 3. فراخوانی `GetAllAsync()` برای دریافت تمام موجودیت‌ها.
        /// 4. تبدیل موجودیت‌ها به `PeriodicRequestDto`.
        /// 5. تنظیم `_bindingSource.DataSource` به لیست DTOها.
        /// 6. فراخوانی `ResetBindings(false)` برای به‌روزرسانی UI.
        ///
        /// Side Effects:
        /// - به‌روزرسانی DataGridView با داده‌های جدید.
        ///
        /// Limitations:
        /// - این متد از `async/await` استفاده می‌کند و باید با `await` فراخوانی شود.
        /// </remarks>
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

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی افزودن (Add)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - باز کردن فرم افزودن درخواست جدید و بارگذاری مجدد لیست در صورت تأیید.
        ///
        /// Workflow:
        /// 1. ایجاد نمونه‌ای از `PeriodicRequestEditForm` با `_editId = null`.
        /// 2. نمایش فرم به صورت Modal.
        /// 3. اگر نتیجه `OK` بود:
        ///    a. بارگذاری مجدد لیست با `LoadRequestsAsync()`.
        ///    b. بازخوانی زمان‌بند با `_scheduler.ReloadAsync()`.
        ///
        /// Side Effects:
        /// - در صورت تأیید، درخواست جدید در دیتابیس ذخیره می‌شود و زمان‌بند به‌روز می‌شود.
        ///
        /// Limitations:
        /// - این متد از `async void` استفاده می‌کند و خطاهای احتمالی باید در فرم `PeriodicRequestEditForm` مدیریت شوند.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnAdd)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            using var form = new PeriodicRequestEditForm(_serviceProvider, _scheduler);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                await LoadRequestsAsync();
                await _scheduler.ReloadAsync();
            }
        }

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی ویرایش (Edit)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - باز کردن فرم ویرایش درخواست انتخاب‌شده و بارگذاری مجدد لیست در صورت تأیید.
        ///
        /// Workflow:
        /// 1. بررسی اینکه آیا ردیفی انتخاب شده است.
        /// 2. اگر نه، پیام هشدار نمایش داده می‌شود.
        /// 3. اگر بله، DTO ردیف انتخاب‌شده را دریافت می‌کند.
        /// 4. ایجاد نمونه‌ای از `PeriodicRequestEditForm` با `_editId = dto.Id`.
        /// 5. نمایش فرم به صورت Modal.
        /// 6. اگر نتیجه `OK` بود:
        ///    a. بارگذاری مجدد لیست با `LoadRequestsAsync()`.
        ///    b. بازخوانی زمان‌بند با `_scheduler.ReloadAsync()`.
        ///
        /// Side Effects:
        /// - در صورت تأیید، درخواست در دیتابیس به‌روز می‌شود و زمان‌بند به‌روز می‌شود.
        ///
        /// Limitations:
        /// - این متد از `async void` استفاده می‌کند و خطاهای احتمالی باید در فرم `PeriodicRequestEditForm` مدیریت شوند.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnEdit)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
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

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی حذف (Delete)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - حذف درخواست انتخاب‌شده از دیتابیس پس از تأیید کاربر.
        ///
        /// Workflow:
        /// 1. بررسی اینکه آیا ردیفی انتخاب شده است.
        /// 2. اگر نه، پیام هشدار نمایش داده می‌شود.
        /// 3. اگر بله، DTO ردیف انتخاب‌شده را دریافت می‌کند.
        /// 4. نمایش پیام تأیید حذف به کاربر.
        /// 5. اگر کاربر تأیید کرد:
        ///    a. ایجاد Scope و دریافت Repository.
        ///    b. جستجوی موجودیت با `GetByIdAsync`.
        ///    c. اگر موجودیت یافت شد، آن را حذف و تغییرات را ذخیره می‌کند.
        ///    d. بارگذاری مجدد لیست.
        ///    e. بازخوانی زمان‌بند.
        ///
        /// Side Effects:
        /// - حذف درخواست از دیتابیس و به‌روزرسانی زمان‌بند.
        ///
        /// Limitations:
        /// - این متد از `async void` استفاده می‌کند و خطاهای احتمالی در `try-catch` مدیریت نمی‌شوند (می‌توان اضافه کرد).
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnDelete)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
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

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی تغییر وضعیت (Toggle Active)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - تغییر وضعیت فعال/غیرفعال یک درخواست و اعمال آن در دیتابیس و زمان‌بند.
        ///
        /// Workflow:
        /// 1. بررسی اینکه آیا ردیفی انتخاب شده است.
        /// 2. اگر نه، پیام هشدار نمایش داده می‌شود.
        /// 3. اگر بله، DTO ردیف انتخاب‌شده را دریافت می‌کند.
        /// 4. ایجاد Scope و دریافت Repository.
        /// 5. جستجوی موجودیت با `GetByIdAsync`.
        /// 6. اگر موجودیت یافت شد:
        ///    a. معکوس کردن `IsActive`.
        ///    b. به‌روزرسانی موجودیت و ذخیره‌سازی.
        ///    c. بارگذاری مجدد لیست.
        ///    d. بازخوانی زمان‌بند.
        ///
        /// Side Effects:
        /// - تغییر وضعیت درخواست در دیتابیس و زمان‌بند.
        ///
        /// Limitations:
        /// - این متد از `async void` استفاده می‌کند و خطاهای احتمالی در `try-catch` مدیریت نمی‌شوند (می‌توان اضافه کرد).
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnToggleActive)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
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

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی ارسال فوری (Send Now)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - ارسال فوری یک درخواست به RTU بدون انتظار برای بازه‌ی زمانی.
        ///
        /// Workflow:
        /// 1. بررسی اینکه آیا ردیفی انتخاب شده است.
        /// 2. اگر نه، پیام هشدار نمایش داده می‌شود.
        /// 3. اگر بله، DTO ردیف انتخاب‌شده را دریافت می‌کند.
        /// 4. ایجاد Scope و دریافت `IIec104Transport` و `IPeriodicRequestRepository`.
        /// 5. جستجوی موجودیت با `GetByIdAsync`.
        /// 6. اگر موجودیت یافت شد:
        ///    a. غیرفعال کردن دکمه و تغییر متن آن.
        ///    b. فراخوانی `SendRequestAsync` برای ارسال درخواست.
        ///    c. به‌روزرسانی `LastExecutedAt` و پاک کردن `ErrorMessage` در دیتابیس.
        ///    d. بارگذاری مجدد لیست و نمایش پیام موفقیت.
        /// 7. در صورت بروز خطا:
        ///    a. ثبت خطا در `ErrorMessage`.
        ///    b. بارگذاری مجدد لیست و نمایش پیام خطا.
        /// 8. در نهایت، فعال کردن مجدد دکمه و بازگرداندن متن آن.
        ///
        /// Side Effects:
        /// - ارسال درخواست به RTU و به‌روزرسانی وضعیت در دیتابیس.
        ///
        /// Limitations:
        /// - این متد از `async void` استفاده می‌کند و خطاها با `try-catch` مدیریت می‌شوند.
        /// - دکمه در حین ارسال غیرفعال می‌شود تا از کلیک مجدد جلوگیری شود.
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (دکمه btnSendNow)</param>
        /// <param name="e">آرگومان‌های رویداد</param>
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

        /// <summary>
        /// ارسال یک درخواست به RTU بر اساس نوع آن
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - ارسال درخواست به RTU با استفاده از `IIec104Transport` بر اساس نوع و پارامترهای درخواست.
        ///
        /// Workflow:
        /// 1. بررسی `request.Type`:
        ///    a. `GeneralInterrogation`: ارسال بازجویی عمومی با QOI از پارامتر (پیش‌فرض ۲۰).
        ///    b. `GroupInterrogation`: ارسال بازجویی گروهی با QOI از پارامتر (پیش‌فرض ۲۰).
        ///    c. `SinglePoint`: ارسال درخواست خواندن یک نقطه‌ی خاص با QOI از پارامتر (پیش‌فرض ۲۱) و Common Address = ۱.
        /// 2. اگر نوع پشتیبانی نشود، یک استثنا پرتاب می‌کند.
        ///
        /// Side Effects:
        /// - ارسال درخواست به RTU.
        ///
        /// Limitations:
        /// - برای `SinglePoint`، Common Address به صورت ثابت ۱ ارسال می‌شود. در صورت نیاز به پیکربندی، باید از تنظیمات خوانده شود.
        /// - پارامتر `QOI` برای `SinglePoint` به صورت عددی تفسیر می‌شود؛ ممکن است برخی RTUها مقدار خاصی را انتظار داشته باشند.
        /// </remarks>
        /// <param name="transport">سرویس حمل‌ونقل برای ارسال درخواست</param>
        /// <param name="request">موجودیت درخواست دوره‌ای</param>
        /// <exception cref="NotSupportedException">در صورتی که نوع درخواست پشتیبانی نشود.</exception>
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

        /// <summary>
        /// مدیریت رویداد اجرای یک درخواست توسط زمان‌بند
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - به‌روزرسانی خودکار لیست درخواست‌ها پس از اجرای خودکار زمان‌بند.
        ///
        /// Workflow:
        /// 1. بررسی اینکه آیا در ترد اصلی هستیم (اگر نه، از `BeginInvoke` استفاده می‌کند).
        /// 2. پیدا کردن آیتم مربوطه در `_requests` بر اساس `RequestId`.
        /// 3. به‌روزرسانی `LastExecutedAt` و `ErrorMessage` در آیتم.
        /// 4. فراخوانی `ResetBindings(false)` برای به‌روزرسانی UI.
        ///
        /// Side Effects:
        /// - به‌روزرسانی DataGridView با آخرین وضعیت اجرا.
        ///
        /// Limitations:
        /// - این متد فقط زمان و خطا را به‌روز می‌کند و وضعیت موفقیت را در `Status` نمایش نمی‌دهد (می‌توان اضافه کرد).
        /// </remarks>
        /// <param name="sender">فرستنده رویداد (معمولاً `PeriodicRequestScheduler`)</param>
        /// <param name="e">اطلاعات مربوط به اجرای درخواست</param>
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

        /// <summary>
        /// مدیریت کلیک روی دکمه‌ی بستن (Close)
        /// </summary>
        /// <remarks>
        /// Goal:
        /// - بستن فرم مدیریت درخواست‌ها.
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
    }
}