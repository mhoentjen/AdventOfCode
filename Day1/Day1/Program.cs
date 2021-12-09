using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    internal static class Program
    {
        private static void Main()
        {
            var input = File.ReadLines(@"..\..\..\input.txt").Select(int.Parse).ToList();

            Console.WriteLine($"The answer for part 1 is {Part1(input)}");
            Console.WriteLine($"The answer for part 2 is {Part2(input)}");
        }

        private static int Part1(List<int> input)
        {
            var previous = input[0];
            var counter = 0;
            foreach (var item in input)
            {
                if (item > previous)
                {
                    counter++;
                }
                previous = item;
            }
            return counter;
        }

        private static int Part2(List<int> input)
        {
            var previous = input[0] + input[1] + input[2];
            var counter = 0;
            for (var i = 0; i < input.Count - 2; i++)
            {
                var sum = input[i] + input[i + 1] + input[i + 2];
                if (sum > previous)
                {
                    counter++;
                }
                previous = sum;
            }
            return counter;
        }
    }
}