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

//int x1 = 20; int x2 = 30;
//int y1 = -10; int y2 = -5;
int x1 = 85; int x2 = 145;
int y1 = -163; int y2 = -108;
//int x1 = 352; int x2 = 377;
//int y1 = -49; int y2 = -30;

//assume target always +'ve x and -'ve y
//bounds are found by the fact that we will overshoot the target in 1 step for certain x and y values
//trivially shown for x, for y this is true because even if fired upwards it will always return to the y=0 point with a -'ve velocity 1 higher than its initial velocity (energy conservation?!)
//also a better x-bound possible given that for low v it will never reach the target before going vertical
(int minvx, int maxvx) = (0, x2);
(int minvy, int maxvy) = (y1, Math.Abs(y1));

Dictionary<int, List<int>> validStepsForY = new Dictionary<int, List<int>>();
for (int vy = minvy; vy <= maxvy; vy++)
{
    int y = 0;
    int cvy = vy;
    for (int step = 1; y >= y1; step++)
    {
        y += cvy;
        cvy -= 1;

        if (y >= y1 && y <= y2)
        {
            if (!validStepsForY.ContainsKey(step))
            {
                validStepsForY.Add(step, new List<int>());
            }
            validStepsForY[step].Add(vy);
        }
    }
}

var maxYStep = validStepsForY.Select(kvp => kvp.Key).Max();

Dictionary<int, List<int>> validStepsForX = new Dictionary<int, List<int>>();
for (int vx = minvx; vx <= maxvx; vx++)
{
    int x = 0;
    int cvx = vx;
    for (int step = 1; x <= x2 && step <= maxYStep; step++)
    {
        x += cvx;
        if (cvx > 0) cvx -= 1;

        if (x >= x1 && x <= x2)
        {
            if (!validStepsForX.ContainsKey(step))
            {
                validStepsForX.Add(step, new List<int>());
            }
            validStepsForX[step].Add(vx);
        }
    }
}

HashSet<(int, int)> solutions = new HashSet<(int, int)>();
for (int step = 0; step <= maxYStep; step++)
{
    if (validStepsForX.ContainsKey(step) && validStepsForY.ContainsKey(step))
    {
        foreach (var tp in
            from x in validStepsForX[step]
            from y in validStepsForY[step]
            select (x, y))
        {
            solutions.Add(tp);
        }
    }
}


var maxYV = solutions.Select(kvp => kvp.Item2).Max();
var answer1 = maxYV * (maxYV + 1) / 2;
var sorted = solutions.OrderBy(tp => tp.Item1).ThenBy(tp => tp.Item2).ToList();
int answer2 = sorted.Count;

watch.Stop();
Console.WriteLine($"Part1: {answer1}");
Console.WriteLine($"Part2: {answer2}");
Console.WriteLine($"Took: {watch.ElapsedMilliseconds}ms");
