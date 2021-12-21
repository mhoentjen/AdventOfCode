using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main()
        {
            var input = File.ReadLines(@"..\..\..\input.txt").ToList();
            var test = File.ReadLines(@"..\..\..\test.txt").ToList();

            Console.WriteLine($"The answer based on the test set for part 1 is {Part1(test)}");
            Console.WriteLine($"The answer for part 1 is {Part1(input)}");
            Console.WriteLine($"The answer based on the test set for part 2 is {Part2(test)}");
            Console.WriteLine($"The answer for part 2 is {Part2(input)}");
        }

        private static int Part1(List<string> input)
        {
            var onesCount = new int[input[0].Length];
            var zeroesCount = new int[input[0].Length];

            // Count ones and zeroes for each position
            foreach (var t in input)
            {
                var characters = t.ToCharArray();
                for (var j = 0; j < onesCount.Length; j++)
                {
                    if (characters[j] == '1') onesCount[j]++;
                    else zeroesCount[j]++;
                }
            }

            var gammaRateStr = "";
            var epsilonRateStr = "";
            for (var i = 0; i < onesCount.Length; i++)
            {
                if (onesCount[i] > zeroesCount[i])
                {
                    gammaRateStr += "1";
                    epsilonRateStr += "0";
                }
                else
                {
                    gammaRateStr += "0";
                    epsilonRateStr += "1";
                }
            }

            var gammaRate = Convert.ToInt32(gammaRateStr, 2);
            var epsilonRate = Convert.ToInt32(epsilonRateStr, 2);

            return gammaRate * epsilonRate;
        }
        
        private static int Part2(List<string> input)
        {
            var o2GenList = input;
            var co2ScrubberList = input;
            var indexO2 = 0;
            while (o2GenList.Count > 1)
            {
                var onesCount = 0;
                var zeroesCount = 0;
                foreach (var item in o2GenList)
                {
                    if (item[indexO2] == '1') onesCount++;
                    else zeroesCount++;
                }
                o2GenList = o2GenList.Where(item => item[indexO2] == (onesCount >= zeroesCount ? '1' : '0')).ToList();
                indexO2++;
            }
            var indexCO2 = 0;
            while (co2ScrubberList.Count > 1)
            {
                var onesCount = 0;
                var zeroesCount = 0;
                foreach (var item in co2ScrubberList)
                {
                    if (item[indexCO2] == '1') onesCount++;
                    else zeroesCount++;
                }
                co2ScrubberList = co2ScrubberList.Where(item => item[indexCO2] == (zeroesCount <= onesCount ? '0' : '1')).ToList();
                indexCO2++;
            }

            var o2Gen = Convert.ToInt32(o2GenList[0], 2);
            var co2Scrubber = Convert.ToInt32(co2ScrubberList[0], 2);

            return o2Gen * co2Scrubber;
        }
    }
}