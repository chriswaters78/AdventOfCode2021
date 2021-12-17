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

//int x1 = 85; int x2 = 145;
//int y1 = -163; int y2 = -108;
//int x1 = 352; int x2 = 377;
//int y1 = -49; int y2 = -30;
int x1 = 10000; int x2 = 11000;
int y1 = -5000; int y2 = -4900;

//assume target always +'ve x and -'ve y
//bounds are found by the fact that we will overshoot the target in 1 step for certain x and y values
//trivially shown for x, for y this is true because even if fired upwards it will always return to the y=0 point with a -'ve velocity 1 higher than its initial velocity (energy conservation?!)
//also a better x-bound possible given that for low v it will never reach the target before going vertical
(int minvx, int maxvx) = (0, x2);
(int minvy, int maxvy) = (y1 , Math.Abs(y1));

int answer1 = int.MinValue;
int answer2 = 0;
//could solve vx and vy seperately to find the step counts for which the projectile is in the target x or y range
//then all valid solutions are overlaps of these for which there are fast algorithms
//however brute force is easily enough for the input range
for (int vx = minvx; vx <= maxvx; vx++)
{
    for (int vy = minvy; vy <= maxvy; vy++)
    {
        int maxy = 0;
        int x = 0; int y = 0;
        for (int step = 1; x <= x2 && y >= y1; step++)
        {
            //sum of arithmetic progression = n[2 * a - (n-1)]/2
            x = step <= vx ? step * (2 * vx - (step - 1)) / 2 : x;
            y = step * (2 * vy - (step - 1)) / 2;

            maxy = y > maxy ? y : maxy;
            if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
            {
                answer2++;
                answer1 = maxy > answer1 ? maxy : answer1;
                //can only break because target is assumed to be -'ve y, so we have already hit our max height if we are in the target
                break;
            }
        }
    }
}

watch.Stop();
Console.WriteLine($"Part1: {answer1}");
Console.WriteLine($"Part2: {answer2}");
Console.WriteLine($"Took: {watch.ElapsedMilliseconds}ms");
