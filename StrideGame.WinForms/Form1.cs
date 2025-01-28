using System.Windows.Forms;

namespace StrideGame.WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Controls.Add(new UserControlWithButton());
        }
    }
}
