using System.Text.RegularExpressions;

namespace adventofcode2022;

public class Day5 : PuzzleBase
{
    class Movement
    {
        public Movement(int count, int from, int to)
        {
            Count = count;
            From = from;
            To = to;
        }

        public int Count { get; }
        public int From { get; }
        public int To { get; }
    }

    public override void Solve()
    {
        var (stacks, movements) = ReadInput();
        Solve1(CopyStacks(stacks), movements);
        Solve2(CopyStacks(stacks), movements);
    }

    private List<Stack<char>> CopyStacks(List<Stack<char>> stacks)
    {
        return stacks.Select(x => x.Reverse().Aggregate(new Stack<char>(),
            (stack, crate) =>
            {
                stack.Push(crate);
                return stack;
            })).ToList();
    }

    private void Solve1(List<Stack<char>> stacks, IReadOnlyCollection<Movement> movements)
    {
        foreach (var movement in movements)
            Move(stacks[movement.From], stacks[movement.To], movement.Count);
        Console.WriteLine(stacks.Select(x => x.Peek()).Aggregate("", (res, crate) => res + crate));
    }
    
    private void Solve2(List<Stack<char>> stacks, IReadOnlyCollection<Movement> movements)
    {
        foreach (var movement in movements)
            MoveSameOrdering(stacks[movement.From], stacks[movement.To], movement.Count);
        Console.WriteLine(stacks.Select(x => x.Peek()).Aggregate("", (res, crate) => res + crate));
    }

    private void Move(Stack<char> from, Stack<char> to, int count)
    {
        for (var i = 0; i < count; i++)
            to.Push(from.Pop());
    }
    
    private void MoveSameOrdering(Stack<char> from, Stack<char> to, int count)
    {
        var buffer = new Stack<char>();
        for (var i = 0; i < count; i++)
            buffer.Push(from.Pop());
        for (var i = 0; i < count; i++)
            to.Push(buffer.Pop());
    }

    private (List<Stack<char>>, List<Movement>) ReadInput()
    {
        var lines = ReadLines();
        var stacks = Enumerable.Range(0, lines[0].Length / 4 + 1).Select(_ => new Stack<char>()).ToList();

        var stacksInputEnd = lines.Select((x, i) => Tuple.Create(x == "", i)).First(x => x.Item1).Item2 - 1;
        for (var i = stacksInputEnd - 1; i >= 0; i--)
        {
            var line = lines[i];
            for (var j = 0; j < stacks.Count; j++)
            {
                if (line[j * 4 + 1] != ' ')
                    stacks[j].Push(line[j * 4 + 1]);
            }
        }

        var regex = new Regex(@"move (\d+) from (\d+) to (\d+)", RegexOptions.Compiled);
        var movements = new List<Movement>();
        for (var i = stacksInputEnd + 2; i < lines.Count; i++)
        {
            var groups = regex.Match(lines[i]).Groups;
            movements.Add(
                new Movement(int.Parse(groups[1].Value), int.Parse(groups[2].Value) - 1, int.Parse(groups[3].Value) - 1)
            );
        }

        return (stacks, movements);
    }
}