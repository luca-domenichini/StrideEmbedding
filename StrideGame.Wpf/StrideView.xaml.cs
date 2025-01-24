using Stride.Core.Diagnostics;
using Stride.Core.Presentation.Controls;
using Stride.Core.Presentation.Interop;
using Stride.Engine;
using Stride.Games;
using System.Windows.Controls;

namespace StrideGame.Wpf;

/// <summary>
/// Interaction logic for StrideView.xaml
/// </summary>
public partial class StrideView : UserControl
{
    private Thread gameThread;

    private readonly TaskCompletionSource<bool> gameStartedTaskSource = new TaskCompletionSource<bool>();

    private nint windowHandle;

    public StrideView()
    {
        InitializeComponent();

        gameThread = new Thread(SafeAction.Wrap(GameRunThread))
        {
            IsBackground = true,
            Name = "Game Thread"
        };
        gameThread.SetApartmentState(ApartmentState.STA);

        Loaded += (sender, args) =>
        {
            StartGame();
        };
    }

    private async Task StartGame()
    {
        gameThread.Start();

        await gameStartedTaskSource.Task;

        SceneView.Content = new GameEngineHost(windowHandle);
    }

    private void GameRunThread()
    {
        // Create the form from this thread
        // EmbeddedGameForm is in Stride.Editor. You may need to copy this class to your own project.
        var form = new EmbeddedGameForm()
        {
            TopLevel = false,
            Visible = false
        };
        windowHandle = form.Handle;

        var context = new GameContextWinforms(form);

        gameStartedTaskSource.SetResult(true);
        var game = new Game();
        //game.Window.IsBorderLess = true;
        game.Activated += (sender, args) => game.Window.IsBorderLess = true;
        game.WindowCreated += (sender, args) => game.Window.IsBorderLess = true;
        game.Run(context);
        //game.SetupBase3DScene();
    }
}

// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
/// <summary>
/// A specialization of <see cref="GameForm"/> that is able to forward keyboard and mousewheel events to an associated <see cref="GameEngineHost"/>.
/// </summary>
[System.ComponentModel.DesignerCategory("")]
public class EmbeddedGameForm : GameForm
{
    public EmbeddedGameForm()
    {
        enableFullscreenToggle = false;
    }

    /// <summary>
    /// Gets or sets the <see cref="GameEngineHost"/> associated to this form.
    /// </summary>
    public GameEngineHost Host { get; set; }

    /// <inheritdoc/>
    protected override void WndProc(ref System.Windows.Forms.Message m)
    {
        if (Host != null)
        {
            switch (m.Msg)
            {
                case NativeHelper.WM_KEYDOWN:
                case NativeHelper.WM_KEYUP:
                case NativeHelper.WM_MOUSEWHEEL:
                case NativeHelper.WM_RBUTTONDOWN:
                case NativeHelper.WM_RBUTTONUP:
                case NativeHelper.WM_LBUTTONDOWN:
                case NativeHelper.WM_LBUTTONUP:
                case NativeHelper.WM_MOUSEMOVE:
                case NativeHelper.WM_CONTEXTMENU:
                    Host.ForwardMessage(m.HWnd, m.Msg, m.WParam, m.LParam);
                    break;
            }
        }
        base.WndProc(ref m);
    }
}
