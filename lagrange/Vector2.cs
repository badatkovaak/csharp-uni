namespace Lagrange;

public struct Vector2
{
    public double x;
    public double y;

    public Vector2(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2((double, double) p)
    {
        this.x = p.Item1;
        this.y = p.Item2;
    }

    public static List<Vector2> TransformToVectors(List<(double, double)> points)
    {
        List<Vector2> result = new List<Vector2>(points.Count);

        foreach ((double, double) point in points)
            result.Add(new Vector2(point));

        return result;
    }

    public static bool operator ==(Vector2 lhs, Vector2 rhs) =>
        (lhs.x == rhs.x) && (lhs.y == rhs.y);

    public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

    public override bool Equals(object? obj)
    {
        if (!(obj is Vector2))
            return false;

        Vector2 vec = (Vector2)obj;

        return vec == this;
    }

    public override string ToString() => $"({this.x:F6}, {this.y:F6})";
}
