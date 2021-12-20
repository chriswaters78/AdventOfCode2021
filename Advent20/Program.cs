using System.Collections;
using System.Text;

const int expandBy = 10;

var lines = File.ReadAllLines(args[0]);
var pattern = new BitArray(lines[0].Select(ch => ch == '#').ToArray());

var grid = lines.Skip(2).Select(line => line.Select(ch => ch == '#').ToArray()).ToArray();
Dictionary<(int, int), bool> points = new Dictionary<(int, int), bool>();
for (int r = -expandBy; r < grid.Length + expandBy; r++)
{
    for (int c = -expandBy; c < grid.First().Length + expandBy; c++)
    {
        if (r >= 0 && r < grid.Length && c >= 0 && c < grid.First().Length)
        {
            points.Add((r, c), grid[r][c]);
        }
        else
        {
            points.Add((r, c), false);
        }
    }
}

print(points);

for (int step = 0; step < 2; step++)
{
    var newPoints = new Dictionary<(int, int), bool>();
    for (int r = -expandBy; r < grid.Length + expandBy; r++)
    {
        for (int c = -expandBy; c < grid.First().Length + expandBy; c++)
        {
            int pi = 0;
            var indexes = new[] {
                ((r-1,c-1),8),
                ((r-1,c),7),
                ((r-1,c+1),6),
                ((r,c-1),5),
                ((r,c),4),
                ((r,c+1),3),
                ((r+1,c-1),2),
                ((r+1,c),1),
                ((r+1,c+1),0),
            };

            foreach (var (index, s) in indexes)
            {
                if (points.ContainsKey(index) )
                {
                    if (points[index])
                    {
                        pi += 1 << s;
                    }
                }
                else
                {
                    if (step % 2 == 1)
                    {
                        pi += 1 << s;
                    }
                }
            }


            newPoints.Add((r, c), pattern[pi]);
        }
    }

    points = newPoints;
    print(points);
}
var answer1 = points.Where(kvp => kvp.Value).Count();

Console.WriteLine($"Part1: {answer1}");

var falsePos = points.Where(kvp => kvp.Key.Item1 == -expandBy && kvp.Value).Count();
Console.WriteLine($"False Positives: {falsePos}");
Console.WriteLine($"Real answer: {answer1 - falsePos}");


void print(Dictionary<(int,int), bool> points)
{
    var minC = points.Keys.Select(tp => tp.Item2).Min();
    var maxC = points.Keys.Select(tp => tp.Item2).Max();
    var minR = points.Keys.Select(tp => tp.Item1).Min();
    var maxR = points.Keys.Select(tp => tp.Item1).Max();
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