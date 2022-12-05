namespace adventofcode2022;

public class Day1: PuzzleBase
{
    public override void Solve()
    {
        var elves = ReadElves().OrderByDescending(x=>x).ToList();
        Console.WriteLine(elves[0]);
        Console.WriteLine(elves[0] + elves[1] + elves[2]);
    }
    
    private List<int> ReadElves()
    {
        var elves = new List<int>();
        var current = 0;
        foreach (var line in ReadLines().Concat(new[]{""}).ToList())
        {
            if (string.IsNullOrEmpty(line))
            {
                elves.Add(current);
                current = 0;
            }
            else
            {
                current += int.Parse(line);
            }
        }
        return elves;
    }
}