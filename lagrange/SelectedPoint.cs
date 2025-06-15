using Avalonia.Controls.Shapes;

namespace Lagrange;

public class SelectedPoint
{
    public readonly int id;
    public Vector2 point;
    public Ellipse ellipse;

    public SelectedPoint(int id, Vector2 v, Ellipse ellipse)
    {
        this.id = id;
        this.point = v;
        this.ellipse = ellipse;
    }
}
