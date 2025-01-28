using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace StrideGame.WinForms;

public partial class UserControlWithButton : UserControl
{
    private StrideControl? _strideControl;

    public UserControlWithButton()
    {
        InitializeComponent();
        Dock = DockStyle.Fill;

        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        _strideControl = new StrideControl();
        _strideControl.Dock = DockStyle.Fill;
        Controls.Add(_strideControl);
    }

    private void Button_Click(object sender, EventArgs e)
    {
        //_strideControl.Game.Paused = !_strideControl.Game.Paused;
    }
}
