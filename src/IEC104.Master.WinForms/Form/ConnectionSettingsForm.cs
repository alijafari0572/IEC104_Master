using IEC104.Master.Application.DTOs;

using IEC104.Master.Application.DTOs;

using IEC104.Master.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IEC104.Master.WinForms.Form;

public partial class ConnectionSettingsForm : System.Windows.Forms.Form
{
    private readonly Iec104Options _options; // ← فیلد جدید

    public ConnectionSettingsForm(Iec104Options options)
    {
        InitializeComponent();
        _options = options;
    }

    public ConnectRequestDto BuildRequest()
    {
        return new ConnectRequestDto(
            txtHost.Text.Trim(),
            int.Parse(txtPort.Text),
            int.Parse(txtCommonAddress.Text),
            int.Parse(txtTimeout.Text),
            chkTls.Checked);
    }

    // رویداد Load فرم (برای مقداردهی اولیه)
    private void ConnectionSettingsForm_Load(object sender, EventArgs e)
    {
        // پر کردن فیلدها با مقادیر پیش‌فرض از _options
        txtHost.Text = _options.Host;
        txtPort.Text = _options.Port.ToString();
        txtCommonAddress.Text = _options.CommonAddress.ToString();
        txtTimeout.Text = _options.TimeoutMs.ToString();
        chkTls.Checked = _options.UseTls;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
    }
}