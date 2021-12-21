// See https://aka.ms/new-console-template for more information

var input = ParseInput(File.ReadLines(@"..\..\..\input.txt").ToList()[0]);
var test = ParseInput(File.ReadLines(@"..\..\..\test.txt").ToList()[0]);

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, final answer is {Part1(input)}");
Console.WriteLine($"Part 2, test answer is {Part2(test)}");
Console.WriteLine($"Part 2, final answer is {Part2(input)}");

List<int> ParseInput(string inputToParse)
{
    var parsedInput = inputToParse.Split(',');
    return parsedInput.Select(int.Parse).ToList();
}

int Part1(IReadOnlyCollection<int> crabPositions)
{
    var fuelConsumptions = Enumerable.Range(crabPositions.Min(), crabPositions.Max())
        .Select(alignPosition => crabPositions
            .Sum(currentPosition => Math.Abs(currentPosition - alignPosition))).ToList();

    return fuelConsumptions.Min();
}

int Part2(List<int> crabPositions)
{
    var fuelConsumptions = new List<int>();
    foreach (var alignPosition in Enumerable.Range(crabPositions.Min(), crabPositions.Max()))
    {
        var sum = 0;
        foreach (var currentPosition in crabPositions)
        {
            double distance = Math.Abs((currentPosition - alignPosition));
            sum += (int) (distance * ((1 + distance) / 2));
        }

        fuelConsumptions.Add(sum);
    }

    return fuelConsumptions.Min();
}