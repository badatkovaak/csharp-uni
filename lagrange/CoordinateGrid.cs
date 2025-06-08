using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;

namespace Lagrange;

class CooridnateGrid : Canvas
{
    (Line, Line) axes;
    List<Line> x_gridlines;
    List<Line> y_gridlines;

    double y_length;
    double x_length;

    public CooridnateGrid(double x_length, double y_length)
    {
        this.x_length = x_length;
        this.y_length = y_length;

        Line x_axis = new Line();
        x_axis.Stroke = Brushes.Black;
        x_axis.StrokeThickness = 2;
        this.Children.Add(x_axis);

        Line y_axis = new Line();
        y_axis.Stroke = Brushes.Black;
        y_axis.StrokeThickness = 2;
        this.Children.Add(y_axis);

        this.axes = (x_axis, y_axis);

        this.x_gridlines = new List<Line>();
        this.y_gridlines = new List<Line>();

        int num_of_gridlines = 8;

        for (int i = 0; i < num_of_gridlines; i++)
        {
            Line x_gridline = new Line();
            x_gridline.Stroke = Brushes.Black;
            x_gridline.StrokeThickness = 1;

            this.Children.Add(x_gridline);
            x_gridlines.Add(x_gridline);
        }

        for (int i = 0; i < num_of_gridlines; i++)
        {
            Line y_gridline = new Line();
            y_gridline.Stroke = Brushes.Black;
            y_gridline.StrokeThickness = 1;

            this.Children.Add(y_gridline);
            y_gridlines.Add(y_gridline);
        }

        this.EffectiveViewportChanged += OnDimensionsChange;
    }

    public Avalonia.Point ConvertCoordinates(double x, double y)
    {
        double x_coord = ((x + x_length) / (2 * x_length)) * this.Bounds.Width;
        double y_coord = ((-y + y_length) / (2 * y_length)) * this.Bounds.Height;
        Console.WriteLine(
            $"converted to {x_coord}, {y_coord} , {this.Bounds.Height}, {this.Bounds.Width}"
        );
        return new Avalonia.Point(x_coord, y_coord);
    }

    public List<Ellipse> PlotFunctionDots(Func<double, double> f, double h)
    {
        int total_len = Convert.ToInt32(Math.Floor(2 * this.x_length / h));

        List<Ellipse> dots = new List<Ellipse>();

        for (int i = 0; i < total_len; i++)
        {
            double x = -this.x_length + h * i;
            double y = f(x);

            Avalonia.Point point = this.ConvertCoordinates(x, y);
            Console.WriteLine($"{x}, {y} -> {point}");

            Ellipse dot = new Ellipse();
            dot.Fill = Brushes.Red;
            dot.Width = 2;
            dot.Height = 2;
            SetLeft(dot, point.X - 1);
            SetTop(dot, point.Y - 1);

            this.Children.Add(dot);
            dots.Add(dot);
        }

        return dots;
    }

    public List<Line> PlotFunctionLines(Func<double, double> f, double h)
    {
        int total_len = Convert.ToInt32(Math.Floor(2 * this.x_length / h));

        List<Line> lines = new List<Line>();
        Avalonia.Point previous = this.ConvertCoordinates(-this.x_length, f(-this.x_length));
        Avalonia.Point current = new Avalonia.Point();

        for (int i = 0; i < total_len; i++)
        {
            double x = -this.x_length + h * i;
            double y = f(x);
            current = this.ConvertCoordinates(x, y);

            Line line = new Line();
            line.Stroke = Brushes.Red;
            line.StrokeThickness = 1;
            line.StartPoint = previous;
            line.EndPoint = current;

            this.Children.Add(line);
            lines.Add(line);
            previous = current;
        }

        return lines;
    }

    public void OnDimensionsChange(object? sender, EffectiveViewportChangedEventArgs e)
    {
        double height = this.Bounds.Height;
        double width = this.Bounds.Width;

        this.axes.Item1.StartPoint = new Avalonia.Point(0, height / 2);
        this.axes.Item1.EndPoint = new Avalonia.Point(width, height / 2);

        this.axes.Item2.StartPoint = new Avalonia.Point(width / 2, 0);
        this.axes.Item2.EndPoint = new Avalonia.Point(width / 2, height);

        double step_x = width / (this.x_gridlines.Count + 2);
        double step_y = height / (this.y_gridlines.Count + 2);

        Console.WriteLine($"Bounds - {this.Bounds.Width} {this.Bounds.Height}");
        Console.WriteLine($"{step_x} {step_y}");
        Console.WriteLine($"x_axis - ({this.axes.Item1.StartPoint}), ({this.axes.Item1.EndPoint})");
        Console.WriteLine($"y_axis - ({this.axes.Item2.StartPoint}), ({this.axes.Item2.EndPoint})");

        int i = 1;

        foreach (Line gridline in this.x_gridlines)
        {
            if (i == this.x_gridlines.Count / 2 + 1)
                i++;

            gridline.StartPoint = new Avalonia.Point(0, step_y * i);
            gridline.EndPoint = new Avalonia.Point(width, step_y * i);
            // Console.WriteLine($"{i}th coords are ({gridline.StartPoint}), ({gridline.EndPoint})");

            i++;
        }

        i = 1;

        foreach (Line gridline in this.y_gridlines)
        {
            if (i == this.y_gridlines.Count / 2 + 1)
                i++;

            gridline.StartPoint = new Avalonia.Point(step_x * i, 0);
            gridline.EndPoint = new Avalonia.Point(step_x * i, height);
            // Console.WriteLine($"{i}th coords are ({gridline.StartPoint}), ({gridline.EndPoint})");

            i++;
        }

        // Avalonia.Point coords = this.ConvertCoordinates(-1, 2);
        // Ellipse p1 = new Ellipse();
        // p1.Fill = Brushes.Aqua;
        // p1.Height = 10;
        // p1.Width = 10;
        // SetLeft(p1, coords.X - 5);
        // SetTop(p1, coords.Y - 5);
        // this.Children.Add(p1);
    }
}
