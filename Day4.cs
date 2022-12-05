using System.Text.RegularExpressions;

namespace adventofcode2022;

public class Day4 : PuzzleBase
{
    public override void Solve()
    {
        var regex = new Regex(@"(\d+)-(\d+),(\d+)-(\d+)", RegexOptions.Compiled);
        var result = 0;
        var result2 = 0;
        foreach (var line in ReadLines())
        {
            var match = regex.Match(line);
            var l1 = int.Parse(match.Groups[1].Value);
            var r1 = int.Parse(match.Groups[2].Value);
            var l2 = int.Parse(match.Groups[3].Value);
            var r2 = int.Parse(match.Groups[4].Value);
            if ((l2 >= l1 && r2 <= r1) ||(l1 >= l2 && r1 <= r2))
                result++;
            
            if (Math.Max(l1,l2) <= Math.Min(r1, r2))
                result2++;
        }
        Console.WriteLine(result);
        Console.WriteLine(result2);
    }
}