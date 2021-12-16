using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

Stopwatch watch = new Stopwatch();
watch.Start();

var pattern = File.ReadAllLines(args[0]).Select(line => line.Select(ch => int.Parse(ch.ToString())).ToArray()).ToArray();

int yL = pattern.Length;
int xL = pattern.First().Length;

var riskLevels = new int[yL * 5][];
for (int row = 0; row < yL * 5; row++)
{
    riskLevels[row] = new int[xL * 5];
    for (int col = 0; col < xL * 5; col++)
    {
        var xI = col / xL;
        var yI = row / yL;

        var level = (pattern[row % yL][col % xL] + xI + yI);
        if (level > 9)
        {
            riskLevels[row][col] = level % 10 + 1;
        }
        else
        {
            riskLevels[row][col] = level;
        }
    }
}

var costs = new int[yL * 5][];
for (int row = 0; row < yL * 5; row++)
{
    costs[row] = Enumerable.Repeat(int.MaxValue, xL * 5).ToArray();
}

costs[0][0] = 0;

//Queue<(int, int)> queue = new Queue<(int, int)>();
PriorityQueue<(int, int), int> queue = new PriorityQueue<(int, int), int>();
queue.Enqueue((1, 0), 0);
queue.Enqueue((0, 1), 0);

while (queue.Count > 0)
{
    var (x,y) = queue.Dequeue();
    var neighbours = new[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) };//.Where(tp => tp.Item1 >= 0 && tp.Item1 < xL * 5 && tp.Item2 >= 0 && tp.Item2 < yL * 5);

    var best = int.MaxValue;
    foreach ((int p, int q) in neighbours)
    {
        if (p >= 0 && p < xL * 5 && q >= 0 && q < yL * 5 && costs[p][q] != int.MaxValue)
        {
            if (costs[p][q] + riskLevels[x][y] < best)
            {
                best = costs[p][q] + riskLevels[x][y];
            }
        }
    }

    //var best = neighbours.Where(tp => costs[tp.Item1][tp.Item2] != int.MaxValue).Select(tp => costs[tp.Item1][tp.Item2] + riskLevels[x][y]).Min();
    if (best < costs[x][y])
    {
        costs[x][y] = best;
        //its a new possible best path to this node, so carry on searching
        foreach ((int p, int q) in neighbours)
        {
            if (p >= 0 && p < xL * 5 && q >= 0 && q < yL * 5)
            {
                queue.Enqueue((p,q), best);
            }
        }
    }
}

var answer = costs[yL * 5 - 1][yL * 5 - 1];
watch.Stop();

Console.WriteLine($"Part 1: {answer}");
Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds}ms");