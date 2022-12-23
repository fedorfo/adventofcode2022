using static System.Math;
namespace adventofcode2022.helpers;

public record V2(int X, int Y)
{
    public static V2 operator +(V2 l, V2 r) => new(l.X + r.X, l.Y + r.Y);
    public static V2 operator -(V2 l, V2 r) => new(l.X - r.X, l.Y - r.Y);
    public static V2 operator /(V2 p, int l) => new V2(p.X / l, p.Y / l);
    public int ManhattanLength() => Abs(X) + Abs(Y);
    public int ChebyshevLength() => Max(Abs(X), Abs(Y));
}
