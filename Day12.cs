namespace adventofcode2022;

public class Day12 : PuzzleBase
{
    public override void Solve()
    {
        var lines = ReadLines();
        var field = lines.Select(x => x.ToList()).ToList();
        var start = FindPositions(field, 'S')[0];
        var end = FindPositions(field, 'E')[0];
        field[start.Item1][start.Item2] = 'a';
        field[end.Item1][end.Item2] = 'z';
        Console.WriteLine(Solve1(field, start, end));
        Console.WriteLine(Solve2(field, end));
    }

    private int Solve1(List<List<char>> field, (int, int) start, (int, int) end)
    {
        return Solve(field, start, end);
    }

    private int Solve2(List<List<char>> field, (int, int) end)
    {
        var positions = FindPositions(field, 'a');
        return positions.Select(x => Solve(field, x, end)).Min();
    }

    private List<(int, int)> FindPositions(List<List<char>> field, char symbol)
    {
        var result = new List<(int, int)>();
        var h = field.Count;
        var w = field[0].Count;
        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (field[i][j] == symbol)
                    result.Add((i, j));
            }
        }

        return result;
    }

    private int Solve(List<List<char>> field, (int, int) start, (int, int) end)
    {
        var h = field.Count;
        var w = field[0].Count;
        var q = new Queue<(int, int)>();
        var d = Enumerable.Range(0, h).Select(_ => Enumerable.Range(0, w).Select(_ => int.MaxValue).ToList()).ToList();
        d[start.Item1][start.Item2] = 0;
        q.Enqueue(start);
        while (q.Count > 0)
        {
            var current = q.Dequeue();
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) != 1)
                        continue;
                    var candidate = (current.Item1 + i, current.Item2 + j);
                    if (candidate.Item1 < 0 || candidate.Item1 >= h)
                        continue;
                    if (candidate.Item2 < 0 || candidate.Item2 >= w)
                        continue;
                    if (field[candidate.Item1][candidate.Item2] - field[current.Item1][current.Item2] > 1)
                        continue;
                    if (d[candidate.Item1][candidate.Item2] > d[current.Item1][current.Item2] + 1)
                    {
                        d[candidate.Item1][candidate.Item2] = d[current.Item1][current.Item2] + 1;
                        q.Enqueue(candidate);
                    }
                }
            }
        }

        return d[end.Item1][end.Item2];
    }
}