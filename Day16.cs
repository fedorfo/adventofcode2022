using System.Text.RegularExpressions;

namespace adventofcode2022;

public class Day16 : PuzzleBase
{
    private const int Infinity = (int)1e9;

    private static readonly Regex LineRegex = new(
        @"Valve ([A-Z]{2}) has flow rate=(\d+); tunnel[s]* lead[s]* to valve[s]* (.*)",
        RegexOptions.Compiled
    );

    public override void Solve()
    {
        var (flowRate, connectedValves) = ReadInput();
        var distance = CalculateDistance(flowRate, connectedValves);
        var valvesWithRates = flowRate.Where(x => x.Value != 0).Select(x => x.Key).ToList();
        Console.WriteLine(Solve1(valvesWithRates, distance, flowRate));
        Console.WriteLine(Solve2(valvesWithRates, distance, flowRate));
    }

    private static int Solve1(
        List<string> valvesWithRates,
        Dictionary<(string, string), int> distance,
        Dictionary<string, int> flowRate
    )
    {
        var resultByMask = CalculateResultByMask(valvesWithRates, distance, flowRate, 30);
        var result = -Infinity;
        for (var mask = 0; mask < 1 << valvesWithRates.Count; mask++)
            result = Math.Max(resultByMask[mask], result);
        return result;
    }

    private static IEnumerable<int> GetSubmasks(int mask)
    {
        var current = mask;
        while (true)
        {
            yield return current;
            if (current == 0)
                yield break;
            current = (current - 1) & mask;
        }
    }

    private static int Solve2(
        List<string> valvesWithRates,
        Dictionary<(string, string), int> distance,
        Dictionary<string, int> flowRate
    )
    {
        var resultByMask = CalculateResultByMask(valvesWithRates, distance, flowRate, 26);
        var result = -Infinity;
        for (var mask = 0; mask < 1 << valvesWithRates.Count; mask++)
            foreach (var mask2 in GetSubmasks(((1 << valvesWithRates.Count) - 1) ^ mask))
                result = Math.Max(resultByMask[mask] + resultByMask[mask2], result);
        return result;
    }

    private static int[] CalculateResultByMask(
        List<string> valvesWithRates,
        Dictionary<(string, string), int> distance,
        Dictionary<string, int> flowRate,
        int minutes
    )
    {
        var dp = new int[minutes + 1, 1 << valvesWithRates.Count, valvesWithRates.Count];
        for (var i = 0; i < minutes + 1; i++)
        for (var j = 0; j < 1 << valvesWithRates.Count; j++)
        for (var k = 0; k < valvesWithRates.Count; k++)
            dp[i, j, k] = -Infinity;

        for (var i = 0; i < valvesWithRates.Count; i++)
        {
            var d = distance[("AA", valvesWithRates[i])];
            if (minutes - d - 1 >= 0)
                dp[minutes - d - 1, 1 << i, i] = flowRate[valvesWithRates[i]] * (minutes - d - 1);
        }

        for (var minutesLeft = minutes; minutesLeft >= 0; minutesLeft--)
        for (var mask = 0; mask < 1 << valvesWithRates.Count; mask++)
        for (var current = 0; current < valvesWithRates.Count; current++)
        for (var candidate = 0; candidate < valvesWithRates.Count; candidate++)
        {
            if ((mask & (1 << candidate)) != 0)
                continue;
            var d = distance[(valvesWithRates[current], valvesWithRates[candidate])];
            var f = flowRate[valvesWithRates[candidate]];
            if (minutesLeft >= d + 1)
            {
                dp[minutesLeft - d - 1, mask | (1 << candidate), candidate] = Math.Max(
                    dp[minutesLeft - d - 1, mask | (1 << candidate), candidate],
                    dp[minutesLeft, mask, current] + f * (minutesLeft - d - 1)
                );
            }
        }

        var result = new int[1 << valvesWithRates.Count];
        for (var i = 0; i < 1 << valvesWithRates.Count; i++)
            result[i] = -Infinity;
        for (var minutesLeft = minutes; minutesLeft >= 0; minutesLeft--)
        for (var mask = 0; mask < 1 << valvesWithRates.Count; mask++)
        for (var current = 0; current < valvesWithRates.Count; current++)
            result[mask] = Math.Max(dp[minutesLeft, mask, current], result[mask]);
        return result;
    }

    private (Dictionary<string, int> flowRate, Dictionary<string, List<string>> connectedValves) ReadInput()
    {
        var lines = ReadLines();
        var flowRate = new Dictionary<string, int>();
        var connectedValves = new Dictionary<string, List<string>>();
        foreach (var line in lines)
        {
            var match = LineRegex.Match(line);
            var valve = match.Groups[1].Value;
            flowRate[valve] = int.Parse(match.Groups[2].Value);
            connectedValves[valve] = match.Groups[3].Value.Split(", ").ToList();
        }

        return (flowRate, connectedValves);
    }

    private static Dictionary<(string, string), int> CalculateDistance(
        Dictionary<string, int> flowRate,
        Dictionary<string, List<string>> connectedValves
    )
    {
        var valves = flowRate.Keys.ToList();
        var distance = new Dictionary<(string, string), int>();
        foreach (var v in valves)
        foreach (var u in valves)
            distance[(v, u)] = v == u ? 0 : Infinity;

        foreach (var v in connectedValves.Keys)
        foreach (var u in connectedValves[v])
            distance[(v, u)] = 1;

        foreach (var k in valves)
        foreach (var v in valves)
        foreach (var u in valves)
            distance[(v, u)] = Math.Min(distance[(v, u)], distance[(v, k)] + distance[(k, u)]);

        return distance;
    }
}