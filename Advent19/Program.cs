var lines = File.ReadAllLines(args[0]).Where(line => !line.StartsWith("--- scanner"));
var beacons = new List<List<(int x, int y, int z)>>();
var curr = new List<(int x, int y, int z)>();
beacons.Add(curr);

List<int[][]> ALLROTS = allRots();

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

int maxCount = 0;
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
                        Console.WriteLine($"Scanner {s1} has {count} matches to scanner {s2} when using delta {dx},{dy},{dz}");
                    }
                }
            }
        }
    }
}

Console.WriteLine("");



List<int[][]> allRots() {
    var aa = new List<int[][]>() {
    new int[3][]    {
        new int[] { 1, 0, 0 },
        new int[] { 0, 1, 0 },
        new int[3] { 0, 0, 1 }},
    new int[3][]    {
        new int[] { 0, 1, 0 },
        new int[] { 0, 0, 1 },
        new int[3] { 1, 0, 0 }},
    new int[3][]    {
        new int[] { 0, 0, 1 },
        new int[] { 1, 0, 0 },
        new int[3] { 0, 1, 0 }},
    };

        var bb = new List<int[][]>() {
        new int[3][]    {
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[3] { 0, 0, 1 }},
        new int[3][]    {
            new int[] { -1, 0, 0 },
            new int[] { 0, -1, 0 },
            new int[3] { 0, 0, 1 }},
        new int[3][]    {
            new int[] { -1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[3] { 0, 0, -1 }},
        new int[3][]    {
            new int[] { 1, 0, 0 },
            new int[] { 0, -1, 0 },
            new int[3] { 0, 0, -1 }},
    };

        var cc = new List<int[][]>() {
        new int[3][]    {
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[3] { 0, 0, 1 }},
        new int[3][]    {
            new int[] { 0, 0, -1 },
            new int[] { 0, -1, 0 },
            new int[3] { -1, 0, 0 }},
    };

    var results = new List<int[][]>();
    foreach (var a in aa)
    {
        foreach (var b in bb)
        {
            foreach (var c in cc)
            {
                results.Add(multiply3By3(a, multiply3By3(b, c)));
            }
        }
    }
    return results;
}

(int x, int y, int z) rotatePoint(int[][] a, (int x,int y,int z) b)
{

    return (a[0][0] * b.x + a[1][0] * b.y + a[2][0] * b.z,
            a[0][1] * b.x + a[1][1] * b.y + a[2][1] * b.z,
            a[0][2] * b.x + a[1][2] * b.y + a[2][2] * b.z );
}

int[][] multiply3By3(int[][] a, int[][]b)
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
