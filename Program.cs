using adventofcode2022;

const int day = 2;
var puzzles = new Dictionary<int, IPuzzle>
{
    {1, new Day1()},
    {2, new Day2()},
};

using var file = File.OpenRead($"input/input{day}.txt");
using var inputStream = new StreamReader(file);
    Console.SetIn(inputStream);
puzzles[day].Solve();