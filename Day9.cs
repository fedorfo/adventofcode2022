namespace adventofcode2022;

public class Day9 : PuzzleBase
{
    class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1.X == p2.X && p1.Y == p2.Y);
        }

        public Point Rotate()
        {
            return new Point(-Y, X);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override int GetHashCode()
        {
            return X * 193 + Y;
        }

        public override bool Equals(object? other)
        {
            if (other is null)
                return false;
            if (other is not Point otherPoint)
                return false;
            return otherPoint == this;
        }

        public int X { get; }
        public int Y { get; }
    }

    class Rope
    {
        private readonly Point[] knots;

        public Rope(IReadOnlyCollection<Point> knots)
        {
            this.knots = knots.ToArray();
        }

        public Rope Rotate(string direction)
        {
            return direction switch
            {
                "R" => this,
                "D" => Rotate(),
                "L" => Rotate().Rotate(),
                "U" => Rotate().Rotate().Rotate(),
                _ => throw new ArgumentException(null, nameof(direction))
            };
        }

        public Rope Move(string direction)
        {
            return Rotate(direction).MoveRight().Rotate(direction).Rotate(direction).Rotate(direction);
        }

        private Rope MoveRight()
        {
            var result = knots.ToList();
            result[0] = knots[0] + new Point(1, 0);
            for (var i = 1; i < knots.Length; i++)
            {
                result[i] = NormalizeTail(result[i - 1], knots[i]);
                ;
                if (result[i] == knots[i])
                    break;
            }

            return new Rope(result);
        }

        private static Point NormalizeTail(Point head, Point tail)
        {
            var v = tail - head;
            if (Math.Abs(v.X) == 2 && Math.Abs(v.Y) == 2)
                return new Point(head.X + v.X / 2, head.Y + v.Y / 2);
            if (Math.Abs(v.X) == 2)
                return new Point(head.X + v.X / 2, head.Y);
            if (Math.Abs(v.Y) == 2)
                return new Point(head.X, head.Y + v.Y / 2);
            return tail;
        }

        private Rope Rotate()
        {
            return new Rope(knots.Select(x => x.Rotate()).ToArray());
        }

        public override string ToString()
        {
            return string.Join("-", knots.Select(x => x.ToString()));
        }

        public void Print()
        {
            var points = knots.Concat(new[]{new Point(0, 0)}); 
            
            var xshift = -points.Select(x => x.X).Min();
            var yshift = -points.Select(x => x.Y).Min();
            var xsize = Math.Max(points.Select(x => x.X).Max(), 0) + xshift + 1;
            var ysize = Math.Max(points.Select(x => x.Y).Max(), 0) + yshift + 1;
            
            var result = Enumerable.Range(0, xsize).Select(_ => Enumerable.Range(0, ysize).Select(_ => ".").ToList()).ToList();
            for (var i = knots.Length-1; i >= 0; i--)
                result[knots[i].X+xshift][knots[i].Y+yshift] = i > 0 ? i.ToString() : "H";

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


        public Point Head => knots[0];
        public Point Tail => knots[^1];
    }

    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(Solve1(lines, 2));
        Console.WriteLine(Solve1(lines, 10));
    }

    private static int Solve1(IReadOnlyList<string> lines, int ropeLength)
    {
        var rope = new Rope(Enumerable.Range(0, ropeLength).Select(_ => new Point(0, 0)).ToArray());
        var visited = new HashSet<Point> { rope.Tail };
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
}