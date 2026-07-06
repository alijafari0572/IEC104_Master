using IEC104.Master.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IEC104.Master.Application.DTOs;

namespace IEC104.Master.WinForms.Form;

public partial class ConnectionSettingsForm : System.Windows.Forms.Form
{
    public ConnectionSettingsForm()
    {
        InitializeComponent();
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

    private void ConnectionSettingsForm_Load(object sender, EventArgs e)
    {

    }
}