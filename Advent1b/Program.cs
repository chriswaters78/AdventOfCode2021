using System;
using System.IO;
using System.Linq;

namespace Advent1b
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File.ReadAllLines(args[0])
                .Select(int.Parse).ToList();

            var slidingTotals = numbers.Take(numbers.Count - 2).Select((value, index) => numbers.Skip(index).Take(3).Sum());
            var answer = slidingTotals
                .Aggregate(new { LastReading = int.MaxValue, Count = 0 }, (acc, currReading) => new { LastReading = currReading, Count = currReading > acc.LastReading ? acc.Count + 1 : acc.Count })
                .Count;
        }
    }
}
