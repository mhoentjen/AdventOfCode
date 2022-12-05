var test = ParseInput(File.ReadLines(@"../../../test.txt")).ToList();
var input = ParseInput(File.ReadLines(@"../../../input.txt")).ToList();

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, final answer is {Part1(input)}");
Console.WriteLine($"Part 2, test answer is {Part2(test)}");
Console.WriteLine($"Part 2, final answer is {Part2(input)}");

int Part1(IEnumerable<Pair> pairs)
{
    return pairs.Count(p => p.Elf1.Contains(p.Elf2) || p.Elf2.Contains(p.Elf1));
}

int Part2(IEnumerable<Pair> pairs)
{
    return pairs.Count(p => p.Elf1.Overlaps(p.Elf2));
}

IEnumerable<Pair> ParseInput(IEnumerable<string> inputToParse)
{
    return inputToParse.Select(line => new Pair(line));
}

internal record Pair
{
    internal Pair(string line)
    {
        var elves = line.Split(',');
        Elf1 = new Assignment(elves[0]);
        Elf2 = new Assignment(elves[1]);
    }

    internal Assignment Elf1 { get; }
    internal Assignment Elf2 { get; }
}

internal record Assignment
{
    internal Assignment(string elf)
    {
        var limits = elf.Split('-');
        Min = int.Parse(limits[0]);
        Max = int.Parse(limits[1]);
    }

    private int Min { get; }
    private int Max { get; }

    internal bool Contains(Assignment other)
    {
        return Min <= other.Min && Max >= other.Max;
    }
    
    internal bool Overlaps(Assignment other)
    {
        return Min <= other.Max && Max >= other.Min;
    }
}