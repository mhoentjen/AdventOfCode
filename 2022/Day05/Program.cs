var test = ParseInput(File.ReadLines(@"../../../test.txt"));
var input = ParseInput(File.ReadLines(@"../../../input.txt"));

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, answer is {Part1(input)}");

test = ParseInput(File.ReadLines(@"../../../test.txt"));
input = ParseInput(File.ReadLines(@"../../../input.txt"));
Console.WriteLine($"Part 2, test answer is {Part2(test)}");
Console.WriteLine($"Part 2, answer is {Part2(input)}");

string Part1((StartingStacks startingStacks, Procedure procedure) inputPart1)
{
    var stacks = inputPart1.startingStacks.Stacks;
    var procedure = inputPart1.procedure.Steps;
    foreach (var step in procedure)
    {
        for (var i = 0; i < step.Number; i++)
        {
            MoveCrate(stacks, step.From, step.To);
        }
    }
    return new string(stacks.Select(s => s.Crates.Last()).ToArray());
}

string Part2((StartingStacks startingStacks, Procedure procedure) inputPart2)
{
    var stacks = inputPart2.startingStacks.Stacks;
    var procedure = inputPart2.procedure.Steps;
    foreach (var step in procedure)
    {
        MoveCrates(stacks, step.Number, step.From, step.To);
    }
    return new string(stacks.Select(s => s.Crates.Last()).ToArray());
}

(StartingStacks, Procedure) ParseInput(IEnumerable<string> inputToParse)
{
    var startingStacksInput = new List<string>();
    var procedureInput = new List<string>();
    var isStartingStacks = true;
    foreach (var line in inputToParse)
    {
        if (line == "")
        {
            isStartingStacks = false;
            continue;
        }

        if (isStartingStacks)
        {
            startingStacksInput.Add(line);
        }
        else
        {
            procedureInput.Add(line);
        }
    }
    return (new StartingStacks(startingStacksInput), new Procedure(procedureInput));
}

void MoveCrate(IReadOnlyCollection<Stack> stacks, int from, int to)
{
    var fromStack = stacks.Single(s => s.Number == from);
    var toStack = stacks.Single(s => s.Number == to);
    var crate = fromStack.Crates.Last();
    fromStack.Crates.RemoveAt(fromStack.Crates.Count - 1);
    toStack.Crates.Add(crate);
}

void MoveCrates(IReadOnlyCollection<Stack> stacks, int number, int from, int to)
{
    var fromStack = stacks.Single(s => s.Number == from);
    var toStack = stacks.Single(s => s.Number == to);
    var crates = fromStack.Crates.TakeLast(number).ToList();
    fromStack.Crates.RemoveRange(fromStack.Crates.Count - number, number);
    toStack.Crates.AddRange(crates);
}

internal record StartingStacks
{
    internal StartingStacks(List<string> input)
    {
        var lastLine = input.Last().ToCharArray();
        for (var index = 0; index < lastLine.Length; index++)
        {
            var c = lastLine[index];
            if (int.TryParse(c.ToString(), out var digit))
            {
                Stacks.Add(new Stack(digit, index));
            }
        }
        
        var otherLines = input.GetRange(0, input.Count - 1);
        otherLines.Reverse();
        foreach (var line in otherLines)
        {
            foreach (var stack in Stacks.Where(stack => line[stack.XPos] != ' '))
            {
                stack.Crates.Add(line[stack.XPos]);
            }
        }
    }

    internal readonly List<Stack> Stacks = new();
}

internal record Procedure
{
    internal Procedure(IEnumerable<string> input)
    {
        Steps = input.Select(line => new Step(line)).ToList();
    }
    
    internal readonly List<Step> Steps = new();
}

internal record Stack(int Number, int XPos)
{
    internal List<char> Crates { get; } = new();
}

internal record Step
{
    internal Step(string line)
    {
        var split = line.Split(' ');
        Number = int.Parse(split[1]);
        From = int.Parse(split[3]);
        To = int.Parse(split[5]);
    }
    internal int Number { get; }
    internal int From { get; }
    internal int To { get; }
}