using adventofcode2022.helpers;

namespace adventofcode2022;

public class Day14 : PuzzleBase
{
    private List<List<V2>> ReadChains()
    {
        var lines = ReadLines();
        return lines.Select(line => line.Split(' ', '-', '>')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x =>
            {
                var pair = x.Split(",");
                return new V2(int.Parse(pair[0]), int.Parse(pair[1]));
            })
            .ToList()
        ).ToList();
    }

    private int Solve1(List<List<V2>> chains)
    {
        var field = new Dictionary<V2, bool>();
        FillField(chains, field);

        var maxy = chains.SelectMany(x => x).Select(x => x.Y).Max();
        var result = 0;
        while (DropSandUnit(field, maxy))
            result++;

        return result;
    }

    private int Solve2(List<List<V2>> chains)
    {
        var field = new Dictionary<V2, bool>();
        FillField(chains, field);

        var maxy = chains.SelectMany(x => x).Select(x => x.Y).Max();
        for (var i = 500 - maxy - 10; i <= 500 + maxy + 10; i++)
            field[new V2(i, maxy + 2)] = true;

        var result = 0;
        while (DropSandUnit(field, maxy + 2))
            result++;

        return result;
    }

    private static void FillField(List<List<V2>> chains, Dictionary<V2, bool> field)
    {
        foreach (var chain in chains)
        {
            for (var i = 1; i < chain.Count; i++)
            {
                var (start, end) = (chain[i - 1], chain[i]);
                while (true)
                {
                    field[start] = true;
                    if (start == end)
                        break;
                    var v = end - start;
                    v /= v.ManhattanLength();
                    start += v;
                }
            }
        }
    }

    private bool DropSandUnit(Dictionary<V2, bool> field, int maxy)
    {
        var sandUnit = new V2(500, 0);
        if (field.ContainsKey(sandUnit))
            return false;

        var directions = new List<V2> { new(0, 1), new(-1, 1), new(1, 1) };
        while (true)
        {
            var moved = false;
            foreach (var direction in directions)
            {
                if (!field.ContainsKey(sandUnit + direction))
                {
                    sandUnit += direction;
                    moved = true;
                    break;
                }
            }

            if (!moved)
            {
                field[sandUnit] = true;
                return true;
            }

            if (sandUnit.Y >= maxy)
                return false;
        }
    }

    public override void Solve()
    {
        var chains = ReadChains();
        Console.WriteLine(Solve1(chains));
        Console.WriteLine(Solve2(chains));
    }
}