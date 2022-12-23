namespace adventofcode2022;

public class Day17 : PuzzleBase
{
    private static readonly List<string> rocks =
        new()
        {
            "####",
            ".#.\n###\n.#.",
            "..#\n..#\n###",
            "#\n#\n#\n#",
            "##\n##"
        };

    public override void Solve()
    {
        var jetPattern = Console.ReadLine()!;
        Console.WriteLine(Solve1(jetPattern));
        Console.WriteLine(Solve2(jetPattern));
    }

    private int Solve1(string jetPattern)
    {
        var chamber = new Chamber();
        var move = 0;
        for (var i = 0; i < 2022; i++)
            chamber.Apply(new Rock(rocks[i % 5], 2), jetPattern, ref move);
        return chamber.Height;
    }

    private long Solve2(string jetPattern)
    {
        var chamber = new Chamber();
        var move = 0;
        var closedAt = new Dictionary<int, (int h, int rockCount)>();
        var i = 0;
        long toAdd = 0;
        var rest = 0;
        while (true)
        {
            chamber.Apply(new Rock(rocks[i % 5], 2), jetPattern, ref move);
            if (chamber.Closed)
            {
                if (!closedAt.ContainsKey(move % jetPattern.Length))
                {
                    closedAt[move % jetPattern.Length] = (chamber.Height, i);
                }
                else
                {
                    var (h, rockCount) = closedAt[move % jetPattern.Length];
                    var cycleLength = i - rockCount;
                    toAdd = (chamber.Height - h) * ((1000000000000 - i - 1) / cycleLength);
                    rest = (int)((1000000000000 - i - 1) % cycleLength);
                    i++;
                    break;
                }
            }

            i++;
        }

        for (var j = 0; j < rest; j++)
            chamber.Apply(new Rock(rocks[(i + j) % 5], 2), jetPattern, ref move);
        return chamber.Height + toAdd;
    }

    private class Rock
    {
        private readonly string rawPresentation;
        private readonly int shift;

        public Rock(string rawPresentation, int shift)
        {
            this.rawPresentation = rawPresentation;
            this.shift = shift;
        }

        private int Width => rawPresentation.Split("\n")[0].Length;

        public Rock MoveLeft()
        {
            return shift > 0 ? new Rock(rawPresentation, shift - 1) : this;
        }

        public Rock MoveRight()
        {
            return shift + Width < 7 ? new Rock(rawPresentation, shift + 1) : this;
        }

        public List<string> GetChamberLines()
        {
            return rawPresentation
                .Split("\n")
                .Select(x => ("".PadLeft(shift, '.') + x).PadRight(7, '.'))
                .Reverse()
                .ToList();
        }
    }

    private class Chamber
    {
        private readonly List<string> lines = new();
        public int Height => lines.Count;

        public bool Closed => lines.Last() == "#######";

        private bool Match(Rock rock, int h)
        {
            if (h < 0)
                return false;

            var rockLines = rock.GetChamberLines();
            for (var i = 0; i < rockLines.Count; i++)
            {
                var chamberLine = GetLine(i + h);
                var rockLine = rockLines[i];
                for (var j = 0; j < 7; j++)
                    if (rockLine[j] == '#' && chamberLine[j] == '#')
                        return false;
            }

            return true;
        }

        private string GetLine(int h)
        {
            return lines.Count > h ? lines[h] : ".......";
        }

        public void Apply(Rock rock, string jetPattern, ref int move)
        {
            var h = lines.Count + 3;
            while (true)
            {
                var newRock = jetPattern[move % jetPattern.Length] == '<' ? rock.MoveLeft() : rock.MoveRight();
                if (Match(newRock, h))
                    rock = newRock;
                move++;
                if (!Match(rock, h - 1))
                {
                    Apply(rock, h);
                    return;
                }

                h--;
            }
        }

        private void Apply(Rock rock, int h)
        {
            var rockLines = rock.GetChamberLines();
            while (lines.Count < h + rockLines.Count)
                lines.Add(".......");
            for (var i = 0; i < rockLines.Count; i++)
                lines[i + h] = string.Join(
                    "",
                    Enumerable.Range(0, 7).Select(j => rockLines[i][j] == '#' ? '#' : lines[i + h][j]).ToList()
                );
        }

        public void Print()
        {
            for (var i = lines.Count - 1; i >= 0; i--)
                Console.WriteLine("|" + lines[i] + "|");
            Console.WriteLine("+-------+");
        }
    }
}