using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

Stopwatch watch = new Stopwatch();
watch.Start();

Dictionary<string, HashSet<string>> graph = new Dictionary<string, HashSet<string>>();

foreach (var edge in File.ReadAllLines(args[0]))
{
    var split = edge.Split('-');
    var nodeA = split[0];
    var nodeB = split[1];

    if (!graph.ContainsKey(nodeA))
    {
        graph[nodeA] = new HashSet<string>();
    }
    graph[nodeA].Add(nodeB);

    if (!graph.ContainsKey(nodeB))
    {
        graph[nodeB] = new HashSet<string>();
    }
    graph[nodeB].Add(nodeA);
}

int paths = 0;
Stack<(string, HashSet<string>, string)> stack = new Stack<(string, HashSet<string>, string)>(new[] { ("start", new HashSet<string>(new[] { "start" }), (string) null) });

while (stack.Any())
{
    (var node, var scVisited, var visitedTwice) = stack.Pop();
    
    if (node == "end")
    {
        paths++;
    }
    else
    {
        foreach (var toVisit in graph[node])
        {
            if (toVisit == "start")
            {
                continue;
            }

            if (Char.IsLower(toVisit.First()))
            {
                if (scVisited.Contains(toVisit))
                {
                    if (visitedTwice == null)
                    {
                        var cloned = new HashSet<string>(scVisited);
                        cloned.Add(toVisit);
                        stack.Push((toVisit, cloned, toVisit));

                    }
                }
                else
                {
                    var cloned = new HashSet<string>(scVisited);
                    cloned.Add(toVisit);
                    stack.Push((toVisit, cloned, visitedTwice));
                }                
            }
            else
            {
                var cloned = new HashSet<string>(scVisited);
                stack.Push((toVisit, cloned, visitedTwice));
            }
        }
    }
}

watch.Stop();
Console.WriteLine($"Part 1: {paths}");
Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds}ms");
