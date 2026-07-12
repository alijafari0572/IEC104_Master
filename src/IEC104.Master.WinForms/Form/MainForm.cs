using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.Abstractions.Repositories;
using IEC104.Master.Application.DTOs;
using IEC104.Master.Domain.Entities;
using IEC104.Master.Infrastructure.Options;
using IEC104.Master.WinForms.Form;
using IEC104.Master.WinForms.Models;
using Microsoft.Extensions.DependencyInjection;
using Point = IEC104.Master.Domain.Entities.Point;

namespace IEC104.Master.WinForms.Form;

public partial class MainForm : System.Windows.Forms.Form
{
    private readonly IIec104MasterAppService _appService;
    private readonly Iec104Options _options;
    private readonly IServiceProvider _serviceProvider; // ← اضافه کنید
    private readonly BindingSource _asduBindingSource = new BindingSource();
    private readonly List<AsduDisplayModel> _asduDisplayList = new List<AsduDisplayModel>();
    private readonly IUnitOfWork _unitOfWork; // ← اضافه کنید

    public MainForm(IIec104MasterAppService appService, Iec104Options options, IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
    {
        InitializeComponent();
        _appService = appService;
        _options = options;
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;

        _appService.MessagePublished += OnMessagePublished;
        _appService.ConnectionStateChanged += OnConnectionStateChanged;
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        btnDisconnect.Enabled = false;
        btnGI.Enabled = false;
        lblStatus.Text = "Disconnected";

        dgvAsduData.DataSource = _asduBindingSource;
        _asduBindingSource.DataSource = _asduDisplayList;

        // تنظیم ستون‌ها
        ConfigureDataGridView();

        // شروع اتصال خودکار
        //await AutoConnectAsync();
    }

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
    }

    private async void btnDisconnect_Click_1(object sender, EventArgs e)
    {
        await _appService.DisconnectAsync();
        btnConnect.Enabled = true;
        btnDisconnect.Enabled = false;
        btnGI.Enabled = false;
    }

    private async void btnGI_Click_1(object sender, EventArgs e)
    {
        await _appService.SendGeneralInterrogationAsync(new GeneralInterrogationRequestDto(20));
    }

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
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// نمایش یا مخفی‌سازی پنل لودینگ
    /// </summary>
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
    private async Task AutoConnectAsync()
    {
        // اگر قبلاً متصل هستیم، نیازی به تلاش مجدد نیست
        if (lblStatus.Text == "Connected")
            return;

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
}