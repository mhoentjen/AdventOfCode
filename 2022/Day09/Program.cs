Dictionary<string, (int, int)> directions = new()
{
    {"U", (0, 1)},
    {"L", (-1, 0)},
    {"R", (1, 0)},
    {"D", (0, -1)}
};
var test = File.ReadLines(@"../../../test.txt").ToList();
var test2 = File.ReadLines(@"../../../test2.txt").ToList();
var testInstructions = ParseInput(test).ToList();
var test2Instructions = ParseInput(test2).ToList();
var input = File.ReadLines(@"../../../input.txt").ToList();
var inputInstructions = ParseInput(input).ToList();

// Console.WriteLine($"Part 1, test answer is {Part1(testInstructions)}");
// Console.WriteLine($"Part 1, answer is {Run(inputInstructions, 2)}");
// Console.WriteLine($"Part 2, test answer is {Run(test2Instructions, 10)}");
Console.WriteLine($"Part 2, answer is {Run(inputInstructions, 10)}");

int Run(List<Instruction> instructions, int elementCount)
{
    var grid = new List<Position> { new(0, 0) };
    var positions = new Position[elementCount];
    for (var index = 0; index < elementCount; index++)
    {
        positions[index] = grid.Single(p => p.X == 0 && p.Y == 0);
    }

    positions[elementCount - 1].Visited = true;
    // PrintGrid(grid, positions[0], positions[1..(elementCount)].ToList() );
    foreach (var instruction in instructions)
    {
        foreach (var _ in Enumerable.Range(0, instruction.Distance))
        {
            positions[0] = MoveHead(grid, positions[0], instruction.Direction);
            for (var index = 1; index <= elementCount - 1; index++)
            {
                positions[index] = Follow(grid, positions[index - 1], positions[index], index == elementCount - 1);
            }
        }
        // PrintGrid(grid, positions[0], positions[1..(elementCount)].ToList() );
    }

    return grid.Count(p => p.Visited);
}

Position MoveHead(List<Position> grid, Position headPosition, (int X, int Y) direction)
{
    var newX = headPosition.X + direction.X;
    var newY = headPosition.Y + direction.Y;
    if (grid.All(p => p.X != newX)) AddColumn(grid, direction.X);
    if (grid.All(p => p.Y != newY)) AddRow(grid, direction.Y);
    return grid.Single(p => p.X == newX && p.Y == newY);
}

Position Follow(List<Position> grid, Position leader, Position follower, bool isTail)
{
    var distanceX = leader.X - follower.X;
    var distanceY = leader.Y - follower.Y;
    var newX = follower.X;
    var newY = follower.Y;
    if (Math.Abs(distanceX) > 1 && distanceY == 0)
    {
        newX = follower.X + Math.Sign(distanceX);
    }
    else
    {
        if (distanceX == 0 && Math.Abs(distanceY) > 1)
        {
            newY = follower.Y + Math.Sign(distanceY);
        }
        else
        {
            if (Math.Abs(distanceX) >= 1 && Math.Abs(distanceY) > 1)
            {
                newX = follower.X + Math.Sign(distanceX);
                newY = follower.Y + Math.Sign(distanceY);
            }
            else if (Math.Abs(distanceX) > 1 && Math.Abs(distanceY) >= 1)
            {
                newX = follower.X + Math.Sign(distanceX);
                newY = follower.Y + Math.Sign(distanceY);
            }
        }
    }

    follower = grid.Single(p => p.X == newX && p.Y == newY);
    if (isTail) follower.Visited = true;
    return follower;
}

IEnumerable<Instruction> ParseInput(IEnumerable<string> lines)
{
    foreach (var parts in lines.Select(line => line.Split(' ')))
    {
        directions.TryGetValue(parts[0], out var direction);
        yield return new Instruction(direction, int.Parse(parts[1]));
    }
}

void AddColumn(List<Position> gridToGrow, int direction)
{
    var currentMinX = gridToGrow.Min(p => p.X);
    var currentMaxX = gridToGrow.Max(p => p.X);
    var currentMinY = gridToGrow.Min(p => p.Y);
    var currentMaxY = gridToGrow.Max(p => p.Y);
    if (direction == -1)
    {
        for (var y = currentMaxY; y >= currentMinY; y--)
        {
            gridToGrow.Add(new Position(currentMinX - 1, y));
        }
    }
    else
    {
        for (var y = currentMinY; y <= currentMaxY; y++)
        {
            gridToGrow.Add(new Position(currentMaxX + 1, y));
        }
    }
}

void AddRow(List<Position> gridToGrow, int direction)
{
    var currentMinX = gridToGrow.Min(p => p.X);
    var currentMaxX = gridToGrow.Max(p => p.X);
    var currentMinY = gridToGrow.Min(p => p.Y);
    var currentMaxY = gridToGrow.Max(p => p.Y);
    if (direction == -1)
    {
        for (var x = currentMaxX; x >= currentMinX; x--)
        {
            gridToGrow.Add(new Position(x, currentMinY - 1));
        }
    }
    else
    {
        for (var x = currentMinX; x <= currentMaxX; x++)
        {
            gridToGrow.Add(new Position(x, currentMaxY + 1));
        }
    }
}

void PrintGrid(List<Position> grid, Position headPosition, List<Position> otherPositions)
{
    var minY = grid.Min(p => p.Y);
    var maxY = grid.Max(p => p.Y);
    foreach (var y in Enumerable.Range(minY, maxY - minY + 1).Reverse())
    {
        var minX = grid.Min(p => p.X);
        var maxX = grid.Max(p => p.X);
        foreach (var x in Enumerable.Range(minX, maxX - minX + 1))
        {
            var position = grid.Single(p => p.X == x && p.Y == y);
            var containsElement = false;

            if (position.X == headPosition.X && position.Y == headPosition.Y)
            {
                Console.Write("H");
                containsElement = true;
            }
            else
            {
                for (var index = 0; index < otherPositions.Count; index++)
                {
                    var otherPosition = otherPositions[index];
                    if (otherPosition.X == x && otherPosition.Y == y)
                    {
                        Console.Write(index + 1);
                        containsElement = true;
                        break;
                    }
                }
            }
            if (!containsElement) Console.Write(position.Visited ? "#" : ".");
        }
        Console.WriteLine();
    }

    Console.WriteLine();
}

internal record Instruction((int, int) Direction, int Distance);

internal record Position(int X, int Y)
{
    internal bool Visited { get; set; }
}