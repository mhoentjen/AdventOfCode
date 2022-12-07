var test = System.IO.File.ReadLines(@"../../../test.txt");
var input = System.IO.File.ReadLines(@"../../../input.txt");

int Part1(IEnumerable<string> input)
{
    var fileSystem = BuildFileSystem(input);
    var smallDirectories = fileSystem.Where(x => x.Size < 100000);
    return smallDirectories.Sum(dir => dir.Size);
}

int Part2(IEnumerable<string> input)
{
    var fileSystem = BuildFileSystem(input).ToList();
    var availableSpace = 70000000 - fileSystem.Single(d => d.Name == "/").Size;
    var spaceToFree = 30000000 - availableSpace;
    return fileSystem.Where(dir => dir.Size > spaceToFree).Min(dir => dir.Size);
}

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, answer is {Part1(input)}");
Console.WriteLine($"Part 2, test answer is {Part2(test)}");
Console.WriteLine($"Part 2, answer is {Part2(input)}");


IEnumerable<Directory> BuildFileSystem(IEnumerable<string> input)
{
    var fileSystem = new List<Directory>()
    {
        new("/", null)
    };
    var pwd = fileSystem[0];
    foreach (var line in input)
    {
        if (line == "$ cd /")
        {
            continue;
        }
        var elements = line.Split(' ');
        if (elements[0] == "$")
        {
            switch (elements[1])
            {
                case "cd":
                    pwd = elements[2] == ".." ?
                        pwd?.Parent :
                        pwd?.Directories.Single(d => d.Name == elements[2]);
                    continue;
                case "ls":
                    continue;
            }
        }
        if (elements[0] == "dir")
        {
            fileSystem.Add(new Directory(elements[1], pwd));
            pwd?.Directories.Add(fileSystem.Last());
            continue;
        }
        pwd?.Files.Add(new File(int.Parse(elements[0])));
    }
    return fileSystem;
}

internal record Directory(string Name, Directory? Parent)
{
    internal List<Directory> Directories { get; } = new();
    internal List<File> Files { get; } = new();

    internal int Size => Directories.Sum(d => d.Size) + Files.Sum(f => f.Size);
}

internal record File(int Size);