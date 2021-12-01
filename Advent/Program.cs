using System;
using System.IO;
using System.Linq;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            var answer = File.ReadAllLines(args[0])
                .Select(int.Parse)
                .Aggregate(new { LastReading = int.MaxValue, Count = 0 }, (acc, currReading) => new { LastReading = currReading, Count = currReading > acc.LastReading ? acc.Count + 1 : acc.Count })
                .Count;
        }
    }
}
