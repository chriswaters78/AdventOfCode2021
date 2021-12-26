using System.Diagnostics;
using System.Text;

Stopwatch watch = new Stopwatch();
watch.Start();

var steps = File.ReadAllLines(args[0]).ToArray();
var ps = new (int, int, int)[14];
for (int i = 0; i < 14; i++)
{
    ps[i] = (int.Parse(new String(steps[4 + i * 18]).Skip(6).ToArray()), int.Parse(new String(steps[5 + i * 18].Skip(6).ToArray())), int.Parse(new String(steps[15 + i * 18].Skip(6).ToArray())));
}

(long? lowest, long? highest) = (long.MaxValue, long.MinValue);
for (long i = 10000000000000; i <= 99999999999999; i++)
{
    var digits = i.ToString().Select(ch => int.Parse(ch.ToString())).ToArray();
    int step = 0;
    long z = 0;

    foreach ((int p1, int p2, int p3) in ps)
    {
        var w = digits[step];
        var test = (z % 26) + p2 == w;
        if (w != 0 && p1 == 26 && test)
        {
            z = z / p1;
        }
        else if (w != 0 && p1 == 1 && !test)
        {
            z = 26 * (z / p1) + w + p3;
        }
        else
        {
            //e.g. 234560000 to 234569999
            i += (long) Math.Pow(10, 13 - step);
            i--;
            break;
        }
        step++;
    }

    if (z == 0)
    {
        (lowest, highest) = (i < lowest ? i : lowest, i > highest ? i : highest);
    }
}

Console.WriteLine($"Part1: {highest}");
Console.WriteLine($"Part2: {lowest}");

string print(int[] arr)
{
    StringBuilder builder = new StringBuilder();
    foreach (var i in arr)
    {
        builder.Append(i);
    }

    return builder.ToString();
}