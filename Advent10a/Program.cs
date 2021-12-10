using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

Stopwatch watch = new Stopwatch();
watch.Start();
var close = ")]}>";
var open = "([{<";

var closeToOpen = close.Zip(open, Tuple.Create).ToDictionary(tp => tp.Item1, tp => tp.Item2);
var closeToPoints1 = close.Zip(new[] { 3, 57, 1197, 25137 }, Tuple.Create).ToDictionary(tp => tp.Item1, tp => tp.Item2);
var openToPoints2 = open.Zip(Enumerable.Range(1,4), Tuple.Create).ToDictionary(tp => tp.Item1, tp => tp.Item2);

var lines = File.ReadAllLines(args[0]).ToList();

long part1Score = 0;
var  part2Scores = new List<long>();

foreach (var line in lines)
{
    var stack = new Stack<char>();
    foreach (var c in line)
    {
        if (closeToOpen.ContainsKey(c))
        {
            if (stack.Pop() != closeToOpen[c])
            {
                part1Score += closeToPoints1[c];
                goto invalidline;
            }
        }
        else
        {
            stack.Push(c);
        }
    }

    part2Scores.Add(stack.Aggregate(0L, (score, c) => score * 5 + openToPoints2[c]));
    invalidline:;
}

watch.Stop();
Console.WriteLine($"Part 1: {part1Score}");
Console.WriteLine($"Part 2: {part2Scores.OrderBy(i => i).Skip(part2Scores.Count / 2).First()}");
Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds}ms");
