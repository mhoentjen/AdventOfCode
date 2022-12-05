var test = ParseInput(File.ReadLines(@"../../../test.txt")).ToList();
var input = ParseInput(File.ReadLines(@"../../../input.txt")).ToList();

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, final answer is {Part1(input)}");
Console.WriteLine($"Part 2, test answer is {Part2(test)}");
Console.WriteLine($"Part 2, final answer is {Part2(input)}");

int Part1(IEnumerable<Rucksack> rucksacks)
{
    return rucksacks.Sum(r => GetPriority(r.CommonItem));
}

int Part2(IEnumerable<Rucksack> rucksacks)
{
    // Make groups of three items:
    var groups = rucksacks
        .Select((item, index) => new { Item = item, Group = index / 3 })
        .GroupBy(r => r.Group)
        .Select(g => g.Select(r => r.Item).ToList());
    // Determine common items
    var badges = groups.Select(group => group[0].Contents.Intersect(group[1].Contents).Intersect(group[2].Contents).Single()).ToList();
    return badges.Select(GetPriority).Sum(p => p);
}

IEnumerable<Rucksack> ParseInput(IEnumerable<string> inputToParse)
{
    return inputToParse.Select(line => new Rucksack(line));
}

int GetPriority(char item)
{
    return item switch
    {
        >= 'A' and <= 'Z' => item - 'A' + 27,
        >= 'a' and <= 'z' => item - 'a' + 1,
        _ => throw new ArgumentOutOfRangeException()
    };
}

internal record Rucksack
{
    internal Rucksack(string line)
    {
        Contents = line.ToCharArray();
        Compartment1 = line[..(line.Length / 2)].ToCharArray();
        Compartment2 = line[(line.Length / 2)..].ToCharArray();
    }

    internal char[] Contents { get; }
    private char[] Compartment1 { get; }
    private char[] Compartment2 { get; }

    internal char CommonItem => Compartment1.Intersect(Compartment2).Single();
}