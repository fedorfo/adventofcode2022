using adventofcode2022.helpers;
using static System.Linq.Enumerable;

namespace adventofcode2022;

public class Day9 : PuzzleBase
{
    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(Solve(lines, 2));
        Console.WriteLine(Solve(lines, 10));
    }

    private static int Solve(IReadOnlyList<string> lines, int ropeLength)
    {
        var rope = new Rope(Range(0, ropeLength).Select(_ => new V2(0, 0)).ToArray());
        var visited = new HashSet<V2> { rope.Tail };
        foreach (var line in lines)
        {
            var args = line.Split(" ");
            var (direction, count) = (args[0], int.Parse(args[1]));
            for (var i = 0; i < count; i++)
            {
                rope = rope.Move(direction);
                visited.Add(rope.Tail);
            }
        }

        return visited.Count;
    }

    private class Rope
    {
        private readonly V2[] knots;

        public Rope(IReadOnlyCollection<V2> knots)
        {
            this.knots = knots.ToArray();
        }


        public V2 Head => knots[0];
        public V2 Tail => knots[^1];

        public Rope Move(string direction)
        {
            var shift = new Dictionary<string, V2>
            {
                { "D", new V2(0, 1) },
                { "R", new V2(1, 0) },
                { "L", new V2(-1, 0) },
                { "U", new V2(0, -1) }
            }[direction];
            var result = knots.ToList();
            result[0] = knots[0] + shift;
            for (var i = 1; i < knots.Length; i++)
            {
                result[i] = NormalizeTail(result[i - 1], knots[i]);
                if (result[i] == knots[i])
                    break;
            }

            return new Rope(result);
        }

        private static V2 NormalizeTail(V2 head, V2 tail)
        {
            var v = tail - head;
            if (Math.Abs(v.X) == 2 && Math.Abs(v.Y) == 2)
                return new V2(head.X + v.X / 2, head.Y + v.Y / 2);
            if (Math.Abs(v.X) == 2)
                return new V2(head.X + v.X / 2, head.Y);
            if (Math.Abs(v.Y) == 2)
                return new V2(head.X, head.Y + v.Y / 2);
            return tail;
        }

        public override string ToString()
        {
            return string.Join("-", knots.Select(x => x.ToString()));
        }

        public void Print()
        {
            var points = knots.Concat(new[] { new V2(0, 0) }).ToList();

            var xshift = -points.Select(x => x.X).Min();
            var yshift = -points.Select(x => x.Y).Min();
            var xsize = Math.Max(points.Select(x => x.X).Max(), 0) + xshift + 1;
            var ysize = Math.Max(points.Select(x => x.Y).Max(), 0) + yshift + 1;

            var result = Range(0, xsize).Select(_ => Range(0, ysize).Select(_ => ".").ToList())
                .ToList();
            for (var i = knots.Length - 1; i >= 0; i--)
                result[knots[i].X + xshift][knots[i].Y + yshift] = i > 0 ? i.ToString() : "H";

            if (result[xshift][yshift] == ".")
                result[xshift][yshift] = "S";

            Console.WriteLine("============");
            for (var j = ysize - 1; j >= 0; j--)
            {
                for (var i = 0; i < xsize; i++)
                    Console.Write(result[i][j]);
                Console.WriteLine();
            }
        }
    }
}