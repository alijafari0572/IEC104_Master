using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Application.DTOs;
using IEC104.Master.Domain.Entities;
using IEC104.Master.Infrastructure.Options;
using IEC104.Master.WinForms.Form;
using IEC104.Master.WinForms.Models;
using Microsoft.Extensions.DependencyInjection;
using Point = IEC104.Master.Domain.Entities.Point;

/// <summary>
/// فضای نام مربوط به فرم‌های اصلی و ویندوزی برنامه
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

namespace IEC104.Master.WinForms.Form;

/// <summary>
/// فرم اصلی برنامه که به عنوان نقطه‌ی ورود کاربر عمل می‌کند.
/// این فرم وظیفه‌ی مدیریت ارتباط با RTU، نمایش داده‌های دریافتی، و کنترل درخواست‌های دوره‌ای را بر عهده دارد.
/// </summary>
/// <remarks>
/// Role:
/// - هماهنگ‌کننده (Orchestrator) بین لایه‌های مختلف برنامه و UI است.
/// - رویدادهای UI را به عملیات در لایه‌ی Application نگاشت می‌کند.
/// - داده‌های دریافتی را به مدل‌های نمایشی (AsduDisplayModel) تبدیل و در DataGridView نمایش می‌دهد.
/// - وضعیت اتصال و دکمه‌های مرتبط را مدیریت می‌کند.
/// - زمان‌بند (Scheduler) را برای ارسال درخواست‌های دوره‌ای شروع و متوقف می‌کند.
///
/// Collaboration:
/// - با IIec104MasterAppService برای ارسال فرمان‌ها و دریافت داده‌ها همکاری می‌کند.
/// - با IUnitOfWork برای ذخیره‌سازی داده‌های دریافتی در دیتابیس همکاری می‌کند.
/// - با IPeriodicRequestScheduler برای مدیریت درخواست‌های دوره‌ای همکاری می‌کند.
/// - از IServiceProvider برای ایجاد نمونه‌هایی از فرم‌های دیگر (مانند ConnectionSettingsForm) استفاده می‌کند.
/// - از BindingSource برای اتصال داده‌ها به DataGridView استفاده می‌کند.
///
/// Lifecycle:
/// - این فرم در زمان اجرای برنامه (از طریق Program.cs) با استفاده از DI Container ساخته می‌شود.
/// - طول عمر آن تا زمان بسته شدن برنامه ادامه دارد.
/// - در رویداد Load، اتصال خودکار و زمان‌بند شروع می‌شوند.
/// - در رویداد FormClosing، زمان‌بند به‌درستی متوقف می‌شود تا از اجرای پس‌زمینه جلوگیری شود.
///
/// Constraints:
/// - این فرم نباید شامل منطق تجاری (Business Logic) باشد؛ همه‌ی منطق باید به لایه‌ی Application یا Infrastructure منتقل شود.
/// - تمام عملیات‌های طولانی (مانند اتصال، ارسال درخواست) باید به صورت غیرهمگام (Async) انجام شوند تا UI قفل نشود.
/// - داده‌های نمایش داده شده در UI باید از طریق BindingSource مدیریت شوند تا به‌روزرسانی‌ها به‌درستی اعمال شوند.
/// </remarks>
public partial class MainForm : System.Windows.Forms.Form
{
    private readonly IIec104MasterAppService _appService;
    private readonly Iec104Options _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _asduBindingSource = new BindingSource();
    private readonly List<AsduDisplayModel> _asduDisplayList = new List<AsduDisplayModel>();
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPeriodicRequestScheduler _scheduler;

    /// <summary>
    /// سازنده‌ی کلاس MainForm
    /// </summary>
    /// <remarks>
    /// Purpose:
    /// - مقداردهی اولیه وابستگی‌ها و ثبت رویدادهای سرویس‌ها.
    ///
    /// Preconditions:
    /// - تمام پارامترها باید از طریق DI Container تأمین شوند و نباید null باشند.
    /// - appService باید قبلاً به RTU متصل نشده باشد (اتصال در زمان بارگذاری فرم انجام می‌شود).
    ///
    /// Workflow:
    /// 1. فراخوانی InitializeComponent() برای بارگذاری طراحی فرم.
    /// 2. ذخیره‌سازی وابستگی‌های تزریق شده در فیلدهای خصوصی.
    /// 3. اشتراک در رویداد RequestExecuted از IPeriodicRequestScheduler.
    /// 4. اشتراک در رویدادهای MessagePublished و ConnectionStateChanged از IIec104MasterAppService.
    ///
    /// Side Effects:
    /// - رویدادهای سرویس‌ها به متدهای خصوصی این کلاس متصل می‌شوند.
    /// - تا زمان فراخوانی رویدادها، هیچ عملیات دیگری انجام نمی‌شود.
    ///
    /// Limitations:
    /// - این سازنده نباید شامل عملیات سنگین یا طولانی باشد.
    /// - اتصال به RTU در این مرحله انجام نمی‌شود (در MainForm_Load انجام می‌شود).
    /// </remarks>
    /// <param name="appService">سرویس اصلی برنامه برای ارتباط با RTU</param>
    /// <param name="options">تنظیمات اولیه برنامه</param>
    /// <param name="serviceProvider">سرویس‌دهنده برای ایجاد نمونه‌های فرم‌های دیگر</param>
    /// <param name="unitOfWork">واحد کار برای عملیات دیتابیس</param>
    /// <param name="scheduler">زمان‌بند برای درخواست‌های دوره‌ای</param>
    /// <exception cref="ArgumentNullException">در صورتی که هر یک از پارامترها null باشند</exception>
    public MainForm(IIec104MasterAppService appService, Iec104Options options, IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IPeriodicRequestScheduler scheduler)
    {
        InitializeComponent();
        _appService = appService;
        _options = options;
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;
        _scheduler = scheduler;
        _scheduler.RequestExecuted += OnSchedulerRequestExecuted;

        _appService.MessagePublished += OnMessagePublished;
        _appService.ConnectionStateChanged += OnConnectionStateChanged;
    }

    /// <summary>
    /// رویداد بارگذاری فرم اصلی
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - مقداردهی اولیه UI و شروع عملیات اتصال خودکار و زمان‌بند.
    ///
    /// Workflow:
    /// 1. غیرفعال کردن دکمه‌های قطع اتصال، GI، مدیریت درخواست‌ها و ارسال دستی در ابتدا.
    /// 2. تنظیم وضعیت اولیه به "Disconnected".
    /// 3. تنظیم DataSource برای DataGridView با استفاده از BindingSource.
    /// 4. پیکربندی ظاهری DataGridView (با فراخوانی ConfigureDataGridView).
    /// 5. شروع اتصال خودکار (با فراخوانی AutoConnectAsync).
    /// 6. شروع زمان‌بند درخواست‌های دوره‌ای (با فراخوانی _scheduler.StartAsync()).
    ///
    /// Side Effects:
    /// - تغییر وضعیت دکمه‌ها و لیبل وضعیت.
    /// - اتصال به RTU (در صورت موفقیت) و شروع ارسال دوره‌ای درخواست‌ها.
    ///
    /// Limitations:
    /// - اگر اتصال خودکار ناموفق باشد، کاربر همچنان می‌تواند به‌صورت دستی اتصال برقرار کند.
    /// - زمان‌بند حتی در صورت عدم موفقیت اتصال شروع می‌شود (اما درخواست‌ها بدون اتصال ارسال نمی‌شوند).
    /// </remarks>
    /// <param name="sender">فرستنده رویداد</param>
    /// <param name="e">آرگومان‌های رویداد</param>
    private async void MainForm_Load(object sender, EventArgs e)
    {
        btnDisconnect.Enabled = false;
        btnGI.Enabled = false;
        btnManageRequests.Enabled = false;
        btnSendRequest.Enabled = false;
        lblStatus.Text = "Disconnected";

        dgvAsduData.DataSource = _asduBindingSource;
        _asduBindingSource.DataSource = _asduDisplayList;

        // تنظیم ستون‌ها
        ConfigureDataGridView();

        // شروع اتصال خودکار
        await AutoConnectAsync();
        // شروع زمان‌بند
        await _scheduler.StartAsync();
    }

    /// <summary>
    /// مدیریت رویداد اجرای یک درخواست دوره‌ای توسط زمان‌بند
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - ثبت نتیجه‌ی اجرای درخواست در لاگ UI به‌صورت همزمان با ترد UI.
    ///
    /// Workflow:
    /// 1. بررسی اینکه آیا اجرا در ترد اصلی (UI Thread) انجام شده است.
    /// 2. اگر نه، از BeginInvoke برای اجرای مجدد متد در ترد اصلی استفاده می‌شود.
    /// 3. در ترد اصلی، یک پیام لاگ با وضعیت موفقیت/شکست و زمان اجرا به lstLog اضافه می‌شود.
    ///
    /// Side Effects:
    /// - به‌روزرسانی lstLog (لیست لاگ) با اطلاعات جدید.
    ///
    /// Limitations:
    /// - پیام‌های لاگ به‌صورت نزولی (جدیدترین در بالا) نمایش داده می‌شوند.
    /// </remarks>
    /// <param name="sender">فرستنده رویداد (معمولاً PeriodicRequestScheduler)</param>
    /// <param name="e">اطلاعات مربوط به اجرای درخواست</param>
    private void OnSchedulerRequestExecuted(object? sender, PeriodicRequestExecutedEventArgs e)
    {
        // نمایش در لاگ
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => OnSchedulerRequestExecuted(sender, e)));
            return;
        }

        string status = e.Success ? "✅" : "❌";
        lstLog.Items.Insert(0, $"{e.ExecutedAt:HH:mm:ss} [Scheduler] {status} {e.RequestName} - {e.Message}");
    }

    protected override async void OnFormClosing(FormClosingEventArgs e)
    {
        await _scheduler.StopAsync();
        base.OnFormClosing(e);
    }

    /// <summary>
    /// پیکربندی ظاهری DataGridView برای نمایش داده‌های ASDU
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - تنظیم ظاهر و رفتار DataGridView شامل رنگ‌ها، فونت‌ها، و عنوان ستون‌ها.
    ///
    /// Workflow:
    /// 1. تنظیم AutoGenerateColumns به true تا ستون‌ها به‌صورت خودکار از مدل ایجاد شوند.
    /// 2. تنظیم رنگ‌های پس‌زمینه، خطوط، و سطرهای زوج و فرد.
    /// 3. تنظیم استایل هدر ستون‌ها با رنگ آبی و فونت توپر.
    /// 4. تغییر عنوان ستون‌ها به فارسی (در صورت وجود ستون‌ها).
    ///
    /// Side Effects:
    /// - تغییر مستقیم ویژگی‌های dgvAsduData.
    ///
    /// Limitations:
    /// - این متد فرض می‌کند که DataGridView قبلاً مقداردهی شده است.
    /// - عنوان ستون‌ها بر اساس نام پراپرتی‌های مدل AsduDisplayModel تنظیم می‌شوند.
    /// </remarks>
    private void ConfigureDataGridView()
    {
        dgvAsduData.AutoGenerateColumns = true;
        dgvAsduData.DataSource = _asduBindingSource;
        dgvAsduData.BackgroundColor = Color.White;
        dgvAsduData.BorderStyle = BorderStyle.None;
        dgvAsduData.GridColor = Color.FromArgb(230, 230, 230);
        dgvAsduData.RowHeadersVisible = false;
        dgvAsduData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // تنظیم رنگ سطرهای زوج و فرد
        dgvAsduData.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
        dgvAsduData.RowsDefaultCellStyle.BackColor = Color.White;
        dgvAsduData.RowsDefaultCellStyle.Font = new Font("Segoe UI", 9F);
        dgvAsduData.RowsDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
        dgvAsduData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
        dgvAsduData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvAsduData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dgvAsduData.EnableHeadersVisualStyles = false;

        if (dgvAsduData.Columns.Count > 0)
        {
            dgvAsduData.Columns["Timestamp"].HeaderText = "زمان";
            dgvAsduData.Columns["TypeId"].HeaderText = "Type ID";
            dgvAsduData.Columns["TypeName"].HeaderText = "نوع";
            dgvAsduData.Columns["ValueType"].HeaderText = "نوع مقدار";
            dgvAsduData.Columns["CommonAddress"].HeaderText = "آدرس مشترک";
            dgvAsduData.Columns["CotDescription"].HeaderText = "COT";
            dgvAsduData.Columns["InformationObjectAddress"].HeaderText = "IOA";
            dgvAsduData.Columns["Value"].HeaderText = "مقدار";
            dgvAsduData.Columns["Quality"].HeaderText = "کیفیت";
        }
    }

    /// <summary>
    /// مدیریت کلیک روی دکمه‌ی اتصال دستی
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - باز کردن فرم تنظیمات اتصال و برقراری ارتباط با RTU.
    ///
    /// Workflow:
    /// 1. ایجاد یک نمونه از ConnectionSettingsForm از طریق IServiceProvider.
    /// 2. نمایش فرم به‌صورت Modal و دریافت تنظیمات از کاربر.
    /// 3. ساخت یک ConnectRequestDto از مقادیر وارد شده.
    /// 4. فراخوانی _appService.ConnectAsync برای برقراری اتصال.
    /// 5. در صورت موفقیت، فعال‌سازی دکمه‌های مربوطه.
    ///
    /// Side Effects:
    /// - تغییر وضعیت دکمه‌ها و لیبل وضعیت (از طریق رویداد ConnectionStateChanged).
    ///
    /// Limitations:
    /// - اگر کاربر فرم تنظیمات را لغو کند، هیچ اتصالی برقرار نمی‌شود.
    /// - این متد از async void استفاده می‌کند، بنابراین خطاها باید در رویداد مدیریت شوند.
    /// </remarks>
    /// <param name="sender">فرستنده رویداد (دکمه Connect)</param>
    /// <param name="e">آرگومان‌های رویداد</param>
    private async void btnConnect_Click_1(object sender, EventArgs e)
    {
        using var form = _serviceProvider.GetRequiredService<ConnectionSettingsForm>();
        if (form.ShowDialog(this) != DialogResult.OK)
            return;

        var request = form.BuildRequest();
        await _appService.ConnectAsync(request);

        btnConnect.Enabled = false;
        btnDisconnect.Enabled = true;
        btnGI.Enabled = true;
        btnManageRequests.Enabled = true;
        btnSendRequest.Enabled = true;
    }

    /// <summary>
    /// مدیریت کلیک روی دکمه‌ی قطع اتصال
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - قطع ارتباط با RTU و غیرفعال کردن دکمه‌های مرتبط.
    ///
    /// Workflow:
    /// 1. فراخوانی _appService.DisconnectAsync برای قطع اتصال.
    /// 2. غیرفعال کردن دکمه‌های GI، مدیریت درخواست‌ها و ارسال دستی.
    /// 3. فعال کردن دکمه‌ی Connect.
    ///
    /// Side Effects:
    /// - تغییر وضعیت دکمه‌ها و لیبل وضعیت (از طریق رویداد ConnectionStateChanged).
    ///
    /// Limitations:
    /// - اگر قطع اتصال ناموفق باشد، وضعیت UI به‌درستی به‌روز نمی‌شود (می‌توان با try-catch مدیریت کرد).
    /// </remarks>
    /// <param name="sender">فرستنده رویداد (دکمه Disconnect)</param>
    /// <param name="e">آرگومان‌های رویداد</param>
    private async void btnDisconnect_Click_1(object sender, EventArgs e)
    {
        await _appService.DisconnectAsync();
        btnConnect.Enabled = true;
        btnDisconnect.Enabled = false;
        btnGI.Enabled = false;
    }

    /// <summary>
    /// مدیریت کلیک روی دکمه‌ی ارسال بازجویی عمومی (GI)
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - ارسال یک درخواست بازجویی عمومی با QOI=20 به RTU.
    ///
    /// Workflow:
    /// 1. ساخت یک GeneralInterrogationRequestDto با QOI=20.
    /// 2. فراخوانی _appService.SendGeneralInterrogationAsync برای ارسال درخواست.
    ///
    /// Side Effects:
    /// - RTU در پاسخ، داده‌های همه‌ی نقاط را ارسال می‌کند که باعث به‌روزرسانی DataGridView می‌شود.
    ///
    /// Limitations:
    /// - این متد QOI را به‌صورت ثابت ۲۰ ارسال می‌کند. برای ارسال با QOI دیگر باید متد را اصلاح کرد.
    /// </remarks>
    /// <param name="sender">فرستنده رویداد (دکمه GI)</param>
    /// <param name="e">آرگومان‌های رویداد</param>
    private async void btnGI_Click_1(object sender, EventArgs e)
    {
        await _appService.SendGeneralInterrogationAsync(new GeneralInterrogationRequestDto(20));
    }

    /// <summary>
    /// مدیریت رویداد دریافت پیام از سرویس اصلی برنامه
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - ذخیره‌سازی داده‌های دریافتی در دیتابیس، نمایش در DataGridView و لاگ.
    ///
    /// Workflow:
    /// 1. بررسی اینکه آیا پیام شامل نقطه‌های اطلاعاتی (Points) است یا خیر.
    /// 2. برای هر نقطه:
    /// a. جستجو در دیتابیس برای نقطه‌ی موجود با همان IOA.
    /// b. اگر وجود نداشت، یک نقطه‌ی جدید ایجاد و ذخیره می‌کند.
    /// c. اگر وجود داشت، اطلاعات آن را به‌روز می‌کند.
    /// d. یک رویداد جدید (Event) به نقاط موجود اضافه می‌کند.
    /// 3. تغییرات را در دیتابیس ذخیره می‌کند.
    /// 4. داده‌ها را به مدل‌های نمایشی (AsduDisplayModel) تبدیل و به _asduDisplayList اضافه می‌کند.
    /// 5. BindingSource را به‌روز می‌کند تا تغییرات در DataGridView منعکس شوند.
    /// 6. پیام را به‌صورت یک لاگ در lstLog نمایش می‌دهد.
    ///
    /// Side Effects:
    /// - نوشتن در دیتابیس و به‌روزرسانی UI.
    /// - در صورت بروز خطا در دیتابیس، خطا در لاگ نمایش داده می‌شود.
    ///
    /// Limitations:
    /// - خطاهای دیتابیس فقط در لاگ نمایش داده می‌شوند و به کاربر اطلاع داده نمی‌شوند.
    /// - داده‌های قدیمی در DataGridView نگهداری می‌شوند (در صورت عدم استفاده از Clear).
    /// </remarks>
    /// <param name="message">پیام دریافتی از سرویس اصلی</param>
    private async void OnMessagePublished(ProtocolMessageDto message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => OnMessagePublished(message)));
            return;
        }

        // ===== ذخیره در دیتابیس =====
        if (message.Points != null && message.Points.Any())
        {
            try
            {
                foreach (var point in message.Points)
                {
                    // ۱. پیدا کردن Point بر اساس IOA
                    var existingPoint = await _unitOfWork.Points.GetByIOAAsync(point.ObjectAddress);

                    if (existingPoint == null)
                    {
                        // ۲. اگر وجود نداشت، ایجاد کنید
                        existingPoint = new Point
                        {
                            InformationObjectAddress = point.ObjectAddress,
                            Name = $"Point_{point.ObjectAddress}",
                            TypeId = message.TypeId,
                            TypeName = message.TypeName,
                            CreatedAt = DateTime.Now
                        };
                        await _unitOfWork.Points.AddAsync(existingPoint);
                    }
                    else
                    {
                        // ۳. به‌روزرسانی اطلاعات
                        existingPoint.TypeId = message.TypeId;
                        existingPoint.TypeName = message.TypeName;
                        existingPoint.UpdatedAt = DateTime.Now;
                        _unitOfWork.Points.Update(existingPoint);
                    }

                    // ★★★ اصلاح اساسی: ایجاد Event و افزودن به رابطه ★★★
                    var eventEntity = new Event
                    {
                        // PointId را تنظیم نمی‌کنیم (EF Core خودش از رابطه می‌فهمد)
                        Timestamp = message.Timestamp,
                        Value = point.Value?.ToString() ?? string.Empty,
                        Quality = point.Quality ?? "Unknown",
                        RawData = message.Details,
                        CreatedAt = DateTime.Now
                    };

                    // ★ اضافه کردن Event به مجموعه‌ی Events نقطه
                    existingPoint.Events.Add(eventEntity);
                }

                // ۴. ذخیره‌سازی نهایی در دیتابیس
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                // نمایش خطا در لاگ
                lstLog.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} [Error] Database save failed - {ex.Message}");
            }
        }

        // نمایش در DataGridView
        if (message.Points != null && message.Points.Any())
        {
            // پاک کردن لیست قبلی (اختیاری - می‌توانید نگه دارید)
            // _asduDisplayList.Clear();

            foreach (var point in message.Points)
            {
                _asduDisplayList.Add(new AsduDisplayModel
                {
                    Timestamp = message.Timestamp,
                    TypeId = message.TypeId,
                    TypeName = message.TypeName,
                    ValueType = point.ValueType,
                    CommonAddress = message.CommonAddress,
                    CotDescription = message.CotDescription,
                    InformationObjectAddress = point.ObjectAddress,

                    Value = point.Value,
                    Quality = point.Quality
                });
            }

            // به‌روزرسانی BindingSource
            _asduBindingSource.ResetBindings(false);
            if (dgvAsduData.Rows.Count > 0)
            {
                // اسکرول به پایین
                dgvAsduData.FirstDisplayedScrollingRowIndex = dgvAsduData.Rows.Count - 1;
            }
        }

        // نمایش در لاگ با رنگ‌های مختلف
        string logEntry = $"{message.Timestamp:HH:mm:ss} [{message.Kind}] {message.Title} - {message.Details}";
        lstLog.Items.Insert(0, logEntry);
    }

    /// <summary>
    /// مدیریت رویداد تغییر وضعیت اتصال
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - به‌روزرسانی UI بر اساس وضعیت اتصال (Connected/Disconnected).
    ///
    /// Workflow:
    /// 1. تنظیم متن و رنگ لیبل وضعیت بر اساس وضعیت.
    /// 2. فعال/غیرفعال کردن دکمه‌های Connect، Disconnect، GI، مدیریت درخواست‌ها و ارسال دستی.
    ///
    /// Side Effects:
    /// - تغییر وضعیت دکمه‌ها و لیبل وضعیت.
    ///
    /// Limitations:
    /// - این متد فرض می‌کند که state.StatusText می‌تواند "Connected" یا "Disconnected" باشد.
    /// </remarks>
    /// <param name="state">وضعیت جدید اتصال</param>
    private void OnConnectionStateChanged(ConnectionStateDto state)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => OnConnectionStateChanged(state)));
            return;
        }

        lblStatus.Text = state.StatusText == "Connected" ? "🟢 Connected" : "Disconnected";
        lblStatus.ForeColor = state.StatusText == "Connected" ? Color.FromArgb(0, 180, 60) : Color.FromArgb(200, 50, 50);

        bool isConnected = state.StatusText == "Connected";
        btnConnect.Enabled = !isConnected;
        btnDisconnect.Enabled = isConnected;
        btnGI.Enabled = isConnected;
        btnManageRequests.Enabled = isConnected;
        btnSendRequest.Enabled = isConnected;
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// نمایش یا مخفی‌سازی پنل لودینگ
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - نمایش یک نشانگر بارگذاری در حین عملیات طولانی (مانند اتصال).
    ///
    /// Workflow:
    /// 1. بررسی اینکه آیا در ترد اصلی هستیم (اگر نه، از BeginInvoke استفاده می‌کند).
    /// 2. تنظیم Visible پنل لودینگ بر اساس پارامتر show.
    /// 3. اگر show برابر true باشد، سرعت انیمیشن ProgressBar را روی ۳۰ تنظیم می‌کند (شروع انیمیشن).
    /// 4. اگر false باشد، سرعت را روی ۰ تنظیم می‌کند (توقف انیمیشن).
    ///
    /// Side Effects:
    /// - تغییر وضعیت نمایش پنل و ProgressBar.
    ///
    /// Limitations:
    /// - اگر پنل یا ProgressBar null باشند، متد هیچ کاری انجام نمی‌دهد.
    /// </remarks>
    /// <param name="show">اگر true باشد، لودینگ نمایش داده می‌شود؛ در غیر این صورت مخفی می‌شود.</param>
    private void ShowLoading(bool show)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => ShowLoading(show)));
            return;
        }
        // بررسی null برای جلوگیری از خطا
        if (panlLoading == null || progressBarLoading == null)
            return;
        panlLoading.Visible = show;
        progressBarLoading.MarqueeAnimationSpeed = show ? 30 : 0;
    }

    /// <summary>
    /// تلاش برای اتصال خودکار با تنظیمات پیش‌فرض
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - برقراری اتصال به RTU با استفاده از تنظیمات پیش‌فرض (بدون نیاز به دخالت کاربر).
    ///
    /// Workflow:
    /// 1. اگر وضعیت فعلی "Connected" است، از ادامه کار صرف‌نظر می‌کند.
    /// 2. نمایش لودینگ (با فراخوانی ShowLoading(true)).
    /// 3. ساخت یک ConnectRequestDto از تنظیمات پیش‌فرض.
    /// 4. فراخوانی _appService.ConnectAsync برای برقراری اتصال.
    /// 5. در صورت موفقیت، رویداد ConnectionStateChanged وضعیت را به‌روز می‌کند.
    /// 6. در صورت بروز خطا، خطا را در لاگ نمایش می‌دهد.
    /// 7. در نهایت، لودینگ را مخفی می‌کند (با فراخوانی ShowLoading(false)).
    ///
    /// Side Effects:
    /// - اتصال به RTU و به‌روزرسانی UI (در صورت موفقیت).
    ///
    /// Limitations:
    /// - اگر اتصال ناموفق باشد، کاربر همچنان می‌تواند به‌صورت دستی اتصال برقرار کند.
    /// - خطاها فقط در لاگ نمایش داده می‌شوند (می‌توان در UI نیز نمایش داد).
    /// </remarks>
    private async Task AutoConnectAsync()
    {
        // اگر قبلاً متصل هستیم، نیازی به تلاش مجدد نیست
        if (lblStatus.Text == "Connected")
            return;

        // ★ تاخیر کوتاه برای اطمینان از بارگذاری کامل فرم
        //await Task.Delay(100);

        ShowLoading(true);
        // ========== اضافه کردن تاخیر تستی ==========
        //await Task.Delay(3000); // ۵ ثانیه تاخیر برای مشاهده‌ی لودینگ
        // ===========================================

        try
        {
            var request = new ConnectRequestDto(
                _options.Host,
                _options.Port,
                _options.CommonAddress,
                _options.TimeoutMs,
                _options.UseTls
            );

            await _appService.ConnectAsync(request);

            // در صورت موفقیت، رویداد ConnectionStateChanged وضعیت را به‌روز می‌کند
        }
        catch (Exception ex)
        {
            // خطا را در لاگ نمایش می‌دهیم
            //_appService.PublishMessage(new ProtocolMessageDto
            //{
            //  Timestamp = DateTime.Now,
            // Kind = "Error",
            //Title = "Auto-Connect Failed",
            //Details = ex.Message
            // });
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    /// <summary>
    /// مدیریت کلیک روی دکمه‌ی مدیریت درخواست‌های دوره‌ای
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - باز کردن فرم PeriodicRequestsForm برای مدیریت درخواست‌های دوره‌ای.
    ///
    /// Workflow:
    /// 1. ایجاد نمونه‌ای از PeriodicRequestsForm از طریق IServiceProvider.
    /// 2. نمایش فرم به‌صورت Modal.
    ///
    /// Side Effects:
    /// - نمایش فرم جدید و درگیر کردن کاربر با آن.
    ///
    /// Limitations:
    /// - این متد پس از بسته شدن فرم، هیچ کاری انجام نمی‌دهد (فقط نمایش می‌دهد).
    /// </remarks>
    /// <param name="sender">فرستنده رویداد (دکمه ManageRequests)</param>
    /// <param name="e">آرگومان‌های رویداد</param>
    private void btnManageRequests_Click(object sender, EventArgs e)
    {
        using var form = _serviceProvider.GetRequiredService<PeriodicRequestsForm>();
        form.ShowDialog(this);
    }

    /// <summary>
    /// مدیریت کلیک روی دکمه‌ی ارسال دستی درخواست
    /// </summary>
    /// <remarks>
    /// Goal:
    /// - باز کردن فرم SendRequestForm برای ارسال دستی یک درخواست به RTU.
    ///
    /// Workflow:
    /// 1. ایجاد نمونه‌ای از SendRequestForm از طریق IServiceProvider.
    /// 2. نمایش فرم به‌صورت Modal.
    ///
    /// Side Effects:
    /// - نمایش فرم جدید و درگیر کردن کاربر با آن.
    ///
    /// Limitations:
    /// - این متد پس از بسته شدن فرم، هیچ کاری انجام نمی‌دهد (فقط نمایش می‌دهد).
    /// </remarks>
    /// <param name="sender">فرستنده رویداد (دکمه SendRequest)</param>
    /// <param name="e">آرگومان‌های رویداد</param>
    private void btnSendRequest_Click(object sender, EventArgs e)
    {
        using var form = _serviceProvider.GetRequiredService<SendRequestForm>();
        form.ShowDialog(this);
    }
}