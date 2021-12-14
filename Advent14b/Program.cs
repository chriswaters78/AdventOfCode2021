using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
namespace Advent14b
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);
            var template = lines[0];
            var rules = lines.Skip(2).Select(line => { var split = line.Split(' '); return ((split[0][0], split[0][1]), split[2][0]); });

            var pairCounts = new Dictionary<(char, char), long>();
            for (int i = 0; i < template.Length - 1; i++)
            {                
                pairCounts.TryAdd((template[i], template[i+1]), 0);
                pairCounts[(template[i], template[i + 1])] += 1;
            }

            var charactersAdded = template.GroupBy(ch => ch).ToDictionary(grp => grp.Key, grp => (long) grp.Count());
            for (int step = 0; step < 40; step++)
            {
                var additions = new List<((char, char), long)>();
                foreach (var rule in rules)
                {
                    charactersAdded.TryAdd(rule.Item2, 0);
                    pairCounts.TryAdd(rule.Item1, 0);

                    additions.Add(((rule.Item1.Item1, rule.Item2), pairCounts[rule.Item1]));
                    additions.Add(((rule.Item2, rule.Item1.Item2), pairCounts[rule.Item1]));

                    charactersAdded[rule.Item2] += pairCounts[rule.Item1];
                    pairCounts[rule.Item1] = 0;
                }

                foreach (var addition in additions)
                {
                    pairCounts.TryAdd(addition.Item1, 0);
                    pairCounts[addition.Item1] += addition.Item2;
                }
            }

            var characterCounts = charactersAdded.Select(kvp => kvp.Value).ToList();
            var answer = characterCounts.Max() - characterCounts.Min();
        }
    }
}
