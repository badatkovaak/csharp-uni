using Avalonia.Controls.Shapes;

namespace Lagrange;

public class SelectedPoint
{
    public Vector2 point;
    public Ellipse ellipse;

    public SelectedPoint(Vector2 v, Ellipse ellipse)
    {
        this.point = v;
        this.ellipse = ellipse;
    }
}
