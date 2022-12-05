var testPart1 = ParseInputPart1(File.ReadLines(@"../../../test.txt").ToList());
var inputPart1 = ParseInputPart1(File.ReadLines(@"../../../input.txt").ToList());

Console.WriteLine($"Part 1, test answer is {Part1(testPart1)}");
Console.WriteLine($"Part 1, final answer is {Part1(inputPart1)}");

int Part1(IEnumerable<Rucksack> rucksacks)
{
    return rucksacks.Sum(r => r.Priority);
}

IEnumerable<Rucksack> ParseInputPart1(IEnumerable<string> inputToParse)
{
    return inputToParse.Select(line => new Rucksack(line));
}

internal record Rucksack
{
    internal Rucksack(string line)
    {
        Compartment1 = line[..(line.Length / 2)].ToCharArray();
        Compartment2 = line[(line.Length / 2)..].ToCharArray();
    }

    private char[] Compartment1 { get; }
    private char[] Compartment2 { get; }

    private char CommonItem => Compartment1.Intersect(Compartment2).Single();

    internal int Priority => CommonItem switch
    {
        >= 'A' and <= 'Z' => CommonItem - 'A' + 27,
        >= 'a' and <= 'z' => CommonItem - 'a' + 1,
        _ => throw new ArgumentOutOfRangeException()
    };
}