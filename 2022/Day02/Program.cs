var testPart1 = ParseInputPart1(File.ReadLines(@"../../../test.txt").ToList());
var inputPart1 = ParseInputPart1(File.ReadLines(@"../../../input.txt").ToList());
var testPart2 = ParseInputPart2(File.ReadLines(@"../../../test.txt").ToList());
var inputPart2 = ParseInputPart2(File.ReadLines(@"../../../input.txt").ToList());

Console.WriteLine($"Part 1, test answer is {Part1(testPart1)}");
Console.WriteLine($"Part 1, final answer is {Part1(inputPart1)}");
Console.WriteLine($"Part 2, test answer is {Part2(testPart2)}");
Console.WriteLine($"Part 2, final answer is {Part2(inputPart2)}");

int Part1(IEnumerable<RoundPart1> rounds)
{
    return rounds.Sum(round => round.Score);
}

int Part2(IEnumerable<RoundPart2> rounds)
{
    return rounds.Sum(round => round.Score);
}

IEnumerable<RoundPart1> ParseInputPart1(IEnumerable<string> inputToParse)
{
    return (from line in inputToParse
        select line.Split(' ')
        into plays
        let opponentShape = plays[0] switch
        {
            "A" => Shape.Rock,
            "B" => Shape.Paper,
            "C" => Shape.Scissors,
            _ => throw new Exception("Invalid shape")
        }
        let myShape = plays[1] switch
        {
            "X" => Shape.Rock,
            "Y" => Shape.Paper,
            "Z" => Shape.Scissors,
            _ => throw new Exception("Invalid shape")
        }
        select new RoundPart1(opponentShape, myShape)).ToList();
}

IEnumerable<RoundPart2> ParseInputPart2(IEnumerable<string> inputToParse)
{
    return (from line in inputToParse
        select line.Split(' ')
        into plays
        let opponentShape = plays[0] switch
        {
            "A" => Shape.Rock,
            "B" => Shape.Paper,
            "C" => Shape.Scissors,
            _ => throw new Exception("Invalid shape")
        }
        let desiredOutcome = plays[1] switch
        {
            "X" => Outcome.Lose,
            "Y" => Outcome.Draw,
            "Z" => Outcome.Win,
            _ => throw new Exception("Invalid shape")
        }
        select new RoundPart2(opponentShape, desiredOutcome)).ToList();
}

internal record RoundPart1
{
    public RoundPart1(Shape opponentShape, Shape myShape)
    {
        OpponentShape = opponentShape;
        MyShape = myShape;
    }

    private Shape OpponentShape { get; }
    private Shape MyShape { get; }

    private Outcome Outcome
    {
        get
        {
            if (MyShape == OpponentShape)
                return Outcome.Draw;
            switch (MyShape)
            {
                case Shape.Rock when OpponentShape == Shape.Scissors:
                case Shape.Paper when OpponentShape == Shape.Rock:
                case Shape.Scissors when OpponentShape == Shape.Paper:
                    return Outcome.Win;
                default:
                    return Outcome.Lose;
            }
        }
    }
    internal int Score => (int)MyShape + (int)Outcome;
}

internal record RoundPart2
{
    public RoundPart2(Shape opponentShape, Outcome desiredOutcome)
    {
        OpponentShape = opponentShape;
        DesiredOutcome = desiredOutcome;
    }

    private Shape OpponentShape { get; }
    private Outcome DesiredOutcome { get; }

    private Shape MyShape
    {
        get
        {
            return DesiredOutcome switch
            {
                Outcome.Draw => OpponentShape,
                Outcome.Win => OpponentShape switch
                {
                    Shape.Rock => Shape.Paper,
                    Shape.Paper => Shape.Scissors,
                    Shape.Scissors => Shape.Rock,
                    _ => throw new ArgumentOutOfRangeException()
                },
                Outcome.Lose => OpponentShape switch
                {
                    Shape.Rock => Shape.Scissors,
                    Shape.Paper => Shape.Rock,
                    Shape.Scissors => Shape.Paper,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    internal int Score => (int)MyShape + (int)DesiredOutcome;
}

internal enum Shape
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

internal enum Outcome
{
    Lose = 0,
    Draw = 3,
    Win = 6
}