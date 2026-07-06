using IEC104.Master.Application.Abstractions;
using IEC104.Master.Application.DTOs;
using IEC104.Master.WinForms.Form;

namespace IEC104.Master.WinForms.Form;

public partial class MainForm : System.Windows.Forms.Form
{
    private readonly IIec104MasterAppService _appService;

    public MainForm(IIec104MasterAppService appService)
    {
        InitializeComponent();
        _appService = appService;

        _appService.MessagePublished += OnMessagePublished;
        _appService.ConnectionStateChanged += OnConnectionStateChanged;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        btnDisconnect.Enabled = false;
        btnGI.Enabled = false;
        lblStatus.Text = "Disconnected";
    }

    private async void btnConnect_Click_1(object sender, EventArgs e)
    {
        using var form = new ConnectionSettingsForm();
        if (form.ShowDialog(this) != DialogResult.OK)
            return;

        var request = form.BuildRequest();
        await _appService.ConnectAsync(request);

        btnConnect.Enabled = false;
        btnDisconnect.Enabled = true;
        btnGI.Enabled = true;
    }

    private async void btnDisconnect_Click(object sender, EventArgs e)
    {
        await _appService.DisconnectAsync();
        btnConnect.Enabled = true;
        btnDisconnect.Enabled = false;
        btnGI.Enabled = false;
    }

    private async void btnGI_Click(object sender, EventArgs e)
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
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}