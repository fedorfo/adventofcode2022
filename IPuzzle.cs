namespace adventofcode2022;

public interface IPuzzle
{
    void Solve();
    int Day { get; }
    string InputFileName { get; }
}