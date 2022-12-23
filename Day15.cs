using System.Text.RegularExpressions;
using adventofcode2022.helpers;

namespace adventofcode2022;

public class Day15 : PuzzleBase
{
    private static readonly Regex LineRegex = new(
        @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)",
        RegexOptions.Compiled
    );

    public override void Solve()
    {
        var lines = ReadLines();
        var (sensors, beacons) = ReadSensorsAndBeacons(lines);
        Console.WriteLine(CountPlacesWithoutBeacon(sensors, beacons, 2000000));
        
        for (var i = 0; i <= 4000000; i++)
        {
            var potentialBeacon = FindPotentialBeacon(sensors, beacons, i, 0, 4000000);
            if (potentialBeacon is not null)
            {
                Console.WriteLine(potentialBeacon.X * (long)4000000 + potentialBeacon.Y);
                return;
            }
        }
    }

    private List<(int l, int r)> GetSegments(List<V2> sensors, List<V2> beacons, int y)
    {
        var segments = new List<(int l, int r)>();
        for (var i = 0; i < sensors.Count; i++)
        {
            var length = (sensors[i] - beacons[i]).ManhattanLength();
            var delta = length - Math.Abs(sensors[i].Y - y);
            if (delta >= 0)
                segments.Add((sensors[i].X - delta, sensors[i].X + delta));
        }
        segments = segments.OrderBy(x => x.l).ToList();
        var result = new List<(int l, int r)> { segments[0] };
        for (var i = 1; i < segments.Count; i++)
        {
            var last = result.Last();
            if (last.r + 1 >= segments[i].l)
            {
                if (last.r < segments[i].r)
                {
                    result.RemoveAt(result.Count - 1);
                    result.Add((last.l, segments[i].r));
                }
            }
            else
                result.Add(segments[i]);
        }
        return result;
    }

    private int CountPlacesWithoutBeacon(List<V2> sensors, List<V2>  beacons, int y)
    {
        var segments = GetSegments(sensors, beacons, y);
        var beaconsCount = beacons.Where(x=>x.Y == y).ToHashSet().Count;
        return segments.Select(x=>x.r-x.l+1).Sum() - beaconsCount;
    }

    private V2? FindPotentialBeacon(List<V2> sensors, List<V2>  beacons, int y, int rangeFrom, int rangeTo)
    {
        var segments = GetSegments(sensors, beacons, y);
        if (segments.First().l > rangeFrom)
            return new V2(0, y);
        if (segments.Last().r < rangeTo)
            return new V2(rangeTo, y);
        if (segments.Count > 1)
            return new V2(segments.First().r + 1, y);
        return null;
    }

    private static (List<V2> sensors, List<V2> beacons) ReadSensorsAndBeacons(List<string> lines)
    {
        var sensors = new List<V2>();
        var beacons = new List<V2>();
        foreach (var line in lines)
        {
            var match = LineRegex.Match(line);
            sensors.Add(new V2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
            beacons.Add(new V2(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
        }

        return (sensors, beacons);
    }
}