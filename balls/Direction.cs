using Avalonia;

namespace Balls;

public readonly struct Direction
{
    readonly Point direction;

    public double X => this.direction.X;
    public double Y => this.direction.Y;

    public Direction(double x, double y) => this.direction = new Point(x, y);

    // public static double operator *(Direction d1, Direction d2) => d1.X * d2.X + d1.Y * d2.Y;

    public static implicit operator Point(Direction d) => d.direction;

    // public static explicit operator Point(Direction d) => d.direction;

    public Direction(Point p) => this.direction = p;

    public Direction WithX(double x) => new Direction(this.direction.WithX(x));

    public Direction WithY(double y) => new Direction(this.direction.WithY(y));

    public bool Equals(Point p) => this.direction.Equals(p);

    public override bool Equals(object? obj) => this.direction.Equals(obj);

    public override int GetHashCode() => this.direction.GetHashCode();

    public override string ToString() => $"{this.direction}";

    public static Direction GenerateRandomDirection()
    {
        Random rng = new Random();
        double x = 2 * rng.NextDouble() - 1;
        double y = 2 * rng.NextDouble() - 1;
        double norm = Math.Sqrt(x * x + y * y);
        return new Direction(x / norm, y / norm);
    }
}
