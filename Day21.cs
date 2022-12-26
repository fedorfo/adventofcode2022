namespace adventofcode2022;

public class Day21 : PuzzleBase
{
    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(Solve1(lines));
        Console.WriteLine(Solve2(lines));
    }

    private static long Solve1(List<string> lines)
    {
        var monkeys = ReadMonkeys(lines);
        return GetValue(monkeys, "root")!.Value;
    }

    private static long Solve2(List<string> lines)
    {
        var monkeys = ReadMonkeys(lines);
        GetValue(monkeys, "root", "humn");
        monkeys["root"].Operation = '=';
        monkeys["humn"].Value = null;
        RestoreValue(monkeys, "root", "humn");
        return monkeys["humn"].Value!.Value;
    }

    private static void RestoreValue(IReadOnlyDictionary<string, Monkey> monkeys, string id, string breakPoint)
    {
        var monkey = monkeys[id];
        var leftMonkey = monkeys[monkey.Left!];
        var rightMonkey = monkeys[monkey.Right!];
        var leftValue = leftMonkey.Value;
        var rightValue = rightMonkey.Value;
        var operation = monkey.Operation!.Value;
        if (leftValue is null)
        {
            leftMonkey.Value = operation switch
            {
                '/' => monkey.Value!.Value * rightValue,
                '*' => monkey.Value!.Value / rightValue,
                '+' => monkey.Value!.Value - rightValue,
                '-' => monkey.Value!.Value + rightValue,
                '=' => rightValue,
                _ => throw new Exception($"Unsupported operation {operation}")
            };
            if (leftMonkey.Id == breakPoint)
                return;
            RestoreValue(monkeys, leftMonkey.Id, breakPoint);
        }
        else if (rightValue is null)
        {
            rightMonkey.Value = operation switch
            {
                '/' => leftValue / monkey.Value!.Value,
                '*' => monkey.Value!.Value / leftValue,
                '+' => monkey.Value!.Value - leftValue,
                '-' => leftValue - monkey.Value!.Value,
                '=' => leftValue,
                _ => throw new Exception($"Unsupported operation {operation}")
            };
            if (rightMonkey.Id == breakPoint)
                return;
            RestoreValue(monkeys, rightMonkey.Id, breakPoint);
        }
    }

    private static long? GetValue(IReadOnlyDictionary<string, Monkey> monkeys, string id, string? except = null)
    {
        if (id == except)
            return null;
        var monkey = monkeys[id];
        if (monkey.Value != null)
            return monkey.Value.Value;
        var leftValue = GetValue(monkeys, monkey.Left!, except);
        var rightValue = GetValue(monkeys, monkey.Right!, except);
        if (leftValue is null || rightValue is null)
            return null;
        var operation = monkey.Operation!.Value;
        monkey.Value = operation switch
        {
            '/' => leftValue / rightValue,
            '*' => leftValue * rightValue,
            '+' => leftValue + rightValue,
            '-' => leftValue - rightValue,
            _ => throw new Exception($"Unsupported operation {operation}")
        };
        return monkey.Value;
    }

    private static Dictionary<string, Monkey> ReadMonkeys(List<string> lines)
    {
        var monkeys = new Dictionary<string, Monkey>();
        foreach (var line in lines)
        {
            var args = line.Split(": ");
            var id = args[0];
            if (long.TryParse(args[1], out var value))
            {
                monkeys[id] = new Monkey(id, value);
            }
            else
            {
                var subArgs = args[1].Split(" ");
                monkeys[id] = new Monkey(id, null, subArgs[1][0], subArgs[0], subArgs[2]);
            }
        }

        return monkeys;
    }

    private record Monkey(string Id, long? Value, char? Operation = null, string? Left = null, string? Right = null)
    {
        public long? Value { get; set; } = Value;
        public char? Operation { get; set; } = Operation;
    }
}