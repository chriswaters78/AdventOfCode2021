using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

Stopwatch watch = new Stopwatch();
watch.Start();

var grid = new[] { Enumerable.Repeat(int.MinValue, 12).ToArray() }
            .Concat(    File.ReadAllLines(args[0]).Select(
                            line => new[] { int.MinValue }
                        .Concat(
                            line.Select(c => int.Parse(c.ToString())))
                        .Concat(new[] { int.MinValue }).ToArray()))
           .Concat(     new[] { Enumerable.Repeat(int.MinValue, 12).ToArray() })
           .ToArray();

Queue<(int, int)> flashes = new Queue<(int, int)>();

int answer1 = 0;
int answer2 = -1;
for (int step = 1; step <= int.MaxValue; step++)
{
    for (int x = 1; x <= 10; x++)
        for (int y = 1; y <= 10; y++)
        {
            flashes.Enqueue((x, y));
        }


    while (flashes.Count > 0)
    {
        var flash = flashes.Dequeue();
        if (grid[flash.Item1][flash.Item2] == 9)
        {
            //its triggered a flash
            flashes.Enqueue((flash.Item1 - 1, flash.Item2 - 1));
            flashes.Enqueue((flash.Item1 - 1, flash.Item2));
            flashes.Enqueue((flash.Item1 - 1, flash.Item2 + 1));
            flashes.Enqueue((flash.Item1, flash.Item2 - 1));
            flashes.Enqueue((flash.Item1, flash.Item2 + 1));
            flashes.Enqueue((flash.Item1 + 1, flash.Item2 - 1));
            flashes.Enqueue((flash.Item1 + 1, flash.Item2));
            flashes.Enqueue((flash.Item1 + 1, flash.Item2 + 1));

            answer1 += 1;            
        }
        
        grid[flash.Item1][flash.Item2] += 1;
    }

    bool allFlashed = true;
    for (int x = 1; x <= 10; x++)
        for (int y = 1; y <= 10; y++)
        {
            if (grid[x][y] > 9)
            {
                grid[x][y] = 0;
            }
            else
            {
                allFlashed = false;
            }
        }

    if (allFlashed)
    {
        answer2 = step;
        break;
    }
}

watch.Stop();
Console.WriteLine($"Part 1: {answer1}");
Console.WriteLine($"Part 2: {answer2}");
Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds}ms");