using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

namespace Paint;

class MainWindow : Window
{
    private Canvas canvas;
    private TextBlock result;
    private List<(double, double)> points;

    public MainWindow()
    {
        this.Title = "Paint";

        Grid main_grid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));

        for (int i = 0; i < 10; i++)
            main_grid.ColumnDefinitions.Add(new ColumnDefinition(0.1, GridUnitType.Star));

        Canvas canvas = new Canvas
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = Brushes.White,
        };
        canvas.PointerPressed += PointerPressedHandler;

        Line l1 = new Line();

        Grid.SetRow(canvas, 0);
        Grid.SetColumn(canvas, 0);
        Grid.SetColumnSpan(canvas, 10);
        main_grid.Children.Add(canvas);

        TextBox x_lower_bound = new TextBox();
        Grid.SetRow(x_lower_bound, 1);
        Grid.SetColumn(x_lower_bound, 1);
        main_grid.Children.Add(x_lower_bound);
        // TextBox x_upper_bound = new TextBox();
        // Grid.SetRow(x_upper_bound, 1);
        // TextBox y_lower_bound = new TextBox();
        // Grid.SetRow(y_lower_bound, 1);
        // TextBox y_upper_bound = new TextBox();
        // Grid.SetRow(y_upper_bound, 1);

        TextBlock display = new TextBlock
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Text = "Hello There",
        };
        Grid.SetRow(display, 2);
        Grid.SetColumn(display, 0);
        Grid.SetColumnSpan(display, 10);
        main_grid.Children.Add(display);

        this.Content = main_grid;
        this.result = display;
        this.canvas = canvas;
        this.points = new List<(double, double)>();
    }

    public void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        Console.WriteLine(e.GetPosition(this.canvas));
    }
}
