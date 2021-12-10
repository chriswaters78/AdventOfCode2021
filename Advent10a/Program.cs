using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

Stopwatch watch = new Stopwatch();
watch.Start();
//var close = ")]}>";
//var open = "([{<";
//var points1 = new[] { 3, 57, 1197, 25137 };
var pairs = new Dictionary<char, char> { { ')', '(' }, { ']', '[' }, { '}', '{' }, { '>', '<' } };
var part1ScoreMap = new Dictionary<char, int> { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
var part2ScoreMap = new Dictionary<char, int> { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };

var lines = File.ReadAllLines(args[0]).ToList();

long part1Score = 0;
var  part2Scores = new List<long>();

foreach (var line in lines)
{
    var stack = new Stack<char>(line.Take(1));
    foreach (var c in line.Skip(1))
    {
        if (pairs.ContainsKey(c))
        {
            if (stack.Peek() != pairs[c])
            {
                part1Score += part1ScoreMap[c];
                goto invalidline;
            }
            stack.Pop();
        }
        else
        {
            stack.Push(c);
        }
    }

    part2Scores.Add(stack.Aggregate((long) 0, (score, c) => score * 5 + part2ScoreMap[c]));
    invalidline:;
}

watch.Stop();
Console.WriteLine($"Part 1: {part1Score}");
Console.WriteLine($"Part 2: {part2Scores.OrderBy(i => i).Skip(part2Scores.Count / 2).First()}");
Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds}ms");
