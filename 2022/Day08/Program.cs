var test = File.ReadLines(@"../../../test.txt").ToList();
var testTrees = ParseInput(test).ToList();
var input = File.ReadLines(@"../../../input.txt").ToList();
var inputTrees = ParseInput(input).ToList();
Dictionary<string, (int, int)> directions = new()
{
    {"Up", (0, -1)},
    {"Left", (-1, 0)},
    {"Right", (1, 0)},
    {"Down", (0, 1)}
};

// Console.WriteLine($"Part 1, test answer is {Part1(testTrees)}");
Console.WriteLine($"Part 1, answer is {Part1(inputTrees)}");
// Console.WriteLine($"Part 2, test answer is {Part2(testTrees)}");
// Console.WriteLine($"Part 2, answer is {Part2(inputTrees)}");

int Part1(List<Tree> trees)
{
    foreach (var tree in trees)
    {
        var visible = false;
        foreach (var _ in directions.Values.Where(direction => CheckVisibilityFromDirection(tree, trees, direction)))
        {
            visible = true;
        }

        tree.Visible = visible;
    }

    return trees.Count(t => t.Visible);
}

int Part2(List<Tree> trees)
{
    foreach (var tree in trees)
    {
        tree.ScenicScore = directions.Values.Aggregate(1, (current, direction) => current * FindViewingDistanceInDirection(tree, trees, direction));
    }

    return trees.Max(t => t.ScenicScore);
}

bool CheckVisibilityFromDirection(Tree tree, List<Tree> allTrees, (int X, int Y) direction)
{
    var index = 1;
    var visible = true;
    while (true)
    {
        var nextTree = allTrees.FirstOrDefault(t => t.X == tree.X + index * direction.X && t.Y == tree.Y + index * direction.Y);
        if (nextTree is null) break;
        if (nextTree.Height >= tree.Height) visible = false;
        index++;
    }

    return visible;
}

int FindViewingDistanceInDirection(Tree tree, List<Tree> allTrees, (int X, int Y) direction)
{
    var index = 1;
    while (true)
    {
        var nextTree = allTrees.FirstOrDefault(t => t.X == tree.X + index * direction.X && t.Y == tree.Y + index * direction.Y);
        if (nextTree is null)
        {
            return index - 1;
        }
        if (nextTree.Height >= tree.Height)
        {
            return index;
        }
        index++;
    }
}

IEnumerable<Tree> ParseInput(List<string> lines)
{
    for (var y = 0; y < lines.Count; y++)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; x++)
        {
            yield return new Tree(int.Parse(line[x].ToString()), x, y);
        }
    }
}

internal record Tree(int Height, int X, int Y)
{
    internal bool Visible { get; set; }
    internal int ScenicScore { get; set; }
}