using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

Stopwatch watch = new Stopwatch();
watch.Start();

var allLines = File.ReadAllLines(args[0]);
var pointLines = allLines.TakeWhile(line => !String.IsNullOrEmpty(line));
var foldLines = allLines.SkipWhile(line => !line.StartsWith("fold")).ToList();

var coords = pointLines.Select(line => { var split = line.Split(','); return (int.Parse(split[0]), int.Parse(split[1])); }).ToList();
var folds = foldLines.Select(line =>
{
    var split = line.Substring(11).Split('=');
    return (split[0], int.Parse(split[1]));
}).ToList();

var maxX = coords.Max(tp => tp.Item1);
var maxY = coords.Max(tp => tp.Item2);

var paper = new bool[maxX + 1,maxY + 1];

foreach ((var x, var y) in coords)
{
    paper[x, y] = true;
}

foreach (var fold in folds)
{
    for (int x = 0; x <= maxX; x++)
    {
        for (int y = 0; y <= maxY; y++)
        {
            if (paper[x, y])
            {
                if (fold.Item1 == "x")
                {
                    if (x > fold.Item2)
                    {
                        paper[x, y] = false;
                        paper[2 * fold.Item2 - x, y] = true;
                    }
                }
                else
                {
                    if (y > fold.Item2)
                    {
                        paper[x, y] = false;
                        paper[x, 2 * fold.Item2 - y] = true;
                    }
                }
            }
        }
    }

    (maxX, maxY) = fold.Item1 == "x" 
        ? (fold.Item2, maxY) 
        : (maxX, fold.Item2);
}

int answer1 = 0;
for (int y = 0; y <= maxY; y++)
{
    for (int x = 0; x <= maxX; x++)
    {
        if (paper[x, y])
        {
            Console.Write('*');
            answer1++;
        }
        else
        {
            Console.Write('.');
        }
    }
    Console.WriteLine();
}
watch.Stop();

Console.WriteLine($"Part 1: {answer1}");
Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds}ms");
