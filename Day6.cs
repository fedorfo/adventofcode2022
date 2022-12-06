namespace adventofcode2022;

public class Day6 : PuzzleBase
{
    public override void Solve()
    {
        var signal = ReadLines()[0];
        Console.WriteLine(Solve(signal, 4));
        Console.WriteLine(Solve(signal, 14));
    }

    private static int Solve(string signal, int count)
    {
        for (var i = 0; i < signal.Length - count; i++)
        {
            if (signal.Substring(i, count).ToHashSet().Count == count)
                return i + count;
        }

        throw new ArgumentException(null, nameof(signal));
    }
}