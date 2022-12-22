namespace adventofcode2022.helpers;

public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Y + p2.Y);
    }

    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.X - p2.X, p1.Y - p2.Y);
    }

    public static bool operator ==(Point p1, Point p2)
    {
        return p1.X == p2.X && p1.Y == p2.Y;
    }

    public static bool operator !=(Point p1, Point p2)
    {
        return !(p1.X == p2.X && p1.Y == p2.Y);
    }

    public static Point operator /(Point p, int l)
    {
        return new Point(p.X / l, p.Y / l);
    }

    public int IntLength()
    {
        return (int)(Math.Sqrt(X * X + Y * Y) + 1e-9);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public override int GetHashCode()
    {
        return X * 193 + Y;
    }

    public override bool Equals(object? other)
    {
        if (other is null)
            return false;
        if (other is not Point otherPoint)
            return false;
        return otherPoint == this;
    }

    public int X { get; }
    public int Y { get; }
}