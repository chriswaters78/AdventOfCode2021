using System.Collections;
using System.Diagnostics;
using System.Text;

Stopwatch watch = new Stopwatch();
watch.Start();

int STEPS = int.Parse(args[1]);
var pattern = new BitArray(File.ReadAllLines(args[0]).First().Select(ch => ch == '#').ToArray());
var grid = File.ReadAllLines(args[0]).Skip(2).ToArray();

var points = (from r in Enumerable.Range(-1, grid.Length + 2)
              from c in Enumerable.Range(-1, grid.First().Length + 2)
              select ((r, c), r >= 0 && r < grid.Length && c >= 0 && c < grid.First().Length ? grid[r][c] == '#' : false))
              .ToDictionary(tp => tp.Item1, tp => tp.Item2);

print(points);
(var maxr, var maxc) = (grid.Length + 2, grid.First().Length + 2);

for (int step = 1; step <= STEPS; step++)
{
    var newPoints = new Dictionary<(int, int), bool>();
    for (int r = -step; r < maxr; r++)
    {
        for (int c = -step; c < maxc; c++)
        {
            int pi = 0;
            for (int ro = -1; ro <= 1; ro++)
            {
                for (int co = -1; co <= 1; co++)
                {
                    var s = (1 - ro) * 3 + (1 - co);
                    var p = (r + ro, c + co);
                    pi += points.ContainsKey(p) ?
                            points[p] ? 1 << s : 0
                            : step % 2 == 0 && pattern[0] && !pattern[511] ? 1 << s : 0;
                }
            }

            newPoints.Add((r, c), pattern[pi]);
        }
    }
    (maxr, maxc) = (++maxr, ++maxc);
    points = newPoints;
}

watch.Stop();
print(points);

Console.WriteLine($"Answer for step {STEPS}: {points.Where(kvp => kvp.Value).Count()}");
Console.WriteLine($"Elapsed time: {watch.ElapsedMilliseconds}ms");

void print(Dictionary<(int,int), bool> points)
{
    (var minC, var maxC) = (points.Keys.Min(tp => tp.Item2), points.Keys.Max(tp => tp.Item2));
    (var minR, var maxR) = (points.Keys.Min(tp => tp.Item1), points.Keys.Max(tp => tp.Item1));
    StringBuilder builder = new StringBuilder();
    for (int c = minC; c < maxC; c++)
    {
        for (int r = minR; r < maxR; r++)
        {
            builder.Append(points[(c, r)] ? '#' : '.');
        }
        builder.AppendLine();
    }
    Console.WriteLine(builder.ToString());
}