// See https://aka.ms/new-console-template for more information

var input = ParseInput(File.ReadLines(@"..\..\..\input.txt").ToList());
var test = ParseInput(File.ReadLines(@"..\..\..\test.txt").ToList());

Console.WriteLine($"Part 1, test answer is {Part1(test)}");
Console.WriteLine($"Part 1, final answer is {Part1(input)}");
Console.WriteLine($"Part 2, test answer is {Part2(test)}");
Console.WriteLine($"Part 2, final answer is {Part2(input)}");

List<Entry> ParseInput(List<string> inputToParse)
{
    var entries = new List<Entry>();
    foreach (var line in inputToParse)
    {
        var splitLine = line.Split('|');
        var entry = new Entry()
        {
            SignalPattern = splitLine[0].Split(' ').Where(s => s != "").ToList(),
            OutputValue = splitLine[1].Split(' ').Where(s => s != "").ToList(),
        };
        entries.Add(entry);
    }

    return entries;
}

int Part1(List<Entry> entries)
{
    var counter = 0;
    foreach (var outputValue in entries.SelectMany(entry => entry.OutputValue))
    {
        switch (outputValue.Length)
        {
            case 2:
            case 3:
            case 4:
            case 7:
            {
                counter++;
                break;
            }
        }
    }

    return counter;
}

int Part2(List<Entry> entries)
{
    var segmentsForNumber = new Dictionary<int, string>()
    {
        {0, "abcefg"},
        {1, "cf"},
        {2, "acdeg"},
        {3, "acdfg"},
        {4, "bcdf"},
        {5, "abdfg"},
        {6, "abdefg"},
        {7, "acf"},
        {8, "abcdefg"},
        {9, "abcdfg"}
    };
    
    // Determine usage frequencies for segments (the real ones)
    var segmentFrequencies = new Dictionary<char, int>();
    foreach (var segment in Enumerable.Range('a', 7))
    {
        var count = segmentsForNumber.Count(kvp => kvp.Value.Contains((char) segment));
        segmentFrequencies.Add((char)segment, count);
    }
    
    var totalSum = 0;
    // Start solving each entry:
    foreach (var entry in entries)
    {
        // Prepare list of options for the relations between signal wires and segments
        var signalSegmentCombinations = new List<SignalWireSegmentCombinations>();
        foreach (var signalWire in Enumerable.Range('a', 7))
        {
            signalSegmentCombinations.Add(new SignalWireSegmentCombinations()
            {
                signalWire = (char)signalWire,
                possibleSegments = Enumerable.Range('a', 7).Select(c => (char)c).ToList()
            });
        }
        
        var signalWireFrequencies = new Dictionary<char, int>();
        foreach (var signalWire in Enumerable.Range('a', 7))
        {
            // Determine usage frequencies for signal wires
            var counter = 0;
            foreach (var signal in entry.SignalPattern)
            {
                if (signal.Contains((char) signalWire)) counter++;
            }

            signalWireFrequencies.Add((char) signalWire, counter);
        }
        
        // Eliminate options based on frequency:
        foreach (var item in signalSegmentCombinations)
        {
            foreach (var segment in segmentFrequencies
                         .Where(segment => signalWireFrequencies[item.signalWire] != segment.Value))
                item.possibleSegments.Remove(segment.Key);
        }
        

        var signalNumberCombinations = new List<SignalNumberCombinations>();
        // Find matching signals and numbers based on length:
        foreach (var signal in entry.SignalPattern)
        {
            var addCombination = new SignalNumberCombinations() {signal = signal, possibleNumbers = new List<int>()};
            foreach (var number in segmentsForNumber)
            {
                if (signal.Length == number.Value.Length)
                {
                    // Match found:
                    addCombination.possibleNumbers.Add(number.Key);
                }
            }
            signalNumberCombinations.Add(addCombination);
        }
        
        // If a signal has only one matching number, use that information to eliminate segments
        foreach (var combination in signalNumberCombinations.Where(c => c.possibleNumbers.Count == 1))
        {
            // If a signal belongs to a particular number, eliminate the segments that are not used for that number.
            foreach (var signalWire in combination.signal)
            {
                var removeSegments = new List<char>();
                foreach (var segment in signalSegmentCombinations.First(c => c.signalWire == signalWire).possibleSegments)
                {
                    if (!segmentsForNumber[combination.possibleNumbers[0]].Contains(segment))
                    {
                        removeSegments.Add(segment);
                    }
                }

                foreach (var segment in removeSegments)
                {
                    signalSegmentCombinations.First(c => c.signalWire == signalWire).possibleSegments
                        .Remove(segment);
                }
            }
        }
        
        // If a one-on-one combination exists, use that to eliminate segments in other items
        foreach (var uniqueCombination in signalSegmentCombinations
                     .Where(combination => combination.possibleSegments.Count == 1))
        {
            foreach (var combination in signalSegmentCombinations
                         .Where(combination => combination.possibleSegments.Count > 1))
            {
                combination.possibleSegments.Remove(uniqueCombination.possibleSegments[0]);
            }
        }

        var decodedOutputValues = new List<string>();
        // Replace each character in the output value by the correct segment:
        foreach (var value in entry.OutputValue)
        {
            var decodedOutputValueChars = new List<char>();
            foreach (var signal in value)
            {
                decodedOutputValueChars.Add(signalSegmentCombinations.First(c => c.signalWire == signal).possibleSegments[0]);
            }

            decodedOutputValueChars.Sort();
            var array = decodedOutputValueChars.ToArray();
            var temp = new string(array);
            decodedOutputValues.Add(temp);
        }
        
        // Look up the digits:
        var displayOutput = decodedOutputValues.Select(v => segmentsForNumber
            .First(s => s.Value == v)).Select(kvp => kvp.Key).ToArray();
        
        // Sum them (with the right multipliers):
        var sum = displayOutput[0] * 1000 + displayOutput[1] * 100 + displayOutput[2] * 10 + displayOutput[3] * 1;
        totalSum += sum;
    }

    return totalSum;
}

public record Entry
{
    public List<string> SignalPattern;
    public List<string> OutputValue;
}

public record SignalWireSegmentCombinations
{
    public char signalWire;
    public List<char> possibleSegments;
}

public record SignalNumberCombinations
{
    public string signal;
    public List<int> possibleNumbers;
}