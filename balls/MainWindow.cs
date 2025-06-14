using Avalonia.Controls;

namespace Balls;

public class MainWindow : Window
{
    public MainWindow()
    {
        this.Title = "Balls";
        BallCanvas b = new BallCanvas();

        this.Content = b;
    }
}
