namespace adventofcode2022;

public class Day3 : PuzzleBase
{
    private class Backpack
    {
        public Backpack(List<HashSet<int>> compartments)
        {
            this.compartments = compartments;
        }

        private readonly List<HashSet<int>> compartments;

        public IEnumerable<int> Intersection => compartments[0].Intersect(compartments[1]).ToHashSet();
        public IEnumerable<int> Union => compartments[0].Union(compartments[1]).ToHashSet();
    }

    private static int GetPriority(char item)
    {
        return item switch
        {
            >= 'a' and <= 'z' => item - 'a' + 1,
            >= 'A' and <= 'Z' => item - 'A' + 27,
            _ => throw new ArgumentException(null, nameof(item))
        };
    }

    public override void Solve()
    {
        var backpacks = ReadBackpacks();
        Console.WriteLine(GetScore(backpacks));
        Console.WriteLine(GetScore2(backpacks));
    }

    private int GetScore(IReadOnlyList<Backpack> backpacks)
    {
        return backpacks.Sum(backpack => backpack.Intersection.Sum());
    }
    
    private int GetScore2(IReadOnlyList<Backpack> backpacks)
    {
        var groupsCount = backpacks.Count / 3;
        var result = 0;
        for (var i = 0; i < groupsCount; i++)
        {
            var intersection = backpacks[i*3].Union;
            for (var j = 1; j < 3; j++)
                intersection = intersection.Intersect(backpacks[i*3+j].Union).ToHashSet();
            result += intersection.Sum();
        }
        return result;
    }

    private List<Backpack> ReadBackpacks()
    {
        var lines = ReadLines();
        var backpacks = new List<Backpack>();
        foreach (var line in lines)
        {
            var compartment1 = line.Substring(0, line.Length / 2);
            var compartment2 = line.Substring(line.Length / 2, line.Length / 2);
            var backpack = new Backpack(new List<HashSet<int>>
            {
                compartment1.Select(GetPriority).ToHashSet(), compartment2.Select(GetPriority).ToHashSet()
            }); 
            backpacks.Add(backpack);
        }
        return backpacks;
    }
}