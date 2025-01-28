using System;
using System.Windows.Forms;

namespace StrideGame.WinForms;

public partial class UserControlWithButton : UserControl
{
    StrideControl _strideControl = new StrideControl();

    public UserControlWithButton()
    {
        InitializeComponent();
        Dock = DockStyle.Fill;

        _strideControl.Dock = DockStyle.Fill;
        Controls.Add(_strideControl);
    }

    private void Button_Click(object sender, EventArgs e)
    {
        _strideControl.Game.Paused = !_strideControl.Game.Paused;
    }
}
