using System.Diagnostics;
using System.Text;

Stopwatch watch = new Stopwatch();
watch.Start();

var lines = File.ReadAllLines(args[0]).Where(line => !line.StartsWith("//"));
var steps = new LinkedList<(bool on, int[] bounds)>();
foreach (var line in lines)
{
    var sp = line.Split(' ');
    var sp2 = sp[1].Split(',').Select(str => str.Substring(2).Split("..").Select(int.Parse).ToArray()).ToArray();
    //x1, x2, y1, y2, z1, z2
    steps.AddLast((sp[0] == "on", new[] { sp2[0][0], sp2[0][1], sp2[1][0], sp2[1][1], sp2[2][0], sp2[2][1] }));
}

var test1 = (true, new int[] { -22, 1, 10, 10, -24, 15 });
var test2 = (true, new int[] { -33, 1, 10, 10, -24, 15 });

var step1 = steps.First;

int s1c = 0;
while (true)
{
    bool anyIntersects = false;
    while (step1 != null)
    {
        int s2c = 0;
        s1c++;
        //if (step1.Value.bounds.SequenceEqual(test1.Item2))
        //{
        //    Console.WriteLine("Hit test1");
        //}
        var step2 = steps.First;
        while (step2 != null)
        {
            s2c++;
            if (step2 == step1)
            {
                step2 = step2.Next;
                continue;
            }

            //if (step2.Value.bounds.SequenceEqual(test2.Item2))
            //{
            //    Console.WriteLine("Hit test");
            //}

            bool intersects = true;
            for (int dim = 0; dim < 3; dim++)
            {
                if (!(step2.Value.bounds[dim * 2] <= step1.Value.bounds[dim * 2 + 1] && step2.Value.bounds[dim * 2 + 1] >= step1.Value.bounds[dim * 2]))
                {
                    intersects &= false;
                }
            }

            bool decomposed = false;
            if (intersects)
            {
                for (int dim = 0; dim < 3; dim++)
                {
                    for (int hand = 0; hand < 2; hand++)
                    {
                        var k = step1.Value.bounds[dim * 2 + hand];
                        var p1 = step2.Value.bounds[dim * 2];
                        var p2 = step2.Value.bounds[dim * 2 + 1];
                        if (hand == 0 && p1 < k && k <= p2)
                        {
                            decomposed = true;
                        }
                        if (hand == 1 && p1 <= k && k < p2)
                        {
                            decomposed = true;
                        }

                        if (decomposed)
                        {
                            anyIntersects |= decomposed;
                            var newStep = (step2.Value.on, step2.Value.bounds.ToArray());

                            step2.Value.bounds[dim * 2 + 1] = k + (hand == 0 ? -1 : 0);
                            newStep.Item2[dim * 2] = k + (hand == 0 ? 0 : 1);

                            if (step2.Value.bounds[dim * 2] > step2.Value.bounds[dim * 2 + 1] || newStep.Item2[dim * 2] > newStep.Item2[dim * 2 + 1])
                            {
                                throw new Exception("BOOM");
                            }
                            steps.AddAfter(step2, newStep);
                            goto skip;
                        }
                    }
                }
            }

        skip:;
            //loop until no more decompositions
            if (!intersects)
            {
                step2 = step2.Next;
            }
            if (intersects && !decomposed)
            {
                step2 = step2.Next;
            }

        }

        step1 = step1.Next; 
    }

    //issue is that if we have intersected s2 with s1, we could have re-inserted s2 decompositions before s1
    //however those inserted s2 may be further decomposed by s1s that we are already past
    //If we reset step1 to first it solves it but is too slow
    //we need to do a two way intersection between s1 and s2, then should work?

    //OR STORE EVERYTHING IN A RANGE TREE
    //FOR st * st, check if intersects
    //if so intersect with each other
    if (anyIntersects)
    {
        Console.WriteLine($"Steps: {steps.Count} in {watch.ElapsedMilliseconds}ms");
        step1 = steps.First;
    }
    else
    {
        break;
    }
}


Console.WriteLine($"Summing cubes...");

var lastOnCubes = steps.GroupBy(tp => (tp.bounds[0], tp.bounds[1], tp.bounds[2], tp.bounds[3], tp.bounds[4], tp.bounds[5])).Where(grp => grp.Last().Item1).Select(grp => grp.Key);

Console.WriteLine("FINAL STEPS:");
//var printed = steps.Select(tp => tp.bounds).Select(print);
//foreach (var line in printed)
//{
//    Console.WriteLine(line);
//}


var answer = lastOnCubes.Sum(tp => (1L + tp.Item2 - tp.Item1) * (1L + tp.Item4 - tp.Item3) * (1L + tp.Item6 - tp.Item5));
Console.WriteLine($"Total count: {answer} in {watch.ElapsedMilliseconds}ms");


string print (int[] arr)
{
    return arr.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i).Append(", ")).ToString();
}