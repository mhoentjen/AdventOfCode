using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main()
        {
            var input = File.ReadLines(@"../../../input.txt").Select(item => item.Split(" ")).ToList();
            //var input = File.ReadLines(@"../../../test.txt").Select(item => item.Split(" ")).ToList();
            
            
            Console.WriteLine($"The answer for part 1 is {Part1(input)}");
            Console.WriteLine($"The answer for part 2 is {Part2(input)}");
        }

        private static int Part1(List<string[]> input)
        {
            var depth = 0;
            var position = 0;
            foreach (var item in input)
            {
                if (item[0] == "forward")
                {
                    position += int.Parse(item[1]);
                }
                else if (item[0] == "down")
                {
                    depth += int.Parse(item[1]);
                }
                else if (item[0] == "up")
                {
                    depth -= int.Parse(item[1]);
                }
            }
            return depth * position;
        }

        private static int Part2(List<string[]> input)
        {
            var depth = 0;
            var position = 0;
            var aim = 0;
            foreach (var item in input)
            {
                if (item[0] == "forward")
                {
                    position += int.Parse(item[1]);
                    depth += int.Parse(item[1]) * aim;
                }
                else if (item[0] == "down")
                {
                    aim += int.Parse(item[1]);
                }
                else if (item[0] == "up")
                {
                    aim -= int.Parse(item[1]);
                }
            }
            return depth * position;
        }
    }
}