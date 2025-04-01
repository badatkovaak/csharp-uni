using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace Lagrange;

class MainWindow : Window
{
    TextBlock display;

    public MainWindow()
    {
        this.Title = "Lagrange";

        Grid main_grid = new Grid();
        main_grid.HorizontalAlignment = HorizontalAlignment.Stretch;
        main_grid.VerticalAlignment = VerticalAlignment.Stretch;

        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
        /*main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));*/
        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));

        TextBox input = new TextBox();
        input.HorizontalAlignment = HorizontalAlignment.Stretch;
        input.VerticalAlignment = VerticalAlignment.Stretch;
        input.Watermark =
            "Write your points below as comma-separated list of comma-separated pairs (something like (1, 0), (2,1), ...)";
        input.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
        input.AcceptsReturn = true;
        input.TextChanged += InputHandler;
        Grid.SetRow(input, 0);
        main_grid.Children.Add(input);

        /*Button enter = new Button();*/
        /*enter.HorizontalAlignment = HorizontalAlignment.Right;*/
        /*enter.VerticalAlignment = VerticalAlignment.Top;*/
        /*enter.Content = "Enter";*/
        /*Grid.SetRow(enter, 1);*/
        /*main_grid.Children.Add(enter);*/

        TextBlock result = new TextBlock();
        result.HorizontalAlignment = HorizontalAlignment.Stretch;
        result.VerticalAlignment = VerticalAlignment.Stretch;
        result.Text = "Lagrange Polynomial of Points - ";
        Grid.SetRow(result, 1);
        main_grid.Children.Add(result);

        this.Content = main_grid;
        this.display = result;
    }

    public void InputHandler(object? sender, RoutedEventArgs e)
    {
        if (e.Source is null)
            return;

        TextBox t = (TextBox)e.Source;

        if (t.Text is null)
            return;

        Console.WriteLine($"text changed - {t.Text}");
        Parser p = new Parser(t.Text);
        List<(double, double)>? points = p.ParsePoints();

        if (points is null)
            Console.WriteLine("result is null");

        if (points is null)
            return;

        Polynomial result = Polynomial.constructLagrangePolynomial(points);
        Console.WriteLine(result);
        this.display.Text = result.ToString();
    }
}
