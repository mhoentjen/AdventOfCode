var test = File.ReadLines(@"../../../test.txt").ToList().Select(line => new Instruction(line)).ToList();
var input = File.ReadLines(@"../../../input.txt").ToList().Select(line => new Instruction(line)).ToList();

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, answer is {Part1(input)}");
Console.WriteLine();
Part2(test);
Console.WriteLine();
Part2(input);

int Part1(IEnumerable<Instruction> instructions)
{
    var cycles = Run(instructions);
    return cycles.Where(c => c.CyclesRun is 20 or 60 or 100 or 140 or 180 or 220).Sum(c => c.SignalStrength);
}

void Part2(IEnumerable<Instruction> instructions)
{
    var cycles = Run(instructions);
    foreach (var cycle in cycles)
    {
        Console.Write(cycle.PixelLit ? "#" : ".");
        if (cycle.CyclesRun % 40 == 0)
            Console.WriteLine();
    }
}

IEnumerable<Cycle> Run(IEnumerable<Instruction> instructions)
{
    var cycles = new List<Cycle>();
    var cycle = 0;
    var X = 1;
    
    foreach (var instruction in instructions)
    {
        switch (instruction.Operation)
        {
            case "noop":
                cycle++;
                cycles.Add(new Cycle(cycle, X));
                break;
            case "addx":
                cycle++;
                cycles.Add(new Cycle(cycle, X));
                cycle++;
                cycles.Add(new Cycle(cycle, X));
                X += instruction.Argument;
                break;
        }
    }
    return cycles;
}

internal record Cycle(int CyclesRun, int X)
{
    internal int SignalStrength => CyclesRun * X;
    internal bool PixelLit => X - 1 == (CyclesRun - 1) % 40 ||
                              X == (CyclesRun - 1) % 40 ||
                              X + 1 == (CyclesRun - 1) % 40;
}

internal record Instruction
{
    internal Instruction(string line)
    {
        var parts = line.Split(' ');
        Operation = parts[0];
        if (Operation == "addx")
        {
            Argument = int.Parse(parts[1]);
        }
    }
    internal string Operation { get; }
    internal int Argument { get; }
}