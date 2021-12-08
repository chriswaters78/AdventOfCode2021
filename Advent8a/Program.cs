using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent8a
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);

            int part1Answer = 0;
            int part2Answer = 0;
            foreach (var line in lines)
            {
                var input = line.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                var patterns = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(str => new String(str.OrderBy(c => c).ToArray())).ToList();
                var outputValues = input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(str => new String(str.OrderBy(c => c).ToArray())).ToList();

                var indexOfDigits = Enumerable.Repeat(-1,10).ToList();
                
                indexOfDigits[1] = patterns.Select((pattern,index) => new { Pattern = pattern, Index = index }).Where(a => a.Pattern.Length == 2).Single().Index;
                indexOfDigits[7] = patterns.Select((pattern, index) => new { Pattern = pattern, Index = index }).Where(a => a.Pattern.Length == 3).Single().Index;
                indexOfDigits[4] = patterns.Select((pattern, index) => new { Pattern = pattern, Index = index }).Where(a => a.Pattern.Length == 4).Single().Index;
                indexOfDigits[8] = patterns.Select((pattern, index) => new { Pattern = pattern, Index = index }).Where(a => a.Pattern.Length == 7).Single().Index;

                var indexes069 = patterns.Select((pattern, index) => new { Pattern = pattern, Index = index }).Where(a => a.Pattern.Length == 6).Select(a => a.Index).ToList();
                if (indexes069.Count != 3)
                {
                    throw new Exception("BOOM!");
                }
                
                indexOfDigits[6] = indexes069.Where(i => getSharedCharCount(patterns[i], patterns[indexOfDigits[1]]) == 1).Single();
                var indexes09 = indexes069.Except(new[] { indexOfDigits[6] }).ToList();

                if (getSharedCharCount(patterns[indexes09[0]], patterns[indexOfDigits[4]]) == 4)
                {
                    indexOfDigits[9] = indexes09[0];
                    indexOfDigits[0] = indexes09[1];
                }
                else if (getSharedCharCount(patterns[indexes09[0]], patterns[indexOfDigits[4]]) == 3)
                {
                    indexOfDigits[9] = indexes09[1];
                    indexOfDigits[0] = indexes09[0];
                }

                var indexes235 = patterns.Select((pattern, index) => new { Pattern = pattern, Index = index }).Where(a => a.Pattern.Length == 5).Select(a => a.Index).ToList();
                indexOfDigits[3] = indexes235.Where(i => getSharedCharCount(patterns[i], patterns[indexOfDigits[1]]) == 2).Single();
                var indexes25 = indexes235.Except(new[] { indexOfDigits[3] }).ToList();
                if (getSharedCharCount(patterns[indexes25[0]], patterns[indexOfDigits[6]]) == 4)
                {
                    indexOfDigits[2] = indexes25[0];
                    indexOfDigits[5] = indexes25[1];
                }
                else
                {
                    indexOfDigits[2] = indexes25[1];
                    indexOfDigits[5] = indexes25[0];
                }

                var outputValuePatternIndexes = outputValues.Select(ov => patterns.IndexOf(ov)).ToList();
                var part1Indexes = new[] { indexOfDigits[1], indexOfDigits[4], indexOfDigits[7], indexOfDigits[8]};
                foreach (var pi in outputValuePatternIndexes)
                {
                    if (part1Indexes.Contains(pi))
                    {
                        part1Answer += 1;
                    }
                }

                part2Answer += indexOfDigits.IndexOf(patterns.IndexOf(outputValues[0])) * 1000;
                part2Answer += indexOfDigits.IndexOf(patterns.IndexOf(outputValues[1])) * 100;
                part2Answer += indexOfDigits.IndexOf(patterns.IndexOf(outputValues[2])) * 10;
                part2Answer += indexOfDigits.IndexOf(patterns.IndexOf(outputValues[3])) * 1;
            }
        }

        private static int getSharedCharCount(string a, string b)
        {
            return a.Intersect(b).Count();
        }
    }
}
