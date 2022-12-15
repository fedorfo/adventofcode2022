namespace adventofcode2022;

public class Day13 : PuzzleBase
{
    private class SignalsComparer : IComparer<List<object>>
    {
        public int Compare(List<object>? x, List<object>? y)
        {
            return Signal.Less(x!, y!) ? -1 : Signal.Less(y!, x!) ? 1 : 0;
        }
    }

    private static class Signal
    {
        public static List<object> Parse(string line)
        {
            line = line.Replace("[", ",[,");
            line = line.Replace("]", ",],");
            return Parse(line.Split(",").Where(x => !string.IsNullOrEmpty(x)).ToList(), 1, out _);
        }

        private static List<object> Parse(List<string> tokens, int startIndex, out int endIndex)
        {
            var result = new List<object>();
            while (true)
            {
                if (tokens[startIndex] == "[")
                {
                    result.Add(Parse(tokens, startIndex + 1, out var end));
                    startIndex = end;
                }
                else if (tokens[startIndex] == "]")
                {
                    endIndex = startIndex + 1;
                    return result;
                }
                else
                {
                    result.Add(int.Parse(tokens[startIndex++]));
                }
            }
        }

        public static bool Less(List<object> l, List<object> r)
        {
            for (var i = 0; i < Math.Max(l.Count, r.Count); i++)
            {
                if (l.Count <= i)
                    return true;
                if (r.Count <= i)
                    return false;

                if (l[i] is int && r[i] is int)
                {
                    if ((int)l[i] < (int)r[i])
                        return true;
                    if ((int)l[i] > (int)r[i])
                        return false;
                }
                else
                {
                    var lTemp = l[i] is int ? new object[] { (int)l[i] }.ToList() : (List<object>)l[i];
                    var rTemp = r[i] is int ? new object[] { (int)r[i] }.ToList() : (List<object>)r[i];
                    if (Less(lTemp, rTemp))
                        return true;
                    if (Less(rTemp, lTemp))
                        return false;
                }
            }

            return false;
        }
    }

    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(Solve1(lines));
        Console.WriteLine(Solve2(lines));
    }

    private int Solve1(List<string> lines)
    {
        var result = 0;
        var counter = 1;
        for (var i = 0; i < lines.Count; i += 3)
        {
            var l = Signal.Parse(lines[i]);
            var r = Signal.Parse(lines[i + 1]);
            if (Signal.Less(l, r))
                result += counter;
            counter++;
        }

        return result;
    }

    private int Solve2(List<string> lines)
    {
        var signals = new List<List<object>>();
        for (var i = 0; i < lines.Count; i += 3)
        {
            signals.Add(Signal.Parse(lines[i]));
            signals.Add(Signal.Parse(lines[i + 1]));
        }

        var separator1 = new object[] { new object[] { 2 }.ToList() }.ToList();
        var separator2 = new object[] { new object[] { 6 }.ToList() }.ToList();

        signals.Add(separator1);
        signals.Add(separator2);

        var comparer = new SignalsComparer();
        signals.Sort(comparer);
        var index1 = signals.Select((x, i) => (x, i)).Single(x => comparer.Compare(separator1, x.x) == 0).i;
        var index2 = signals.Select((x, i) => (x, i)).Single(x => comparer.Compare(separator2, x.x) == 0).i;
        return (index1 + 1) * (index2 + 1);
    }
}