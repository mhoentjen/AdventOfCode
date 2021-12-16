using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5
{
    class Program
    {
        static void Main()
        {
            var input = ParseInput(File.ReadLines(@"..\..\..\input.txt").ToList());
            var test = ParseInput(File.ReadLines(@"..\..\..\test.txt").ToList());

            Console.WriteLine($"Part 1, test answer is {Part1(test)}");
            Console.WriteLine($"Part 1, final answer is {Part1(input)}");
            Console.WriteLine($"Part 2, test answer is {Part2(test)}");
            Console.WriteLine($"Part 2, final answer is {Part2(input)}");
        }
        
        private static ParsedInput ParseInput(IEnumerable<string> input)
        {
            var coordinates = input.Select(row => row.Split(" -> "));
            return new ParsedInput()
            {
                Lines = coordinates.Select(c => new Line()
                {
                    Start = new Point()
                    {
                        X = int.Parse(c[0].Split(',')[0].Trim()),
                        Y = int.Parse(c[0].Split(',')[1].Trim())
                    },
                    End = new Point()
                    {
                        X = int.Parse(c[1].Split(',')[0].Trim()),
                        Y = int.Parse(c[1].Split(',')[1].Trim())
                    }
                }).ToList()
            };
        }

        private record Line
        {
            public Point Start;
            public Point End;
        }

        private record Point
        {
            public int X;
            public int Y;
        }
        
        private record ParsedInput
        {
            public List<Line> Lines;
        }

        private static void DrawGrid(IEnumerable<int[]> grid, string fileName)
        {
            var rowsList = grid.Select(row => row.Aggregate("", (current, point) => current + point.ToString())).ToList();
            File.WriteAllLines(@$"..\..\..\{fileName}", rowsList);
        }

        private static int[][] CreateGrid(ParsedInput input)
        {
            // Make a grid with points. The size is based on the largest coordinates found in the input
            var gridWidth = input.Lines.Max(l => Math.Max(l.Start.X, l.End.X)+1);
            var gridHeight = input.Lines.Max(l => Math.Max(l.Start.Y, l.End.Y)+1);
            var grid = new int[gridHeight][];
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                grid[i] = new int[gridWidth];
                Array.Fill(grid[i], 0);
            }

            return grid;
        }

        
        private static int Part1(ParsedInput input)
        {
            var grid = CreateGrid(input);
            
            //DrawGrid(grid, "empty.txt");
            // Process each line:
            foreach (var line in input.Lines)
            {
                //Only use horizontal and vertical lines:
                if (line.Start.X != line.End.X && line.Start.Y != line.End.Y) continue;
                
                // Create new list to store which points on the grid are part of the line:
                var addPoints = new List<Point>();

                // Vertical line:
                if (line.Start.X == line.End.X)
                {
                    addPoints = DeterminePointsInVerticalLine(line);
                }
                else if (line.Start.Y == line.End.Y)
                {
                    addPoints = DeterminePointsInHorizontalLine(line);
                }
                
                // Go through all points to add and mark them on the grid:
                foreach (var point in addPoints)
                {
                    grid[point.Y][point.X]++;
                }
            }

            //DrawGrid(grid, "Full.txt");
            
            // Count the points on the grid that are higher than 1:

            var counter = 0;
            foreach (var row in grid)
            {
                foreach (var point in row)
                {
                    if (point > 1) counter++;
                }
            }

            return counter;
        }

        private static List<Point> DeterminePointsInVerticalLine(Line line)
        {
            // Create new list to store which points on the grid are part of the line:
            var addPoints = new List<Point>();

            // Vertical line:
            if (line.Start.X == line.End.X)
            {
                for (var y = Math.Min(line.Start.Y, line.End.Y); y <= Math.Max(line.Start.Y, line.End.Y); y++)
                {
                    addPoints.Add(new Point()
                        {
                            X = line.Start.X,
                            Y = y
                        }
                    );
                }
            }

            return addPoints;
        }
        
        private static List<Point> DeterminePointsInHorizontalLine(Line line)
        {
            // Create new list to store which points on the grid are part of the line:
            var addPoints = new List<Point>();
            
            if (line.Start.Y == line.End.Y)
            {
                for (var x = Math.Min(line.Start.X, line.End.X); x <= Math.Max(line.Start.X, line.End.X); x++)
                {
                    addPoints.Add(new Point()
                        {
                            X = x,
                            Y = line.Start.Y
                        }
                    );
                }
            }
            
            return addPoints;
        }
        
        private static List<Point> DeterminePointsInDiagonalLine(Line line)
        {
            // Create new list to store which points on the grid are part of the line:
            var addPoints = new List<Point>();

            // Diagonal line:
            if (Math.Abs(line.Start.X - line.End.X) == Math.Abs(line.Start.Y - line.End.Y))
            {
                for (var i = 0; i <= Math.Abs(line.End.X - line.Start.X); i++)
                {
                    var directionX = line.End.X > line.Start.X ? 1 : -1;
                    var directionY = line.End.Y > line.Start.Y ? 1 : -1;
                    addPoints.Add(new Point()
                        {
                            X = line.Start.X + i * directionX,
                            Y = line.Start.Y + i * directionY,
                        }
                    );
                }
            }

            return addPoints;
        }

        private static int Part2(ParsedInput input)
        {
            var grid = CreateGrid(input);
            
            //DrawGrid(grid, "empty.txt");
            // Process each line:
            foreach (var line in input.Lines)
            {
                // Create new list to store which points on the grid are part of the line:
                List<Point> addPoints;

                if (line.Start.X == line.End.X)
                {
                    addPoints = DeterminePointsInVerticalLine(line);
                }
                else if (line.Start.Y == line.End.Y)
                {
                    addPoints = DeterminePointsInHorizontalLine(line);
                }
                else addPoints = DeterminePointsInDiagonalLine(line);
                
                // Go through all points to add and mark them on the grid:
                foreach (var point in addPoints)
                {
                    grid[point.Y][point.X]++;
                }
            }

            //DrawGrid(grid, "Full.txt");
            
            // Count the points on the grid that are higher than 1:

            var counter = 0;
            foreach (var row in grid)
            {
                foreach (var point in row)
                {
                    if (point > 1) counter++;
                }
            }

            return counter;
        }
    }
}