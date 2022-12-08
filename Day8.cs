namespace adventofcode2022;

public class Day8 : PuzzleBase
{
    private static readonly List<Tuple<int, int>> Directions = new()
    {
        Tuple.Create(1, 0), Tuple.Create(-1, 0), Tuple.Create(0, 1), Tuple.Create(0, -1)
    };

    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(Solve1(lines));
        Console.WriteLine(Solve2(lines));
    }

    private static int Solve1(IReadOnlyList<string> lines)
    {
        var result = 0;
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                if (IsVisible(lines, i, j))
                    result++;
            }
        }

        return result;
    }

    private static int Solve2(IReadOnlyList<string> lines)
    {
        var result = 0;
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                var viewingDistance = GetViewingDistance(lines, i, j);
                if (viewingDistance > result)
                    result = viewingDistance;
            }
        }

        return result;
    }

    private static int GetViewingDistance(IReadOnlyList<string> lines, int x, int y)
    {
        var result = 1;
        foreach (var (vx, vy) in Directions)
            result *= GetVisibleTreesInDirection(lines, x, y, vx, vy);
        return result;
    }

    private static int GetVisibleTreesInDirection(IReadOnlyList<string> lines, int x, int y, int vx, int vy)
    {
        var value = lines[x][y];
        var result = 0;
        while (true)
        {
            (x, y) = (x + vx, y + vy);
            if (!IsValidCoordinates(lines, x, y))
                return result;
            result++;
            if (lines[x][y] >= value)
                return result;
        }
    }

    private static bool IsVisible(IReadOnlyList<string> lines, int x, int y)
    {
        var result = false;
        foreach (var (vx, vy) in Directions)
            result = result || IsVisibleInDirection(lines, x, y, vx, vy);
        return result;
    }

    private static bool IsVisibleInDirection(IReadOnlyList<string> lines, int x, int y, int vx, int vy)
    {
        var value = lines[x][y];
        while (true)
        {
            (x, y) = (x + vx, y + vy);
            if (!IsValidCoordinates(lines, x, y))
                return true;
            if (lines[x][y] >= value)
                return false;
        }
    }

    private static bool IsValidCoordinates(IReadOnlyList<string> lines, int x, int y)
    {
        return x >= 0 && x < lines.Count && y >= 0 && y < lines[x].Length;
    }
}