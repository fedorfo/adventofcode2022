namespace adventofcode2022;

public class Day10 : PuzzleBase
{
    public override void Solve()
    {
        var lines = ReadLines();
        var values = CalculateValues(lines);
        Solve1(values);
        Solve2(values);
    }

    private List<long> CalculateValues(IEnumerable<string> lines)
    {
        long current = 1;
        var values = new List<long>{1};
        
        foreach (var line in lines)
        {
            var args = line.Split(" ");
            switch (args[0])
            {
                case "noop":
                    values.Add(current);
                    break;
                case "addx":
                    values.Add(current);
                    current += long.Parse(args[1]);
                    values.Add(current);
                    break;
                default:
                    throw new Exception($"Invalid arg {args[0]}");
            }
        }
        return values;
    }

    private static void Solve1(List<long> values)
    {
        var indexes = new[] { 20, 60, 100, 140, 180, 220 };
        var result = indexes.Select(x => values[x-1] * x).Sum();
        Console.WriteLine(result);
    }
    
    private static void Solve2(List<long> values)
    {
        for (var i = 0; i < 240; i++)
        {
            var position = i % 40;
            Console.Write(Math.Abs(position - values[i]) <= 1 ? '#' : '.');
            if (i % 40 == 39)
                Console.WriteLine();
        }
    }
}