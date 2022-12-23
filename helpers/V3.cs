using static System.Math;

namespace adventofcode2022.helpers;

public record V3(int X, int Y, int Z)
{
    public static V3 operator +(V3 l, V3 r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
    public static V3 operator -(V3 l, V3 r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
    public int ManhattanLength() => Abs(X) + Abs(Y) + Abs(Z);
    public int ChebyshevLength() => Max(Max(Abs(X), Abs(Y)), Abs(Z));
    public IEnumerable<V3> GetNeighbours6()
    {
        for (var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        for (var z = -1; z <= 1; z++)
        {
            var candidate = new V3(x,y,z);
            if (candidate.ManhattanLength() == 1)
                yield return this + candidate;
        }
    }

    public static V3 Parse(string value, string[]? separators = null)
    {
        var defaultSeparators = new[] { ",", " " };
        var args = value
            .Split(separators ?? defaultSeparators, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
        return new V3(args[0], args[1], args[2]);
    }
}