using System.Diagnostics;

Stopwatch watch = new Stopwatch();
watch.Start();

var lines = File.ReadAllLines(args[0]).Where(line => !line.StartsWith("--- scanner"));
var beacons = new List<List<(int x, int y, int z)>>();
List<int[][]> ALLROTS = allRots();

{
    var curr = new List<(int x, int y, int z)>();
    beacons.Add(curr);

    foreach (var line in lines)
    {
        if (!String.IsNullOrEmpty(line))
        {
            var split = line.Split(',');
            curr.Add((int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
        }
        else
        {
            curr = new List<(int, int, int)>();
            beacons.Add(curr);
        }
    }
}

int maxCount = 0;

var results = Enumerable.Range(0, beacons.Count).ToDictionary(
                    s1 => s1, 
                    tp => new Dictionary<int, (int[][] rot, (int dx, int dy, int dz))>());

for  (var s1 = 0; s1 < beacons.Count; s1++)
{
    for (var s2 = 0; s2 < beacons.Count; s2++)
    {
        if (s1 == s2) continue;

        foreach (var rot in ALLROTS)
        {
            foreach (var p1 in beacons[s1].Select(p => rotatePoint(rot, p)))
            {
                foreach (var p2 in beacons[s2])
                {

                    //find vector from p1 to p2
                    var (dx, dy, dz) = (p2.x - p1.x, p2.y - p1.y, p2.z - p1.z);

                    //delta every rotated point in beacons[s1]
                    //and count the matches
                    var mapped = beacons[s1].Select(p => rotatePoint(rot, p)).Select(p => (p.x += dx, p.y += dy, p.z += dz));

                    var count = mapped.Intersect(beacons[s2]).Count();
                    if (count > maxCount)
                    {
                        maxCount = count;
                    }
                    if (count >= 12)
                    {
                        results[s1].Add(s2, (rot, (dx, dy, dz)));
                        
                        //Console.WriteLine($"Scanner {s1} has {count} matches to scanner {s2} when using delta {dx},{dy},{dz}");
                        goto nextRot;
                    }
                }
            }
            nextRot:;
        }
    }
}

//need to do a dfs on the results to find the sequence of transforms that will map them all to scanner 0 coords
var mappingsToX = new Dictionary<int, Dictionary<int, List<int>>>();
//(s1,s2) => the distance from s1 to s2
 var deltas = new Dictionary<(int, int), int>();

//all points mapped to scanner 0 coordinates
HashSet<(int, int, int)> allPoints = new HashSet<(int, int, int)>(beacons[0]);

for (int s1 = 0; s1 < beacons.Count; s1++)
{
    Dictionary<int, List<int>> mappingToX = new Dictionary<int, List<int>>();
    mappingsToX[s1] = mappingToX;
    HashSet<int> seen = new HashSet<int>();
    Stack<(int, List<int>)> stack = new Stack<(int, List<int>)>();
    stack.Push((s1, new List<int>()));

    while (stack.Count > 0)
    {
        (var curr, var currPath) = stack.Pop();

        if (seen.Contains(curr))
        {
            continue;
        }
        seen.Add(curr);
        currPath = new[] { curr }.Concat(currPath).ToList();
        mappingToX.Add(curr, currPath);

        foreach (var map in results[curr])
        {
            stack.Push((map.Key, currPath));
        }
    }


    for (int s2 = 0; s2 < beacons.Count; s2++)
    {
        if (s2 == s1)
        {
            continue;
        }
        foreach (var p in beacons[s2])
        {
            var c = p;
            var d = (0,0,0);
            for (int i = 0; i < mappingsToX[s1][s2].Count - 1; i++)
            {
                var mapFrom = mappingsToX[s1][s2][i];
                var mapTo = mappingsToX[s1][s2][i + 1];

                (var rot, var delta) = results[mapFrom][mapTo];
                c = rotatePoint(rot, c);
                c = (c.x + delta.dx, c.y + delta.dy, c.z + delta.dz);
                d = rotatePoint(rot, d);
                d = (d.Item1 + delta.dx, d.Item2 + delta.dy, d.Item3 + delta.dz);
            }
            if (s1 == 0)
            {
                allPoints.Add(c);
            }


            deltas[(s1, s2)] = Math.Abs(d.Item1) + Math.Abs(d.Item2) + Math.Abs(d.Item3);
        }

    }

}

watch.Stop();
Console.WriteLine($"Part1: {allPoints.Count}");
Console.WriteLine($"Part2: {deltas.Values.Max()}");
Console.WriteLine($"Elapsed: {watch.ElapsedMilliseconds}ms");



List<int[][]> allRots() {
    var aa = new List<int[][]>() {
        new int[][] {
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[] { 0, 0, 1 }},
        new int[][] {
            new int[] { 0, 1, 0 },
            new int[] { 0, 0, 1 },
            new int[] { 1, 0, 0 }},
        new int[][] {
            new int[] { 0, 0, 1 },
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 }},
    };

    var bb = new List<int[][]>() {
        new int[][] {
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[] { 0, 0, 1 }},
        new int[][]    {
            new int[] { -1, 0, 0 },
            new int[] { 0, -1, 0 },
            new int[] { 0, 0, 1 }},
        new int[][] {
            new int[] { -1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[] { 0, 0, -1 }},
        new int[][] {
            new int[] { 1, 0, 0 },
            new int[] { 0, -1, 0 },
            new int[] { 0, 0, -1 }},
    };

    var cc = new List<int[][]>() {
        new int[3][]    {
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[] { 0, 0, 1 }},
        new int[3][]    {
            new int[] { 0, 0, -1 },
            new int[] { 0, -1, 0 },
            new int[] { -1, 0, 0 }},
    };

    return (from a in aa
           from b in bb
           from c in cc
           select multiply3By3(a, multiply3By3(b, c))).ToList();
}

(int x, int y, int z) rotatePoint(int[][] a, (int x,int y,int z) b)
{
    return (a[0][0] * b.x + a[1][0] * b.y + a[2][0] * b.z,
            a[0][1] * b.x + a[1][1] * b.y + a[2][1] * b.z,
            a[0][2] * b.x + a[1][2] * b.y + a[2][2] * b.z );
}

int[][] multiply3By3(int[][] a, int[][] b)
{
    int[][] result = new int[3][]
    {
        new int[3],
        new int[3],
        new int[3],
    };

    result[0][0] = a[0][0] * b[0][0] + a[1][0] * b[0][1] + a[2][0] * b[0][2];
    result[1][0] = a[0][0] * b[1][0] + a[1][0] * b[1][1] + a[2][0] * b[1][2];
    result[2][0] = a[0][0] * b[2][0] + a[1][0] * b[2][1] + a[2][0] * b[2][2];

    result[0][1] = a[0][1] * b[0][0] + a[1][1] * b[0][1] + a[2][1] * b[0][2];
    result[1][1] = a[0][1] * b[1][0] + a[1][1] * b[1][1] + a[2][1] * b[1][2];
    result[2][1] = a[0][1] * b[2][0] + a[1][1] * b[2][1] + a[2][1] * b[2][2];

    result[0][2] = a[0][2] * b[0][0] + a[1][2] * b[0][1] + a[2][2] * b[0][2];
    result[1][2] = a[0][2] * b[1][0] + a[1][2] * b[1][1] + a[2][2] * b[1][2];
    result[2][2] = a[0][2] * b[2][0] + a[1][2] * b[2][1] + a[2][2] * b[2][2];

    return result;
}
