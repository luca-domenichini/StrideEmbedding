using Stride.Engine;
using Stride.Games;
using System;
using System.Windows.Forms;

namespace StrideGame.WinForms
{
    public partial class Form1 : Form
    {
        private TeapotDemo _game;
        private GameContext _gameContext;

        public Form1()
        {
            InitializeComponent();
            _gameContext = new GameContextWinforms(this);
            _game = new TeapotDemo();
            _game.WindowCreated += (sender, args) =>
            {
                if (sender is Game game)
                {
                    game.Window.AllowUserResizing = true;
                }
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _game.Run(_gameContext);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _game.Paused = !_game.Paused;
        }
    }
}
