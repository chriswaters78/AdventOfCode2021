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
            var template = new LinkedList<char>(lines[0]);
            var rules = lines.Skip(2).Select(line => { var split = line.Split(' '); return (split[0], split[2][0]); });

            Dictionary<string, long> pairCounts = new Dictionary<string, long>();
            var currentNode = template.First;
            while (currentNode.Next != null)
            {
                var pair = new String(new[] { currentNode.Value, currentNode.Next.Value });
                if (!pairCounts.ContainsKey(pair))
                {
                    pairCounts[pair] = 0;
                }
                pairCounts[pair] += 1;

                currentNode = currentNode.Next;
            }

            Dictionary<char, long> charactersAdded = new Dictionary<char, long>();
            for (int step = 0; step < 40; step++)
            {
                List<(string, long)> additions = new List<(string, long)>();
                foreach (var rule in rules)
                {
                    if (pairCounts.ContainsKey(rule.Item1) && pairCounts[rule.Item1] > 0)
                    {
                        //e.g. say we have BC -> D a total of N times
                        //we set BC count to 0
                        //we add BD x N
                        //we add DC x N

                        var newPair1 = new String(new[] { rule.Item1[0], rule.Item2 });
                        var newPair2 = new String(new[] { rule.Item2, rule.Item1[1]});

                        additions.Add((newPair1, pairCounts[rule.Item1]));
                        additions.Add((newPair2, pairCounts[rule.Item1]));

                        if (!charactersAdded.ContainsKey(rule.Item2))
                        {
                            charactersAdded[rule.Item2] = 0;
                        }
                        charactersAdded[rule.Item2] += pairCounts[rule.Item1];
                        pairCounts[rule.Item1] = 0;
                    }
                }

                foreach (var addition in additions)
                {
                    if (!pairCounts.ContainsKey(addition.Item1))
                    {
                        pairCounts[addition.Item1] = 0;
                    }
                    pairCounts[addition.Item1] += addition.Item2;
                }
            }

            foreach (var c in template)
            {
                if (!charactersAdded.ContainsKey(c))
                {
                    charactersAdded[c] = 0;
                }
                charactersAdded[c] += 1;
            }

            var characterCounts = charactersAdded.Select(kvp => kvp.Value).ToList();
            var answer = characterCounts.Max() - characterCounts.Min();

        }
    }
}
