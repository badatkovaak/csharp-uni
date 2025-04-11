using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;

namespace Lagrange;

class CooridnateGrid : Canvas
{
    Line x;
    Line y;

    double max_x;
    double min_x;

    double max_y;
    double min_y;

    public CooridnateGrid()
    {
        this.max_x = 10;
        this.min_x = -10;
        this.max_y = 10;
        this.min_y = -10;

        Line x = new Line();
        x.Stroke = Brushes.Black;
        x.StrokeThickness = 2;
        this.x = x;
        this.Children.Add(x);

        Line y = new Line();
        y.Stroke = Brushes.Black;
        y.StrokeThickness = 2;
        this.y = y;
        this.Children.Add(y);

        this.EffectiveViewportChanged += OnDimensionsChange;
    }

    public void OnDimensionsChange(object? sender, EffectiveViewportChangedEventArgs e)
    {
        double height = e.EffectiveViewport.Height;
        double width = e.EffectiveViewport.Width;

        this.x.StartPoint = new Avalonia.Point(width / 2, 0);
        this.x.EndPoint = new Avalonia.Point(width / 2, height);

        this.y.StartPoint = new Avalonia.Point(0, height / 2);
        this.y.EndPoint = new Avalonia.Point(width, height / 2);
    }
}
