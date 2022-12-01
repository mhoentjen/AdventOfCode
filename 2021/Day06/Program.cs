// Trying new console template introduced with .NET 6...
// See https://aka.ms/new-console-template for more information

using Day06;

var inputPart1 = ParseInputPart1(File.ReadLines(@"../../../input.txt").ToList()[0]);
var testPart1 = ParseInputPart1(File.ReadLines(@"../../../test.txt").ToList()[0]);
var inputPart2 = ParseInputPart2(File.ReadLines(@"../../../input.txt").ToList()[0]);
var testPart2 = ParseInputPart2(File.ReadLines(@"../../../test.txt").ToList()[0]);

Console.WriteLine($"Part 1, test answer is {Part1(testPart1)}");
Console.WriteLine($"Part 1, final answer is {Part1(inputPart1)}");
Console.WriteLine($"Part 2, test answer is {Part2(testPart2)}");
Console.WriteLine($"Part 2, final answer is {Part2(inputPart2)}");

List<Fish> ParseInputPart1(string inputToParse)
{
    var inputFish = inputToParse.Split(',');
    return inputFish.Select(item => new Fish() {Counter=int.Parse(item)}).ToList();
}

List<int> ParseInputPart2(string inputToParse)
{
    var inputFish = inputToParse.Split(',');
    return inputFish.Select(int.Parse).ToList();
}

int Part1(List<Fish> input)
{
    List<Fish> fishTracker = input;
    for (var i = 0; i < 80; i++)
    {
        fishTracker = AdvanceOneDay(fishTracker);
    }

    return fishTracker.Count;
}

long Part2(List<int> input)
{
    // Make initial list of fish grouped by their timer value:
    var fishTracker = new long[9];
    for (var i = 0; i < fishTracker.Length; i++)
    {
        fishTracker[i] = input.Count(fish => fish == i);
    }

    for (var day = 1; day <= 256; day++)
    {
        // When a day passes, all fish in group 0 produce extra fish and move to group 6
        var newFish = fishTracker[0];

        // timer values 1 thru 8 move to next group:
        for (var group = 0; group < 8; group++)
        {
            fishTracker[group] = fishTracker[group + 1];
        }
        
        // New fish are added to group 8:
        fishTracker[8] = newFish;
        
        // Fish that start a new cycle are added to group 6:
        fishTracker[6] += newFish;
    }
    return fishTracker.Sum();
}



List<Fish> AdvanceOneDay(List<Fish> fishToAdvance)
{
    var fish = fishToAdvance;
    for (var i = 0; i < fishToAdvance.Count; i++)
    {
        if (fish[i].Counter == 0)
        {
            fish[i].Counter = 6;
            fish.Add(new Fish(){Counter = 9});
        }
        else
        {
            fish[i].Counter--;
        }
    }
    return fish;
}