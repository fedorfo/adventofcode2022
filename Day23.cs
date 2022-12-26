using adventofcode2022.helpers;

namespace adventofcode2022;

public class Day23 : PuzzleBase
{
    private static readonly List<List<V2>> Directions = new()
    {
        new List<V2> { new(-1, 0), new(-1, -1), new(-1, 1) },
        new List<V2> { new(1, 0), new(1, -1), new(1, 1) },
        new List<V2> { new(0, -1), new(-1, -1), new(1, -1) },
        new List<V2> { new(0, 1), new(-1, 1), new(1, 1) }
    };

    public override void Solve()
    {
        var lines = ReadLines();
        var elves = new List<V2>();
        for (var i = 0; i < lines.Count; i++)
        for (var j = 0; j < lines[i].Length; j++)
            if (lines[i][j] == '#')
                elves.Add(new V2(i, j));

        Console.WriteLine(Solve1(elves));
        Console.WriteLine(Solve2(elves));
    }

    private static int Solve1(List<V2> elves)
    {
        for (var i = 0; i < 10; i++)
            elves = EmulateRound(elves, i);

        var minX = elves.Select(x => x.X).Min();
        var maxX = elves.Select(x => x.X).Max();
        var minY = elves.Select(x => x.Y).Min();
        var maxY = elves.Select(x => x.Y).Max();
        return (maxX - minX + 1) * (maxY - minY + 1) - elves.Count;
    }
    
    private static int Solve2(List<V2> elves)
    {
        for(var i = 0;; i++)
        {
            var oldPositionsSet = elves.ToHashSet();
            elves = EmulateRound(elves, i);
            var newPositionsSet = elves.ToHashSet();
            if (oldPositionsSet.Intersect(newPositionsSet).Count() == elves.Count)
                return i+1;
        }
    }

    private static List<V2> EmulateRound(List<V2> elves, int round)
    {
        var oldPositions = elves.ToHashSet();
        var newPositions = new List<V2>();
        for (var i = 0; i < elves.Count; i++)
        {
            var elf = elves[i];
            if (elf.GetNeighbours8().Select(x => oldPositions.Contains(x) ? 1 : 0).Sum() != 0)
            {
                for (var directionIndex = 0; directionIndex < 4; directionIndex++)
                {
                    var directions = Directions[(directionIndex + round) % 4];
                    if (directions.Select(x => oldPositions.Contains(elf + x) ? 1 : 0).Sum() == 0)
                    {
                        newPositions.Add(elf + directions[0]);
                        break;
                    }
                }
            }
            if (newPositions.Count == i)
                newPositions.Add(elf);
        }

        var newPositionsCount = newPositions.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        for (var i = 0; i < elves.Count; i++)
            if (newPositionsCount[newPositions[i]] > 1)
                newPositions[i] = elves[i];

        return newPositions.OrderBy(x=>x.X).ToList();
    }

    private static void Print(List<V2> elves, int round)
    {
        Console.WriteLine($"== of Round {round} ==");
        var set = elves.ToHashSet();
        var minX = elves.Select(x => x.X).Min();
        var maxX = elves.Select(x => x.X).Max();
        var minY = elves.Select(x => x.Y).Min();
        var maxY = elves.Select(x => x.Y).Max();
        for (var x = minX; x <= maxX; x++)
        {
            for (var y = minY; y <= maxY; y++)
            {
                if (set.Contains(new V2(x, y)))
                    Console.Write('#');
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}