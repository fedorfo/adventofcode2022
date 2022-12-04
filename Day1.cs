namespace adventofcode2022;

public class Day1: IPuzzle
{
    public void Solve()
    {
        var elves = ReadElves().OrderByDescending(x=>x).ToList();
        Console.WriteLine(elves[0]);
        Console.WriteLine(elves[0] + elves[1] + elves[2]);
    }

    private static List<int> ReadElves()
    {
        var elves = new List<int>();
        var current = 0;
        while (true)
        {
            var line = Console.ReadLine();
            if (line is null || string.IsNullOrEmpty(line))
            {
                elves.Add(current);
                current = 0;
                if (line is null)
                    return elves;
            }
            else
            {
                current += int.Parse(line);
            }
        }
    }
}