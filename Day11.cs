namespace adventofcode2022;

public class Day11 : PuzzleBase
{
    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(Solve(lines, 3, 20));
        Console.WriteLine(Solve(lines, 1, 10000));
    }

    private long Solve(List<string> lines, long relief, int rounds)
    {
        var (operations, tests, items) = ReadMoneys(lines);
        var inspections = Enumerable.Range(0, items.Count).Select(_ => (long)0).ToList();

        for (var round = 1; round <= rounds; round++)
        {
            for (var i = 0; i < items.Count; i++)
            {
                inspections[i] += items[i].Count;
                var newItems = items[i].Select(x => operations[i](x) / relief).ToList();
                items[i] = new List<long>();
                foreach (var item in newItems)
                    items[tests[i](item)].Add(item);
            }
        }

        inspections = inspections.OrderByDescending(x => x).ToList();
        return inspections[0] * inspections[1];
    }

    private (List<Func<long, long>>, List<Func<long, int>>, List<List<long>>) ReadMoneys(
        List<string> lines)
    {
        long commonDivisor = 1;
        var operations = new List<Func<long, long>>();
        var tests = new List<Func<long, int>>();
        var items = new List<List<long>>();

        var i = 0;
        while (i < lines.Count)
        {
            var divisor = long.Parse(lines[i + 3].Replace("  Test: divisible by ", ""));
            commonDivisor *= divisor;
            i += 7;
        }

        i = 0;
        while (i < lines.Count)
        {
            items.Add(lines[i + 1].Replace("  Starting items: ", "").Split(", ").Select(long.Parse).ToList());

            var operationArgs = lines[i + 2].Replace("  Operation: new = old ", "").Split(" ");
            if (operationArgs[0] == "+" && operationArgs[1] != "old")
            {
                var operationArg = long.Parse(operationArgs[1]);
                operations.Add(x => (x + operationArg) % commonDivisor);
            }
            else if (operationArgs[0] == "+")
                operations.Add(x => (x + x) % commonDivisor);
            else if (operationArgs[0] == "*" && operationArgs[1] != "old")
            {
                var operationArg = long.Parse(operationArgs[1]);
                operations.Add(x => x * operationArg % commonDivisor);
            }
            else if (operationArgs[0] == "*")
                operations.Add(x => x * x % commonDivisor);

            var divisor = long.Parse(lines[i + 3].Replace("  Test: divisible by ", ""));
            var idx1 = int.Parse(lines[i + 4].Replace("    If true: throw to monkey ", ""));
            var idx2 = int.Parse(lines[i + 5].Replace("    If false: throw to monkey ", ""));
            tests.Add(x =>
            {
                var rem = x % divisor;
                return rem == 0 ? idx1 : idx2;
            });
            i += 7;
        }

        return (operations, tests, items);
    }
}