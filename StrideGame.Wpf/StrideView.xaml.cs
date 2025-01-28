using Stride.CommunityToolkit.Engine;
using Stride.Core.Diagnostics;
using Stride.Core.Presentation.Controls;
using Stride.Core.Presentation.Interop;
using Stride.Engine;
using Stride.Games;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace StrideGame.Wpf;

/// <summary>
/// Interaction logic for StrideView.xaml
/// Class draft from https://gist.github.com/EricEzaM/5797be1f4b28f15e9be53287a02d3d67
/// </summary>
public partial class StrideView : UserControl
{
    /// <summary>
    /// A specialization of <see cref="GameForm"/> that is able to forward keyboard and mousewheel events to an associated <see cref="GameEngineHost"/>.
    /// </summary>
    [DesignerCategory("")]
    internal class EmbeddedGameForm : GameForm
    {
        public EmbeddedGameForm()
        {
            enableFullscreenToggle = false;
        }

        /// <summary>
        /// Gets or sets the <see cref="GameEngineHost"/> associated to this form.
        /// </summary>
        public GameEngineHost? Host { get; set; }

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
                    case NativeHelper.WM_RBUTTONDBLCLK:
                    case NativeHelper.WM_RBUTTONDOWN:
                    case NativeHelper.WM_RBUTTONUP:
                    case NativeHelper.WM_LBUTTONDBLCLK:
                    case NativeHelper.WM_LBUTTONDOWN:
                    case NativeHelper.WM_LBUTTONUP:
                    case NativeHelper.WM_MBUTTONDBLCLK:
                    case NativeHelper.WM_MBUTTONDOWN:
                    case NativeHelper.WM_MBUTTONUP:
                    case NativeHelper.WM_MOUSEMOVE:
                    case NativeHelper.WM_CONTEXTMENU:
                        Host.ForwardMessage(m.HWnd, m.Msg, m.WParam, m.LParam);
                        break;
                }
            }
            base.WndProc(ref m);
        }
    }

    private Thread? _strideThread;
    private readonly TaskCompletionSource<bool> _strideStartedTaskSource = new();
    private EmbeddedGameForm? _form;
    private nint _winformHandle;

    public string? GameClass { get; set; }

    public StrideView()
    {
        InitializeComponent();

        Loaded += (sender, args) =>
        {
            if (!DesignerProperties.GetIsInDesignMode(this) && GameClass is not null)
            {
                _strideThread = new Thread(SafeAction.Wrap(StrideThreadRun))
                {
                    IsBackground = true,
                    Name = "Stride-Thread"
                };
                _strideThread.SetApartmentState(ApartmentState.STA);
                _strideThread.Start();

                _strideStartedTaskSource.Task.Wait();

                _form!.Host = new GameEngineHost(_winformHandle);
                SceneView.Content = _form.Host;
            }
        };
    }

    private void StrideThreadRun()
    {
        // This runs on Stride thread
        _form = new EmbeddedGameForm()
        {
            TopLevel = false,
            Visible = false
        };
        _winformHandle = _form.Handle;

        _strideStartedTaskSource.SetResult(true);

        var context = new GameContextWinforms(_form);
        var game = (Game)Activator.CreateInstance(Type.GetType(GameClass!)!)!;
        game.Run(context
            , start: scene =>
            {
                game.Window.IsBorderLess = true;

                // Get WPF parent root window, then force a resize +10, then -10 to fix the window size
                Dispatcher.Invoke(() =>
                {
                    var parentWindow = Window.GetWindow(this);
                    if (parentWindow != null)
                    {
                        parentWindow.Width += 10;
                        parentWindow.Width -= 10;
                    }
                });
            }
            , update: (scene, gameTime) =>
            {
            }
        );
    }
}
