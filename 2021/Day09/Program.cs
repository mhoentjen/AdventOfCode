using Day09;

var input = ParseInput(File.ReadLines(@"../../../input.txt").ToList());
var test = ParseInput(File.ReadLines(@"../../../test.txt").ToList());

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
                Height = int.Parse(item),
                X = j,
                Y = i
            };
            parsedInput.Add(point);
        }
    }
    return parsedInput;
}

int Part1(List<Point> heightMap)
{
    foreach (var point in heightMap)
    {
        point.Above = heightMap.FirstOrDefault(p => p.X == point.X && p.Y == point.Y - 1)?.Height;
        point.Left = heightMap.FirstOrDefault(p => p.X == point.X - 1 && p.Y == point.Y)?.Height;
        point.Right = heightMap.FirstOrDefault(p => p.X == point.X + 1 && p.Y == point.Y)?.Height;
        point.Below = heightMap.FirstOrDefault(p => p.X == point.X && p.Y == point.Y + 1)?.Height;
        point.IsLowPoint = (point.Height < point.Above || point.Above == null) &&
                           (point.Height < point.Left || point.Left == null) &&
                           (point.Height < point.Right || point.Right == null) &&
                           (point.Height < point.Below || point.Below == null);
    }

    return heightMap.Where(p => p.IsLowPoint).Sum(p => p.Height + 1);
}

int Part2(List<Point> heightMap)
{
    foreach (var point in heightMap)
    {
        point.Above = heightMap.FirstOrDefault(p => p.X == point.X && p.Y == point.Y - 1)?.Height;
        point.Left = heightMap.FirstOrDefault(p => p.X == point.X - 1 && p.Y == point.Y)?.Height;
        point.Right = heightMap.FirstOrDefault(p => p.X == point.X + 1 && p.Y == point.Y)?.Height;
        point.Below = heightMap.FirstOrDefault(p => p.X == point.X && p.Y == point.Y + 1)?.Height;
        point.IsLowPoint = (point.Height < point.Above || point.Above == null) &&
                           (point.Height < point.Left || point.Left == null) &&
                           (point.Height < point.Right || point.Right == null) &&
                           (point.Height < point.Below || point.Below == null);
        if (point.IsLowPoint)
        {
            point.ToProcess = true;
            point.LowPointX = point.X;
            point.LowPointY = point.Y;
        }
    }

    while (heightMap.Any(p => p.ToProcess))
    {
        foreach (var point in heightMap.Where(p => p.ToProcess))
        {
            if (point.Above != null && point.Above > point.Height && point.Above < 9)
            {
                var pointAbove = heightMap.First(p => p.X == point.X && p.Y == point.Y - 1);
                pointAbove.ToProcess = true;
                pointAbove.LowPointX = point.LowPointX;
                pointAbove.LowPointY = point.LowPointY;
            }
            if (point.Left != null && point.Left > point.Height && point.Left < 9)
            {
                var pointLeft = heightMap.First(p => p.X == point.X - 1 && p.Y == point.Y);
                pointLeft.ToProcess = true;
                pointLeft.LowPointX = point.LowPointX;
                pointLeft.LowPointY = point.LowPointY;
            }
            if (point.Right != null && point.Right > point.Height && point.Right < 9)
            {
                var pointRight = heightMap.First(p => p.X == point.X + 1 && p.Y == point.Y);
                pointRight.ToProcess = true;
                pointRight.LowPointX = point.LowPointX;
                pointRight.LowPointY = point.LowPointY;
            }
            if (point.Below != null && point.Below > point.Height && point.Below < 9)
            {
                var pointBelow = heightMap.First(p => p.X == point.X && p.Y == point.Y + 1);
                pointBelow.ToProcess = true;
                pointBelow.LowPointX = point.LowPointX;
                pointBelow.LowPointY = point.LowPointY;
            }

            point.ToProcess = false;
        }
    }
    
    // Count points in every basin:
    var counts = heightMap.Where(p => p.IsLowPoint).Select(lowPoint => heightMap.Count(p => p.LowPointX == lowPoint.X && p.LowPointY == lowPoint.Y)).ToList();
    
    var orderedCounts = counts.OrderByDescending(c => c).ToArray();
    return orderedCounts[0] * orderedCounts[1] * orderedCounts[2];
}