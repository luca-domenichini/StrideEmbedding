using StrideGame.WinForms;
using System.Windows;
using System.Windows.Forms.Integration;

namespace StrideGame.WpfAndWinForms;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private WindowsFormsHost _windowsFormsHost = new WindowsFormsHost();

    public MainWindow()
    {
        InitializeComponent();

        _windowsFormsHost.Child = new UserControlWithButton();
        WinFormHost.Children.Add(_windowsFormsHost);
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
        _windowsFormsHost.Dispose();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Width += 10;
        Width -= 10;
    }
}