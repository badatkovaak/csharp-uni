using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;

namespace Balls;

public class BallCanvas : Canvas
{
    List<Ball> balls;
    const double move_speed = 0.1;
    const int milliseconds_interval = 15;

    // const double move_distance = move_speed * milliseconds_interval;
    const double move_distance = 1;

    public BallCanvas()
    {
        this.Background = Brushes.White;
        this.balls = new List<Ball>();

        this.PointerReleased += PointerReleasedHandler;
        this.SizeChanged += OnSizeChanged;

        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(milliseconds_interval),
        };

        timer.Tick += (object? sender, EventArgs e) =>
            Dispatcher.UIThread.Post(() => this.MoveAllBalls());
        timer.Start();
    }

    public void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        foreach (Ball ball in this.Children)
        {
            double new_x = e.NewSize.Width * ball.Position.X / e.PreviousSize.Width;
            double new_y = e.NewSize.Height * ball.Position.Y / e.PreviousSize.Height;

            ball.Position = new Point(new_x, new_y);
        }
    }

    public void PointerReleasedHandler(object? sender, PointerReleasedEventArgs e)
    {
        Point p = e.GetPosition(this);
        Ball b = new Ball(p, Direction.GenerateRandomDirection());

        this.Children.Add(b);
        this.balls.Add(b);

        Console.WriteLine($"Added point {b.Position} {b.Direction}");
    }

    private int IsOutsideBounds1(double x, double y)
    {
        int result = 0;

        if (Ball.Radius + x > this.Bounds.Width)
            result += 1;

        if (x - Ball.Radius < 0)
            result += 1;

        if (Ball.Radius + y > this.Bounds.Height)
            result += 2;

        if (y - Ball.Radius < 0)
            result += 2;

        return result;
    }

    public void MoveBall(Ball ball, double distance)
    {
        double new_x = ball.Position.X + ball.Direction.X * distance;
        double new_y = ball.Position.Y + ball.Direction.Y * distance;

        int bounds_collisions = IsOutsideBounds1(new_x, new_y);

        if ((bounds_collisions & 1) != 0)
            ball.Direction = ball.Direction.WithX(-ball.Direction.X);

        if ((bounds_collisions & 2) != 0)
            ball.Direction = ball.Direction.WithY(-ball.Direction.Y);

        ball.Position = new Point(new_x, new_y);
    }

    public void HandleBallCollisions()
    {
        for (int i = 0; i < this.balls.Count; i++)
        for (int j = i + 1; j < this.balls.Count; j++)
        {
            Ball b1 = this.balls[i];
            Ball b2 = this.balls[j];

            if (Ball.Distance(b1, b2) > 2 * Ball.Radius)
                continue;

            Console.WriteLine($"from - {b1.Direction} {b2.Direction}");

            Point c = b1.Position - b2.Position;
            Point v = (Point)b1.Direction - b2.Direction;
            double coeff1 = PointFunctions.DotProduct(v, c);
            double coeff2 = PointFunctions.DotProduct(c, c);

            b1.Direction = new Direction(b1.Direction - (coeff1 / coeff2) * c);
            b2.Direction = new Direction(b2.Direction + (coeff1 / coeff2) * c);

            Console.WriteLine($"to - {b1.Direction} {b2.Direction}");
        }
    }

    public void MoveAllBalls()
    {
        foreach (Ball ball in this.balls)
            MoveBall(ball, move_distance);

        HandleBallCollisions();
    }
}
