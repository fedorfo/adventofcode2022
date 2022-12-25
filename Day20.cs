namespace adventofcode2022;

public class Day20 : PuzzleBase
{
    public override void Solve()
    {
        var numbers = ReadLines().Select(long.Parse).ToList();
        Console.WriteLine(Solve1(numbers));
        Console.WriteLine(Solve2(numbers));
    }

    private static long Solve1(List<long> numbers)
    {
        var list = numbers.Select((x, i) => (index: i, value: x)).ToList();
        MixList(list);
        return GetAnswer(list);
    }
    
    private static long Solve2(List<long> numbers)
    {
        var list = numbers.Select((x, i) => (index: i, value: x * 811589153)).ToList();
        for (var i = 0; i < 10; i++)
            MixList(list);
        return GetAnswer(list);
    }

    private static long GetAnswer(List<(int index, long value)> list)
    {
        var zeroIndex = list.FindIndex(x => x.value == 0);
        var shifts = new[] { 1000, 2000, 3000 };
        var values = shifts.Select(x => list[(zeroIndex + x) % list.Count].value).ToList();
        return values.Sum();
    }

    private static void MixList(List<(int index, long value)> list)
    {
        var length = list.Count;
        for (var i = 0; i < length; i++)
        {
            var index = list.FindIndex(x => x.index == i);
            var item = list[index];
            list.RemoveAt(index);
            index = (int)((index + item.value) % list.Count);
            if (index < 0) index += list.Count;
            list.Insert(index, item);
        }
    }
}