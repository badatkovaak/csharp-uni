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

    // HashSet<SelectedPoint> selectedPoints;
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
        // this.selectedPoints = new HashSet<SelectedPoint>();
        this.selectedPoints = new List<SelectedPoint>();

        this.SizeChanged += OnSizeChanged;
    }

    public Point ConvertCoordsMathToScreen(Vector2 vec)
    {
        double x_coord = ((vec.x + x_length) / (2 * x_length)) * this.Bounds.Width;
        double y_coord = ((-vec.y + y_length) / (2 * y_length)) * this.Bounds.Height;
        return new Point(x_coord, y_coord);
    }

    public Vector2 ConvertCoordsScreenToMath(Point point)
    {
        double x = (point.X / this.Bounds.Width - 0.5) * 2 * x_length;
        double y = (-point.Y / this.Bounds.Height + 0.5) * 2 * y_length;
        return new Vector2(x, y);
    }

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
        Vector2 vec = new Vector2(-this.x_length, f(-this.x_length));
        Avalonia.Point previous = this.ConvertCoordsMathToScreen(vec);
        Avalonia.Point current = new Point();

        for (int i = 0; i < total_len; i++)
        {
            double x = -this.x_length + h * i;
            double y = f(x);
            current = this.ConvertCoordsMathToScreen(new Vector2(x, y));

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

    public void DrawSelectedPoint(Point point, Ellipse e)
    {
        double size = 10;
        e.Height = size;
        e.Width = size;
        e.Fill = Brushes.Blue;

        Canvas.SetLeft(e, point.X - size / 2);
        Canvas.SetTop(e, point.Y - size / 2);

        this.Children.Add(e);
    }

    public int AddSelectedPoint(Vector2 v)
    {
        Point p = ConvertCoordsMathToScreen(v);
        Ellipse ellipse = new Ellipse();
        this.DrawSelectedPoint(p, ellipse);

        this.selectedPoints.Add(new SelectedPoint(this.point_id, v, ellipse));
        this.point_id++;

        return point_id;
    }

    public int AddSelectedPoint(Point point)
    {
        Vector2 v = ConvertCoordsScreenToMath(point);
        return AddSelectedPoint(v);
    }

    public void RemoveSelectedPoint(int id)
    {
        SelectedPoint? p = null;

        foreach (SelectedPoint point in this.selectedPoints)
        {
            if (point.id != id)
                continue;

            p = point;
        }

        if (p is null)
            return;

        this.Children.Remove(p.ellipse);
        // this.selectedPoints.RemoveWhere((point) => point.id == id);
        this.selectedPoints.RemoveAll((point) => point.id == id);
    }

    public void RedrawSelectedPoint(SelectedPoint point)
    {
        Point p = ConvertCoordsMathToScreen(point.point);
        Canvas.SetLeft(point.ellipse, p.X - point.ellipse.Width / 2);
        Canvas.SetTop(point.ellipse, p.Y - point.ellipse.Height / 2);
    }

    public void AdjustPoints(List<Vector2> points)
    {
        Console.WriteLine("adjust points is called");
        List<SelectedPoint> toDelete = new List<SelectedPoint>();

        foreach (SelectedPoint selPoint in this.selectedPoints)
            if (!points.Contains(selPoint.point))
                toDelete.Add(selPoint);

        foreach (SelectedPoint p in toDelete)
            this.RemoveSelectedPoint(p.id);

        foreach (Vector2 vec in points)
            if (!this.selectedPoints.Exists((p) => p.point == vec))
                this.AddSelectedPoint(vec);
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
            this.RedrawPlot(plot);

        foreach (SelectedPoint point in this.selectedPoints)
            this.RedrawSelectedPoint(point);
    }
}
