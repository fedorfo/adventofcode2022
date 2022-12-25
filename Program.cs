using adventofcode2022;
using adventofcode2022.helpers;

var puzzles = new Dictionary<int, IPuzzle>();
var puzzleTypes = typeof(Program).Assembly.DefinedTypes.Where(
    x => x.ImplementedInterfaces.Contains(typeof(IPuzzle)) && !x.IsAbstract
);
foreach (var puzzleTypeInfo in puzzleTypes)
{
    var constructor = puzzleTypeInfo.GetConstructor(Array.Empty<Type>());
    var puzzle = (IPuzzle)constructor!.Invoke(Array.Empty<object>());
    puzzles.Add(puzzle.Day, puzzle);
}


//var day = int.Parse(Console.ReadLine()!);
var day = Helpers.Max(puzzles.Keys.ToArray());
using var file = File.OpenRead($"input/{puzzles[day].InputFileName}");
using var inputStream = new StreamReader(file);
Console.SetIn(inputStream);
puzzles[day].Solve();