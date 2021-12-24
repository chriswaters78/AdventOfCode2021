// See https://aka.ms/new-console-template for more information
var lines = File.ReadAllLines(args[0]);
var steps = new List<(bool on, int x1, int x2, int y1, int y2, int z1, int z2)>();

foreach (var line in lines)
{
    var sp = line.Split(' ');
    var sp2 = sp[1].Split(',').Select(str => str.Substring(2).Split("..").Select(int.Parse).ToArray()).ToArray();
    steps.Add((sp[0] == "on", sp2[0][0], sp2[0][1], sp2[1][0], sp2[1][1], sp2[2][0], sp2[2][1]));
}

int bounds = 50;
int total = 0;
List<(int, int, int)> cubes = new List<(int, int, int)>();
for (int x = -bounds; x <= bounds; x++)
{
    for (int y = -bounds; y <= bounds; y++)
    {
        for (int z = -bounds; z <= bounds; z++)
        {
            bool on = false;
            foreach (var step in steps)
            {
                if (x >= step.x1 && x <= step.x2 && y >= step.y1 && y <= step.y2 && z >= step.z1 && z <= step.z2)
                {
                    on = step.on;
                }
            }

            if (on)
            {
                cubes.Add((x, y, z));
            }
        }
    }
}

Console.WriteLine($"Part 1: {cubes.Count}");