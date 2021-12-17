using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

Stopwatch watch = new Stopwatch();
watch.Start();

int x1 = 85;
int x2 = 145;

int y1 = -163;
int y2 = -108;

int minvx = 0;
int maxvx = x2 + 1;

int minvy = y1 > 0 && y2 > 0 ? 0 : y1 - 1;
int maxvy = y1 > 0 && y2 > 0 ? Math.Abs(y2 + 1) : Math.Abs(y1 - 1);

int answer1 = int.MinValue;
List<(int, int)> everyV = new List<(int, int)>();
for (int vx = minvx; vx <= maxvx; vx++)
{
    for (int vy = minvy; vy <= maxvy; vy++)
    {
        int maxy = 0;
        int x = 0;
        int y = 0;
        bool done = false;
        for (int step = 1; x <= x2 && y >= y1; step++)
        {
            x = step <= vx ? step * (2 * vx - (step - 1)) / 2 : x;
            y = step * (2 * vy - (step - 1)) / 2;

            maxy = y > maxy ? y : maxy;
            if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
            {
                if (!done)
                {
                    everyV.Add((vx, vy));
                    done = true;
                }
                if (maxy > answer1)
                {
                    answer1 = maxy;
                }
            }
        }
    }
}

watch.Stop();
Console.WriteLine($"Part1: {answer1}");
Console.WriteLine($"Part2: {everyV.Count}");
Console.WriteLine($"Took: {watch.ElapsedMilliseconds}ms");
