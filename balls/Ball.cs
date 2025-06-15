using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Balls;

public class Ball : Ellipse
{
    Point position;
    Direction direction;
    public const double Radius = 10;

    public Point Position
    {
        get => this.position;
        set
        {
            this.position = value;
            Canvas.SetLeft(this, value.X - Radius);
            Canvas.SetTop(this, value.Y - Radius);
        }
    }

    public Direction Direction
    {
        get => this.direction;
        set => this.direction = value;
    }

    public Ball(Point pos, Direction dir)
        : base()
    {
        this.position = pos;
        this.direction = dir;

        this.Height = 2 * Radius;
        this.Width = 2 * Radius;
        this.Fill = Brushes.Red;

        Canvas.SetLeft(this, pos.X - Radius);
        Canvas.SetTop(this, pos.Y - Radius);
    }

    public static double Distance(Ball b1, Ball b2)
    {
        double diff_x = b1.Position.X - b2.Position.X;
        double diff_y = b1.Position.Y - b2.Position.Y;
        return Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
    }
}
