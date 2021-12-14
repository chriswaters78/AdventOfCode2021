using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Advent14
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);
            var template = new LinkedList<char>(lines[0]);
            var rules = lines.Skip(2).Select(line => { var split = line.Split(' '); return (split[0], split[2][0]); });


            for (int step = 0; step < 40; step++)
            {
                var currentNode = template.First;
                List<(LinkedListNode<char>, char)> inserts = new List<(LinkedListNode<char>, char)>();
                while (currentNode.Next != null)
                {
                    foreach (var rule in rules)
                    {
                        if (rule.Item1[0] == currentNode.Value && rule.Item1[1] == currentNode.Next.Value)
                        {
                            inserts.Add((currentNode, rule.Item2));
                        }
                    }

                    currentNode = currentNode.Next;
                }

                foreach (var insert in inserts)
                {
                    var next = insert.Item1.Next;
                    template.AddAfter(insert.Item1, insert.Item2);                    
                }
            }

            var sorted = template.GroupBy(ch => ch).Select(grp => (grp.Key, grp.Count())).OrderBy(tp => tp.Item2).ToList();
            var answer = sorted.Last().Item2 - sorted.First().Item2;
        }
    }
}
