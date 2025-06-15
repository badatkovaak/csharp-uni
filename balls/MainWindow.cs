using Avalonia.Controls;
using Avalonia.Layout;

namespace Balls;

public class MainWindow : Window
{
    public MainWindow()
    {
        this.Title = "Balls";

        BallCanvas b = new BallCanvas();
        b.HorizontalAlignment = HorizontalAlignment.Stretch;
        b.VerticalAlignment = VerticalAlignment.Stretch;

        this.Content = b;
    }
}
