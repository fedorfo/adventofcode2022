using adventofcode2022.helpers;
using static adventofcode2022.helpers.Helpers;
using static System.Linq.Enumerable;

namespace adventofcode2022;

public class Day24 : PuzzleBase
{
    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(Solve1(lines));
        Console.WriteLine(Solve2(lines));
    }

    private int Solve1(List<string> lines)
    {
        var (h, w) = (lines.Count-2, lines[0].Length-2);
        var start = new V2(0, 1);
        var end = new V2(h + 1, w);
        return CalculateTime(lines, start, end, 0);
    }
    
    private int Solve2(List<string> lines)
    {
        var (h, w) = (lines.Count-2, lines[0].Length-2);
        var start = new V2(0, 1);
        var end = new V2(h + 1, w);
        var step1 = CalculateTime(lines, start, end, 0);
        var step2 = CalculateTime(lines, end, start, step1);
        return CalculateTime(lines, start, end, step2);
    }

    private int CalculateTime(List<string> lines, V2 start, V2 end, int startTime)
    {
        var (h, w) = (lines.Count-2, lines[0].Length-2);
        var cycleLength = LeastCommonMultiple(h, w);
        var queue = new Queue<(V2, int)>();
        var marked = new HashSet<(V2, int)>();
        queue.Enqueue((start, startTime));
        marked.Add((start, startTime%cycleLength));
        while (queue.Count != 0)
        {
            var (v, time) = queue.Dequeue();
            if (v == end)
                return time;
            foreach (var u in v.GetNeighbours4().Concat(new[] { v }))
            {
                if (marked.Contains((u, (time + 1) % cycleLength)))
                    continue;
                if (!IsEmpty(lines, u, time + 1))
                    continue;
                marked.Add((u, (time + 1) % cycleLength));
                queue.Enqueue((u, time + 1));
            }
        }

        return -1;
    }

    private bool IsEmpty(List<string> lines, V2 p, int time)
    {
        var (h, w) = (lines.Count-2, lines[0].Length-2);
        if (p.X < 0 || p.X > h + 1 || p.Y < 0 || p.Y > w + 1 || lines[p.X][p.Y] == '#')
            return false;
        if (p.X == 0 || p.X == h + 1)
            return true; 
        if (lines[Mod(p.X - 1 + time, h) + 1][p.Y] == '^')
            return false;
        if (lines[Mod(p.X - 1 - time, h) + 1][p.Y] == 'v')
            return false;
        if (lines[p.X][Mod(p.Y - 1 + time, w) + 1] == '<')
            return false;
        if (lines[p.X][Mod(p.Y - 1 - time, w) + 1] == '>')
            return false;
        return true;
    }

    private static int LeastCommonMultiple(int a, int b)
    {
        return Range(1, a * b - 1).First(x => x % a == 0 && x % b == 0);
    }
}