using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace Lagrange;

class MainWindow : Window
{
    private CooridnateGrid canvas;
    private TextBlock result;
    private TextBox points_input;
    private List<(double, double)> points;
    private int? plot_id;

    public MainWindow()
    {
        this.Title = "Lagrange";

        Grid main_grid = new Grid();
        main_grid.HorizontalAlignment = HorizontalAlignment.Stretch;
        main_grid.VerticalAlignment = VerticalAlignment.Stretch;
        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));

        for (int i = 0; i < 10; i++)
            main_grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));

        CooridnateGrid canvas = new CooridnateGrid(10, 10);
        canvas.VerticalAlignment = VerticalAlignment.Stretch;
        canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
        canvas.Background = Brushes.White;
        canvas.PointerPressed += PointerPressedHandler;
        Grid.SetRow(canvas, 0);
        Grid.SetColumn(canvas, 0);
        Grid.SetColumnSpan(canvas, 10);
        main_grid.Children.Add(canvas);

        TextBox points_input = new TextBox();
        points_input.Text = "(-2, -2) (-1, 0) (0, -1) (1, 2)";
        Grid.SetRow(points_input, 1);
        Grid.SetColumn(points_input, 0);
        Grid.SetColumnSpan(points_input, 3);
        main_grid.Children.Add(points_input);
        this.points_input = points_input;

        TextBlock tb = new TextBlock();
        tb.HorizontalAlignment = HorizontalAlignment.Center;
        tb.VerticalAlignment = VerticalAlignment.Center;
        tb.Text = "computed polynomial:";
        tb.FontSize = 16;
        Grid.SetRow(tb, 1);
        Grid.SetColumn(tb, 5);
        main_grid.Children.Add(tb);

        TextBlock display = new TextBlock();
        display.HorizontalAlignment = HorizontalAlignment.Center;
        display.VerticalAlignment = VerticalAlignment.Center;
        display.Text = "";
        display.FontSize = 16;
        Grid.SetRow(display, 1);
        Grid.SetColumn(display, 6);
        Grid.SetColumnSpan(display, 4);
        main_grid.Children.Add(display);

        Button b = new Button();
        b.Content = "Plot";
        b.Click += ButtonOnClick;
        Grid.SetRow(b, 1);
        Grid.SetColumn(b, 3);
        main_grid.Children.Add(b);

        this.Content = main_grid;
        this.result = display;
        this.canvas = canvas;
        this.points = new List<(double, double)>();
    }

    public void ButtonOnClick(object? sender, RoutedEventArgs e)
    {
        string? s = this.points_input.Text;

        if (s is null)
            return;

        List<(double, double)>? points = new Parser(s).ParsePoints();

        if (points is null)
            return;

        foreach ((double, double) point in points)
            Console.WriteLine($"({point.Item1}, {point.Item2})");

        Polynomial p = Polynomial.constructLagrangePolynomial(points);

        if (this.plot_id is not null)
        {
            this.canvas.RemovePlot((int)plot_id);
        }

        // <<<<<<< HEAD
        this.plot_id = this.canvas.PlotFunctionLines((x) => p.Evaluate(x), 0.1);
        // =======
        // this.result.Text = p.ToString();

        // this.graph = this.canvas.PlotFunctionLines((x) => p.Evaluate(x), 0.1);
        // >>>>>>> 4fd2def (1)
    }

    public void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        Console.WriteLine(e.GetPosition(this.canvas));
    }
}
