using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.DTOs;
using IEC104.Master.Infrastructure.Options;
using IEC104.Master.WinForms.Form;
using IEC104.Master.WinForms.Models;
using Microsoft.Extensions.DependencyInjection;

namespace IEC104.Master.WinForms.Form;

public partial class MainForm : System.Windows.Forms.Form
{
    private readonly IIec104MasterAppService _appService;
    private readonly Iec104Options _options;
    private readonly IServiceProvider _serviceProvider; // ← اضافه کنید
    private readonly BindingSource _asduBindingSource = new BindingSource();
    private readonly List<AsduDisplayModel> _asduDisplayList = new List<AsduDisplayModel>();

    public MainForm(IIec104MasterAppService appService, Iec104Options options, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _appService = appService;
        _options = options;
        _serviceProvider = serviceProvider;

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
        await AutoConnectAsync();
    }

    private void ConfigureDataGridView()
    {
        // اجازه دهید ستون‌ها به‌صورت خودکار از مدل ساخته شوند
        dgvAsduData.AutoGenerateColumns = true;
        dgvAsduData.DataSource = _asduBindingSource;

        // پس از اتصال، عنوان ستون‌ها را به فارسی تغییر دهید
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

    private void OnMessagePublished(ProtocolMessageDto message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => OnMessagePublished(message)));
            return;
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

            // اسکرول به پایین
            dgvAsduData.FirstDisplayedScrollingRowIndex = dgvAsduData.Rows.Count - 1;
        }

        lstLog.Items.Insert(0, $"{message.Timestamp:HH:mm:ss} [{message.Kind}] {message.Title} - {message.Details}");
    }

    private void OnConnectionStateChanged(ConnectionStateDto state)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => OnConnectionStateChanged(state)));
            return;
        }

        lblStatus.Text = state.StatusText;

        // به‌روزرسانی وضعیت دکمه‌ها بر اساس اتصال
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
        await Task.Delay(3000); // ۵ ثانیه تاخیر برای مشاهده‌ی لودینگ
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