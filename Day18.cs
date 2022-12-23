using adventofcode2022.helpers;

namespace adventofcode2022;

public class Day18 : PuzzleBase
{
    public override void Solve()
    {
        var droplets = ReadLines()
            .Select(x=>V3.Parse(x))
            .ToHashSet();
        Console.WriteLine(Solve1(droplets));
        Console.WriteLine(Solve2(droplets));
    }

    private int Solve1(HashSet<V3> droplets)
    {
        return droplets.Sum(droplet => droplet.GetNeighbours6().Count(x => !droplets.Contains(x)));
    }
    
    private int Solve2(HashSet<V3> droplets)
    {
        var maxValue = droplets.Select(x => x.ChebyshevLength()).Max() + 1;
        var queue = new Queue<V3>();
        var marked = new HashSet<V3>();
        var start = new V3(maxValue,maxValue,maxValue);
        queue.Enqueue(start);
        marked.Add(start);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var x in current.GetNeighbours6())
            {
                if (x.ChebyshevLength() <= maxValue && !droplets.Contains(x) && !marked.Contains(x))
                {
                    queue.Enqueue(x);
                    marked.Add(x);
                }
            }
        }
        return droplets.Sum(droplet => droplet.GetNeighbours6().Count(x => marked.Contains(x)));
    }
}