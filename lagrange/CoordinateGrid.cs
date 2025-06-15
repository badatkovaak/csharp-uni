using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Lagrange;

class CooridnateGrid : Canvas
{
    (Line, Line) axes;
    List<Line> x_gridlines;
    List<Line> y_gridlines;

    private int plot_id = 0;
    List<Plot> plots;

    private int point_id = 0;
    List<SelectedPoint> selectedPoints;

    double y_length;
    double x_length;

    public CooridnateGrid(double x_length, double y_length)
    {
        this.x_length = Math.Floor(x_length);
        this.y_length = Math.Floor(y_length);

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

        int num_of_gridlines_x = 2 * (Convert.ToInt32(x_length) - 1);
        int num_of_gridlines_y = 2 * (Convert.ToInt32(y_length) - 1);

        for (int i = 0; i < num_of_gridlines_x; i++)
        {
            Line x_gridline = new Line();
            x_gridline.Stroke = Brushes.Black;
            x_gridline.StrokeThickness = 1;

            this.Children.Add(x_gridline);
            x_gridlines.Add(x_gridline);
        }

        for (int i = 0; i < num_of_gridlines_y; i++)
        {
            Line y_gridline = new Line();
            y_gridline.Stroke = Brushes.Black;
            y_gridline.StrokeThickness = 1;

            this.Children.Add(y_gridline);
            y_gridlines.Add(y_gridline);
        }

        this.plots = new List<Plot>();
        this.selectedPoints = new List<SelectedPoint>();

        this.SizeChanged += OnSizeChanged;
    }

    public Point ConvertCoordinates(double x, double y)
    {
        double x_coord = ((x + x_length) / (2 * x_length)) * this.Bounds.Width;
        double y_coord = ((-y + y_length) / (2 * y_length)) * this.Bounds.Height;
        return new Point(x_coord, y_coord);
    }

    // public List<Ellipse> PlotFunctionDots(Func<double, double> f, double h)
    // {
    //     int total_len = Convert.ToInt32(Math.Floor(2 * this.x_length / h));
    //
    //     List<Ellipse> dots = new List<Ellipse>();
    //
    //     for (int i = 0; i < total_len; i++)
    //     {
    //         double x = -this.x_length + h * i;
    //         double y = f(x);
    //
    //         Point point = this.ConvertCoordinates(x, y);
    //
    //         Ellipse dot = new Ellipse();
    //         dot.Fill = Brushes.Red;
    //         dot.Width = 2;
    //         dot.Height = 2;
    //         SetLeft(dot, point.X - 1);
    //         SetTop(dot, point.Y - 1);
    //
    //         this.Children.Add(dot);
    //         dots.Add(dot);
    //     }
    //
    //     return dots;
    // }

    private bool isInsideBounds(Point p)
    {
        if (p.X < 0 || p.X > this.Bounds.Width)
            return false;

        if (p.Y < 0 || p.Y > this.Bounds.Height)
            return false;

        return true;
    }

    private List<Line> PlotFunctionInner(Func<double, double> f, double h)
    {
        int total_len = Convert.ToInt32(Math.Floor(2 * this.x_length / h));

        List<Line> lines = new List<Line>();
        Avalonia.Point previous = this.ConvertCoordinates(-this.x_length, f(-this.x_length));
        Avalonia.Point current = new Point();

        for (int i = 0; i < total_len; i++)
        {
            double x = -this.x_length + h * i;
            double y = f(x);
            current = this.ConvertCoordinates(x, y);

            if (!this.isInsideBounds(current) || !this.isInsideBounds(previous))
            {
                previous = current;
                continue;
            }

            Line line = new Line();
            line.Stroke = Brushes.Red;
            line.StrokeThickness = 2;
            line.StartPoint = previous;
            line.EndPoint = current;

            this.Children.Add(line);
            lines.Add(line);
            previous = current;
        }
        return lines;
    }

    public int PlotFunction(Func<double, double> f, double h)
    {
        List<Line> lines = this.PlotFunctionInner(f, h);

        this.plots.Add(new Plot(this.plot_id, f, lines, h));
        this.plot_id++;

        return this.plot_id - 1;
    }

    public void RemovePlot(int id)
    {
        foreach (Plot plot in this.plots)
        {
            if (plot.id != id)
                continue;

            foreach (Line line in plot.plotLines)
            {
                this.Children.Remove(line);
            }
        }

        this.plots.RemoveAll((x) => x.id == id);
    }

    public void RedrawPlot(Plot plot)
    {
        foreach (Line line in plot.plotLines)
        {
            this.Children.Remove(line);
        }

        List<Line> lines = this.PlotFunctionInner(plot.function, plot.step);

        foreach (Line line in lines)
        {
            plot.plotLines.Add(line);
        }
    }

    public Ellipse DrawSelectedPoint(Point point)
    {
        double size = 4;
        Ellipse e = new Ellipse();
        e.Height = size;
        e.Width = size;
        e.Fill = Brushes.Blue;

        Canvas.SetLeft(e, point.X - size / 2);
        Canvas.SetTop(e, point.Y - size / 2);

        this.Children.Add(e);

        return e;
    }

    public void AddSelectedPoint(Point point) {
        double x = (point.X/this.Bounds.Width - 0.5) *2* x_length;
        double y = (-point.Y/this.Bounds.Height + 0.5) * 2*y_length;
        Vector2 v = new Vector2(x, y);
    }

    public void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        double height = e.NewSize.Height;
        double width = e.NewSize.Width;

        this.axes.Item1.StartPoint = new Point(0, height / 2);
        this.axes.Item1.EndPoint = new Point(width, height / 2);

        this.axes.Item2.StartPoint = new Point(width / 2, 0);
        this.axes.Item2.EndPoint = new Point(width / 2, height);

        double step_x = width / (this.x_gridlines.Count + 2);
        double step_y = height / (this.y_gridlines.Count + 2);

        Console.WriteLine($"Bounds - {e.NewSize.Width} {e.NewSize.Height}");
        Console.WriteLine($"{step_x} {step_y}");
        Console.WriteLine($"x_axis - ({this.axes.Item1.StartPoint}), ({this.axes.Item1.EndPoint})");
        Console.WriteLine($"y_axis - ({this.axes.Item2.StartPoint}), ({this.axes.Item2.EndPoint})");

        int i = 1;

        foreach (Line gridline in this.x_gridlines)
        {
            if (i == this.x_gridlines.Count / 2 + 1)
                i++;

            gridline.StartPoint = new Point(0, step_y * i);
            gridline.EndPoint = new Point(width, step_y * i);

            i++;
        }

        i = 1;

        foreach (Line gridline in this.y_gridlines)
        {
            if (i == this.y_gridlines.Count / 2 + 1)
                i++;

            gridline.StartPoint = new Point(step_x * i, 0);
            gridline.EndPoint = new Point(step_x * i, height);

            i++;
        }

        foreach (Plot plot in this.plots)
        {
            this.RedrawPlot(plot);
        }
    }
}
