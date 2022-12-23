using adventofcode2022;

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
const int day = 18;
using var file = File.OpenRead($"input/{puzzles[day].InputFileName}");
using var inputStream = new StreamReader(file);
Console.SetIn(inputStream);
puzzles[day].Solve();