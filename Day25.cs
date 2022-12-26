using static adventofcode2022.helpers.Helpers;

namespace adventofcode2022;

public class Day25 : PuzzleBase
{
    private static readonly Dictionary<char, long> SnafuDigitToLong = new()
    {
        { '2', 2 }, { '1', 1 }, { '0', 0 }, { '-', -1 }, { '=', -2 }
    };

    private static readonly Dictionary<long, char> LongDigitToSnafu = new()
    {
        { 2, '2' }, { 1, '1' }, { 0, '0' }, { -1, '-' }, { -2, '=' }
    };

    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(Solve1(lines));
    }

    private string Solve1(List<string> lines)
    {
        long result = 0;
        foreach (var line in lines)
            result += SnafuToLong(line);
        return LongToSnafu(result);
    }

    private static long SnafuToLong(string value)
    {
        return value.Reverse().Select((x, i) => SnafuDigitToLong[x] * LongPow(5, i)).Sum();
    }

    private static string LongToSnafu(long value)
    {
        var result = "";
        while (value > 0)
        {
            var reminder = value % 5;
            if (reminder > 2)
                reminder -= 5;
            result += LongDigitToSnafu[reminder];
            value /= 5;
            if (reminder < 0)
                value += 1;
        }
        return string.Join("",result.Reverse());
    }
}