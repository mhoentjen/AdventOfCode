var input = File.ReadLines(@"../../../input.txt").ToList();
var test = File.ReadLines(@"../../../test.txt").ToList();

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, final answer is {Part1(input)}");
Console.WriteLine($"Part 2, test answer is {Part2(test)}");
Console.WriteLine($"Part 2, final answer is {Part2(input)}");


int Part1(List<string> lines)
{
    var illegalCharacters = new List<char>();
    foreach (var line in lines)
    {
        var charPairs = new Dictionary<char, char>
        {
            {'{', '}'},
            {'(', ')'},
            {'[', ']'},
            {'<', '>'}
        };

        var expectedClosingCharacters = new List<char>();
        foreach (var character in line)
        {
            if (charPairs.ContainsKey(character))
            {
                expectedClosingCharacters.Add(charPairs[character]);
            }
            else if (character != expectedClosingCharacters[^1])
            {
                illegalCharacters.Add(character);
                expectedClosingCharacters.Clear();
                break;
            }
            else
            {
                expectedClosingCharacters.RemoveAt(expectedClosingCharacters.Count - 1);
            }
        }
    }

    return illegalCharacters.Count(c => c == ')') * 3
           + illegalCharacters.Count(c => c == ']') * 57
           + illegalCharacters.Count(c => c == '}') * 1197
           + illegalCharacters.Count(c => c == '>') * 25137;
}

long Part2(List<string> lines)
{
    var scores = new List<long>();
    foreach (var line in lines)
    {
        var lineIsCorrupted = false;
        var charPairs = new Dictionary<char, char>
        {
            {'{', '}'},
            {'(', ')'},
            {'[', ']'},
            {'<', '>'}
        };

        var expectedClosingCharacters = new List<char>();
        foreach (var character in line)
        {
            if (charPairs.ContainsKey(character))
            {
                expectedClosingCharacters.Add(charPairs[character]);
            }
            else if (character != expectedClosingCharacters[^1])
            {
                
                expectedClosingCharacters.Clear();
                lineIsCorrupted = true;
                break;
            }
            else
            {
                expectedClosingCharacters.RemoveAt(expectedClosingCharacters.Count - 1);
            }
        }

        expectedClosingCharacters.Reverse();
        
        if (!lineIsCorrupted)
        {
            long score = 0;
            foreach (var character in expectedClosingCharacters)
            {
                score *= 5;
                switch (character)
                {
                    case ')':
                        score += 1;
                        break;
                    case ']':
                        score += 2;
                        break;
                    case '}':
                        score += 3;
                        break;
                    case '>':
                        score += 4;
                        break;
                }
            }
            scores.Add(score);
        }
    }
    var orderedScores = scores.OrderBy(s => s).ToArray();
    return orderedScores[(scores.Count - 1) / 2];
}
