using Day11;

var input = ParseInput(File.ReadLines(@"..\..\..\input.txt").ToList());
var test = ParseInput(File.ReadLines(@"..\..\..\test.txt").ToList());

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, final answer is {Part1(input)}");
Console.WriteLine($"Part 2, test answer is {Part2(test)}");
Console.WriteLine($"Part 2, final answer is {Part2(input)}");

List<Point> ParseInput(IReadOnlyList<string> inputToParse)
{
    var parsedInput = new List<Point>();
    for (var i = 0; i < inputToParse.Count; i++)
    {
        var line = inputToParse[i];
        for (var j = 0; j < line.Length; j++)
        {
            var item = line[j].ToString();
            var point = new Point()
            {
                EnergyLevel = int.Parse(item),
                X = j,
                Y = i
            };
            parsedInput.Add(point);
        }
    }
    return parsedInput;
}

void DrawGrid(List<Point> grid, string comment)
{
    Console.WriteLine(comment);
    for (var line = 0; line < 10; line++)
    {
        var lineString = grid.Where(p => p.Y == line).Aggregate("", (current, point) => current + point.EnergyLevel);
        Console.WriteLine(lineString);
    }
    
}

int Part1(List<Point> grid)
{
    var countFlashes = 0;

    DrawGrid(grid, "Before any steps:");
    
    // Perform 100 steps:
    for (var step = 1; step <= 100; step++)
    {
        foreach (var point in grid)
        {
            var XYMax = 9;
            // Determine surrounding points for each point:
            point.SurroundingPoints = new List<(int X, int Y)>();
            // Above left:
            if (point.X > 0 && point.Y > 0) point.SurroundingPoints.Add((point.X - 1, point.Y - 1));
            // Above:
            if (point.Y > 0) point.SurroundingPoints.Add((point.X, point.Y - 1));
            // Above right:
            if (point.X < XYMax && point.Y > 0) point.SurroundingPoints.Add((point.X + 1, point.Y - 1));
            // Left:
            if (point.X > 0) point.SurroundingPoints.Add((point.X - 1, point.Y));
            // Right:
            if (point.X < XYMax) point.SurroundingPoints.Add((point.X + 1, point.Y));
            // Below left:
            if (point.X > 0 && point.Y < XYMax) point.SurroundingPoints.Add((point.X - 1, point.Y + 1));
            // Below:
            if (point.Y < XYMax) point.SurroundingPoints.Add((point.X, point.Y + 1));
            // Below right:
            if (point.X < XYMax && point.Y < XYMax) point.SurroundingPoints.Add((point.X + 1, point.Y + 1));

            // First, the energy level of each octopus increases by 1:
            point.EnergyLevel++;

            // Then, any octopus with an energy level greater than 9 flashes.
            if (point.EnergyLevel > 9)
            {
                point.ToFlash = true;
            }
        }

        while (grid.Any(p => p.ToFlash && p.HasFlashed == false))
        {
            foreach (var pointToFlash in grid.Where(p => p.ToFlash && !p.HasFlashed))
            {
                // This increases the energy level of all adjacent octopuses by 1, including octopuses that are diagonally adjacent.
                foreach (var (x, y) in pointToFlash.SurroundingPoints!)
                {
                    var adjacentPoint = grid.First(p => p.X == x && p.Y == y);
                    adjacentPoint.EnergyLevel++;
                    if (adjacentPoint.EnergyLevel > 9)
                    {
                        adjacentPoint.ToFlash = true;
                    }
                }

                pointToFlash.HasFlashed = true;
                countFlashes++;
            }
        }

        // Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
        foreach (var pointToReset in grid.Where(p => p.HasFlashed))
        {
            pointToReset.HasFlashed = false;
            pointToReset.ToFlash = false;
            pointToReset.EnergyLevel = 0;
        }
        DrawGrid(grid, $"After step {step}");
    }

    return countFlashes;
}

int Part2(List<Point> grid)
{
    DrawGrid(grid, "Before any steps:");
    var synchronizedStep = 0;
    var step = 1;
    // Perform 100 steps:
    while (synchronizedStep == 0)
    {
        foreach (var point in grid)
        {
            var XYMax = 9;
            // Determine surrounding points for each point:
            point.SurroundingPoints = new List<(int X, int Y)>();
            // Above left:
            if (point.X > 0 && point.Y > 0) point.SurroundingPoints.Add((point.X - 1, point.Y - 1));
            // Above:
            if (point.Y > 0) point.SurroundingPoints.Add((point.X, point.Y - 1));
            // Above right:
            if (point.X < XYMax && point.Y > 0) point.SurroundingPoints.Add((point.X + 1, point.Y - 1));
            // Left:
            if (point.X > 0) point.SurroundingPoints.Add((point.X - 1, point.Y));
            // Right:
            if (point.X < XYMax) point.SurroundingPoints.Add((point.X + 1, point.Y));
            // Below left:
            if (point.X > 0 && point.Y < XYMax) point.SurroundingPoints.Add((point.X - 1, point.Y + 1));
            // Below:
            if (point.Y < XYMax) point.SurroundingPoints.Add((point.X, point.Y + 1));
            // Below right:
            if (point.X < XYMax && point.Y < XYMax) point.SurroundingPoints.Add((point.X + 1, point.Y + 1));

            // First, the energy level of each octopus increases by 1:
            point.EnergyLevel++;

            // Then, any octopus with an energy level greater than 9 flashes.
            if (point.EnergyLevel > 9)
            {
                point.ToFlash = true;
            }
        }

        while (grid.Any(p => p.ToFlash && p.HasFlashed == false))
        {
            foreach (var pointToFlash in grid.Where(p => p.ToFlash && !p.HasFlashed))
            {
                // This increases the energy level of all adjacent octopuses by 1, including octopuses that are diagonally adjacent.
                foreach (var (x, y) in pointToFlash.SurroundingPoints!)
                {
                    var adjacentPoint = grid.First(p => p.X == x && p.Y == y);
                    adjacentPoint.EnergyLevel++;
                    if (adjacentPoint.EnergyLevel > 9)
                    {
                        adjacentPoint.ToFlash = true;
                    }
                }

                pointToFlash.HasFlashed = true;
            }
        }

        // Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
        foreach (var pointToReset in grid.Where(p => p.HasFlashed))
        {
            pointToReset.HasFlashed = false;
            pointToReset.ToFlash = false;
            pointToReset.EnergyLevel = 0;
        }

        DrawGrid(grid, $"After step {step}");
        if (grid.All(p => p.EnergyLevel == 0))
        {
            synchronizedStep = step;
        }
        step++;
    }
    return synchronizedStep;
}