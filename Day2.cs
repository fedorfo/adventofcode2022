namespace adventofcode2022;

public class Day2 : PuzzleBase
{
    public override void Solve()
    {
        var lines = ReadLines();
        Console.WriteLine(GetScore(lines));
        Console.WriteLine(GetScore2(lines));
    }

    private enum Shape{
        Rock,
        Paper,
        Scissors
    }
    
    private enum RoundResult{
        Win,
        Draw,
        Lose,
    }

    private static readonly Dictionary<Shape, int> ShapeToScore = new()
    {
        { Shape.Rock, 1 }, { Shape.Paper, 2 }, { Shape.Scissors, 3 },
    };
    
    private static readonly Dictionary<RoundResult, int> RoundResultToScore = new()
    {
        { RoundResult.Win, 6 }, { RoundResult.Draw, 3 }, { RoundResult.Lose, 0 },
    };
    
    private static readonly Dictionary<string, RoundResult> EncodedRoundResultToRoundResult = new()
    {
        { "X", RoundResult.Lose}, { "Y", RoundResult.Draw}, { "Z", RoundResult.Win},
    };

    private static RoundResult GetRoundResult(Shape your, Shape opponents)
    {
        if (your == opponents)
            return RoundResult.Draw;
        if (your == Shape.Paper && opponents == Shape.Rock)
            return RoundResult.Win;
        if (your == Shape.Rock && opponents == Shape.Scissors)
            return RoundResult.Win;
        if (your == Shape.Scissors && opponents == Shape.Paper)
            return RoundResult.Win;
        return RoundResult.Lose;
    }
    
    private static Shape GetYourShape(Shape opponents, RoundResult roundResult)
    {
        foreach (var yourShape in (Shape[])Enum.GetValues(typeof(Shape)))
        {
            if (GetRoundResult(yourShape, opponents) == roundResult)
                return yourShape;
        }

        throw new Exception($"Can not find your shape for opponents {opponents} and roundResult {roundResult}");
    }
    

    private static Shape GetShape(string encodedShape)
    {
        if (new[] { "A", "X" }.Contains(encodedShape))
            return Shape.Rock;
        if (new[] { "B", "Y" }.Contains(encodedShape))
            return Shape.Paper;
        if (new[] { "C", "Z" }.Contains(encodedShape))
            return Shape.Scissors;
        throw new ArgumentException(encodedShape);
    }
    
    private static int GetScore(List<string> lines)
    {
        var score = 0;
        foreach (var line in lines)
        {
            var opponentsShape = GetShape(line[0].ToString());
            var yourShape = GetShape(line[2].ToString());
            score += ShapeToScore[yourShape] + RoundResultToScore[GetRoundResult(yourShape, opponentsShape)];
        }
        return score;
    }
    
    private static int GetScore2(List<string> lines)
    {
        var score = 0;
        foreach (var line in lines)
        {
            var opponentsShape = GetShape(line[0].ToString());
            var roundResult = EncodedRoundResultToRoundResult[line[2].ToString()];
            var yourShape = GetYourShape(opponentsShape, roundResult);
            score += ShapeToScore[yourShape] + RoundResultToScore[roundResult];
        }
        return score;
    }
}