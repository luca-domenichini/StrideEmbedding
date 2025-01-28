using Stride.Engine;
using Stride.Games;
using System;
using System.Windows.Forms;

namespace StrideGame.WinForms;

public partial class StrideControl : UserControl
{
    public TeapotDemo Game { get; private set; }
    private GameContext _gameContext;

    public StrideControl()
    {
        InitializeComponent();

        _gameContext = new GameContextWinforms(this);
        Game = new TeapotDemo();
        Game.WindowCreated += (sender, args) =>
        {
            if (sender is Game game)
            {
                game.Window.AllowUserResizing = true;
            }
        };
    }

    private void StrideControl_Load(object sender, EventArgs e)
    {
        BeginInvoke(() => Game.Run(_gameContext));
    }
}
