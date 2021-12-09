using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent9a
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(args[0]).Select(str => str.Select(c => int.Parse(c.ToString())).ToList()).ToList();
            var lineLength = input[0].Count;
            var noLines = input.Count;
            input = new[] { Enumerable.Repeat(int.MaxValue, lineLength + 2) }.Concat(
                input.Select(line => new int[] { int.MaxValue }
                .Concat(line)
                .Concat(new[] { int.MaxValue })))
                .Concat(new[] { Enumerable.Repeat(int.MaxValue, lineLength + 2) }).Select(ie => ie.ToList()).ToList();

            List<int> lowPoints = new List<int>();
            List<HashSet<(int, int)>> basins = new List<HashSet<(int, int)>>();
            for (int i = 1; i <= noLines; i++)
            {
                for (int j = 1; j <= lineLength; j++)
                {
                    if (input[i-1][j] > input[i][j]
                        && input[i + 1][j] > input[i][j]
                        && input[i][j - 1] > input[i][j]
                        && input[i][j + 1] > input[i][j])
                    {
                        lowPoints.Add(input[i][j]);
                        basins.Add(new HashSet<(int, int)>() { (i, j) });
                    }
                }
            }

            var answer = lowPoints.Sum(i => i + 1);

            //expand each basin
            for (int c = 0; c < basins.Count; c++)
            {
                while (true)
                {
                    var expandedBasin = new HashSet<(int, int)>(basins[c]);
                    foreach (var (i, j) in basins[c])
                    {
                        var candidates = new[] { (i - 1, j), (i + 1, j), (i, j - 1), (i, j + 1 )};
                        foreach (var (ci,cj) in candidates)
                        {
                            if (input[ci][cj] > input[i][j] && input[ci][cj] != int.MaxValue && input[ci][cj] != 9)
                            {
                                if (!expandedBasin.Contains((ci,cj)))
                                {
                                    expandedBasin.Add((ci, cj));
                                }
                            }
                        }
                    }

                    if (expandedBasin.Count == basins[c].Count)
                    {
                        break;
                    }
                    basins[c] = expandedBasin;
                }
            }

            var answer2 = basins.OrderByDescending(basin => basin.Count).Take(3).Aggregate(1, (total, hs) => total * hs.Count);
        }
    }
}
