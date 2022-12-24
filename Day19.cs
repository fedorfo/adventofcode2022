using System.Text.RegularExpressions;
using adventofcode2022.helpers;
using static System.Math;

namespace adventofcode2022;

public class Day19 : PuzzleBase
{
    //public override string InputFileName => "sample.txt";

    public override void Solve()
    {
        var blueprints = ReadLines().Select(Blueprint.Parse).ToList();
        Console.WriteLine(Solve1(blueprints));
        Console.WriteLine(Solve2(blueprints));
    }

    private static int Solve2(List<Blueprint> blueprints)
    {
        var total = 1;
        foreach (var blueprint in blueprints.Take(3))
        {
            var result = Helpers.Measure(() => Calculate(blueprint,
                new Dictionary<long, int>(), 1, 0, 0, 0, 0, 0, 0,
                0, 32));
            Console.WriteLine(result);
            total *= result;
        }

        return total;
    }

    private static int Solve1(List<Blueprint> blueprints)
    {
        var total = 0;
        foreach (var blueprint in blueprints)
        {
            var geodes = Helpers.Measure(() =>
                Calculate(blueprint, new Dictionary<long, int>(), 1, 0, 0, 0, 0, 0, 0, 0, 24));
            var result = geodes * blueprint.Id;
            Console.WriteLine(result);
            total += result;
        }

        return total;
    }

    private static int SumArithmeticProgression(int n)
    {
        return n >= 0 ? (n + 1) * n / 2 : 0;
    }

    private static long GetMask(int oreRobots,
        int clayRobots,
        int obsidianRobots,
        int geodeRobots,
        int ore,
        int clay,
        int obsidian,
        int geode,
        int minutes)
    {
        return (long)oreRobots | ((long)clayRobots << 5) | ((long)obsidianRobots << 10) | ((long)geodeRobots << 15) |
               ((long)ore << 23) | ((long)clay << 31) | ((long)obsidian << 39) | ((long)geode << 47) |
               ((long)minutes << 55);
    }

    private static int Calculate(
        Blueprint blueprint,
        Dictionary<long, int> cache,
        int oreRobots,
        int clayRobots,
        int obsidianRobots,
        int geodeRobots,
        int ore,
        int clay,
        int obsidian,
        int geode,
        int minutes
    )
    {
        if (obsidianRobots == 0 && SumArithmeticProgression(minutes - 1) <= blueprint.GeodeRobotObsidianCost)
            return 0;

        if (clayRobots == 0 && SumArithmeticProgression(minutes - 1) <= blueprint.ObsidianRobotClayCost)
            return 0;

        if (minutes == 0)
            return geode;

        var mask = GetMask(
            oreRobots,
            clayRobots,
            obsidianRobots,
            geodeRobots,
            ore,
            clay,
            obsidian,
            geode,
            minutes
        );
        if (cache.TryGetValue(mask, out var cacheValue))
            return cacheValue;

        var res = 0;
        if (ore >= blueprint.GeodeRobotOreCost && obsidian >= blueprint.GeodeRobotObsidianCost)
            res = Max(
                res,
                Calculate(
                    blueprint,
                    cache,
                    oreRobots,
                    clayRobots,
                    obsidianRobots,
                    geodeRobots + 1,
                    ore + oreRobots - blueprint.GeodeRobotOreCost,
                    clay + clayRobots,
                    obsidian + obsidianRobots - blueprint.GeodeRobotObsidianCost,
                    geode + geodeRobots,
                    minutes - 1
                )
            );

        if (ore >= blueprint.OreRobotOreCost && ore <= 10)
            res = Max(
                res,
                Calculate(
                    blueprint,
                    cache,
                    oreRobots + 1,
                    clayRobots,
                    obsidianRobots,
                    geodeRobots,
                    ore + oreRobots - blueprint.OreRobotOreCost,
                    clay + clayRobots,
                    obsidian + obsidianRobots,
                    geode + geodeRobots,
                    minutes - 1
                )
            );

        if (ore >= blueprint.ObsidianRobotOreCost && clay >= blueprint.ObsidianRobotClayCost)
            res = Max(
                res,
                Calculate(
                    blueprint,
                    cache,
                    oreRobots,
                    clayRobots,
                    obsidianRobots + 1,
                    geodeRobots,
                    ore + oreRobots - blueprint.ObsidianRobotOreCost,
                    clay + clayRobots - blueprint.ObsidianRobotClayCost,
                    obsidian + obsidianRobots,
                    geode + geodeRobots,
                    minutes - 1
                )
            );

        if (ore >= blueprint.ClayRobotOreCost && clay <= 15)
            res = Max(
                res,
                Calculate(
                    blueprint,
                    cache,
                    oreRobots,
                    clayRobots + 1,
                    obsidianRobots,
                    geodeRobots,
                    ore + oreRobots - blueprint.ClayRobotOreCost,
                    clay + clayRobots,
                    obsidian + obsidianRobots,
                    geode + geodeRobots,
                    minutes - 1
                )
            );

        if (!(ore >= blueprint.OreRobotOreCost && ore >= blueprint.ClayRobotOreCost &&
              ore >= blueprint.ObsidianRobotOreCost && ore >= blueprint.GeodeRobotOreCost &&
              clay >= blueprint.ObsidianRobotClayCost && obsidian >= blueprint.GeodeRobotObsidianCost))
            res = Max(
                res,
                Calculate(
                    blueprint,
                    cache,
                    oreRobots,
                    clayRobots,
                    obsidianRobots,
                    geodeRobots,
                    ore + oreRobots,
                    clay + clayRobots,
                    obsidian + obsidianRobots,
                    geode + geodeRobots,
                    minutes - 1
                )
            );
        cache[mask] = res;

        return res;
    }

    public record Blueprint(
        int Id,
        int OreRobotOreCost,
        int ClayRobotOreCost,
        int ObsidianRobotOreCost,
        int ObsidianRobotClayCost,
        int GeodeRobotOreCost,
        int GeodeRobotObsidianCost
    )
    {
        private static readonly Regex BlueprintRegex = new(
            @"Blueprint (\d+): " +
            @"Each ore robot costs (\d+) ore. " +
            @"Each clay robot costs (\d+) ore. " +
            @"Each obsidian robot costs (\d+) ore and (\d+) clay. " +
            @"Each geode robot costs (\d+) ore and (\d+) obsidian.",
            RegexOptions.Compiled
        );

        public static Blueprint Parse(string line)
        {
            var match = BlueprintRegex.Match(line);
            return new Blueprint(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value),
                int.Parse(match.Groups[5].Value),
                int.Parse(match.Groups[6].Value),
                int.Parse(match.Groups[7].Value)
            );
        }
    }
}