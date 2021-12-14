using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
{
    class Program
    {
        private static void Main()
        {
            var input = ParseInput(File.ReadLines(@"..\..\..\input.txt").ToList());
            var test = ParseInput(File.ReadLines(@"..\..\..\test.txt").ToList());

            Console.WriteLine($"Part 1, test answer is {Part1(test)}");
            Console.WriteLine($"Part 1, final answer is {Part1(input)}");
            Console.WriteLine($"Part 2, test answer is {Part2(test)}");
            Console.WriteLine($"Part 2, final answer is {Part2(input)}");
        }

        private record Number
        {
            public int Value;
            public bool Marked;
            public int Row;
            public int Column;
        }

        private record Board
        {
            public List<Number> Numbers;
            public bool Completed;
        }

        private record Result
        {
            public int FinalScore;
            public int Ranking;
        }

        private record ParsedInput
        {
            public List<int> DrawnNumbers;
            public List<Board> Boards;
        }

        private static ParsedInput ParseInput(IReadOnlyList<string> input)
        {
            var drawnNumbers = input[0].Split(',').Select(int.Parse).ToList();
            var boards = new List<Board>();

            for (var i = 2; i < input.Count; i += 6)
            {
                var board = new Board
                {
                    Numbers = new List<Number>()
                };
                for (var j = 0; j < 5; j++)
                {
                    var rowString = input[i + j].Split(' ').Where(item => item != "");
                    var rowInt = rowString.Select(item => int.Parse(item.Trim())).ToList();
                    board.Numbers.AddRange(rowInt.Select((t, k) => new Number() {Value = t, Row = j, Column = k}));
                }
                boards.Add(board);
            }

            return new ParsedInput()
            {
                DrawnNumbers = drawnNumbers,
                Boards = boards
            };
        }
        
        
        private static int Part1(ParsedInput input)
        {
            foreach (var drawnNumber in input.DrawnNumbers)
            {
                // Mark the drawn number on each board
                foreach (var board in input.Boards)
                {
                    foreach (var number in board.Numbers.Where(number => number.Value == drawnNumber))
                    {
                        number.Marked = true;
                    }

                    // Check if this board has a completed row or column
                    var columnCompleted = board.Numbers.GroupBy(number => number.Column).Any(column => column.All(number => number.Marked));
                    var rowCompleted = board.Numbers.GroupBy(number => number.Row).Any(row => row.All(number => number.Marked));
                    
                    // If so, calculate the score (the sum of all unmarked numbers, multiplied by the drawn number)
                    if (columnCompleted || rowCompleted)
                    {
                        var sumUnmarked = board.Numbers.Where(number => number.Marked == false).Sum(number => number.Value);
                        return sumUnmarked * drawnNumber;
                    }
                }
            }

            return 0;
        }
        
        
        private static int Part2(ParsedInput input)
        {
            var results = new List<Result>();
            
            foreach (var drawnNumber in input.DrawnNumbers)
            {
                // Mark the drawn number on each board
                foreach (var board in input.Boards)
                {
                    if (board.Completed) continue;
                    foreach (var number in board.Numbers.Where(number => number.Value == drawnNumber))
                    {
                        number.Marked = true;
                    }

                    // Check if this board has a completed row or column
                    var columnCompleted = board.Numbers.GroupBy(number => number.Column)
                        .Any(column => column.All(number => number.Marked));
                    var rowCompleted = board.Numbers.GroupBy(number => number.Row)
                        .Any(row => row.All(number => number.Marked));

                    // If so, calculate the score (the sum of all unmarked numbers, multiplied by the drawn number)
                    if (columnCompleted || rowCompleted)
                    {
                        board.Completed = true;
                        var sumUnmarked = board.Numbers.Where(number => number.Marked == false)
                            .Sum(number => number.Value);
                        results.Add(new Result()
                        {
                            FinalScore = sumUnmarked * drawnNumber,
                            Ranking = (results.LastOrDefault()?.Ranking ?? 0) + 1
                        });
                    }
                }
            }
            // Return the final score of the board that was last to be completed
            return results.OrderByDescending(r => r.Ranking).First().FinalScore;
        }
    }
}