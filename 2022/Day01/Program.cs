// See https://aka.ms/new-console-template for more information

var input = ParseInput(File.ReadLines(@"../../../input.txt").ToList());

Console.WriteLine($"Part 1, final answer is {Part1(input)}");
Console.WriteLine($"Part 2, final answer is {Part2(input)}");


List<Elf> ParseInput(IReadOnlyList<string> inputToParse)
{
    var elves = new List<Elf> { new() };
    foreach (var line in inputToParse)
    {
        if (string.IsNullOrEmpty(line))
        {
            elves.Add(new Elf());
            continue;
        }
        elves.Last().Food.Add(int.Parse(line));
    }
    return elves;
}

int Part1(IEnumerable<Elf> elves)
{
    var orderedElves = elves.OrderByDescending(elf => elf.Food.Sum());
    return orderedElves.First().Food.Sum();
}

int Part2(IEnumerable<Elf> elves)
{
    var orderedElves = elves.OrderByDescending(elf => elf.Food.Sum());
    return orderedElves.Take(3).Sum(elf => elf.Food.Sum());
}

internal record Elf
{
    internal List<int> Food { get; } = new();
}