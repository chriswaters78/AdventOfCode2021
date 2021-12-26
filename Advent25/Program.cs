using System.Text;

char[][] grid = File.ReadAllLines(args[0]).Select(str => str.ToArray()).ToArray();

int R = grid.Length;
int C = grid.First().Length;

int step;
for (step = 1; step < int.MaxValue; step++)
{
    bool moved = false;
    var grid2 = grid.Select(arr => arr.ToArray()).ToArray();
    //try move right
    for (int r = 0; r < R; r++)
    {
        for (int c = 0; c < C; c++)
        {
            if (grid[r][c] == '>' && grid[r][(c + 1) % C] == '.')
            {
                //can move
                grid2[r][(c + 1) % C] = '>';
                grid2[r][c] = '.';
                moved = true;
            }
        }
    }
    grid = grid2;
    grid2 = grid.Select(arr => arr.ToArray()).ToArray();
    //down
    for (int r = 0; r < R; r++)
    {
        for (int c = 0; c < C; c++)
        {
            if (grid[r][c] == 'v' && grid[(r + 1) % R][c] == '.')
            {
                grid2[(r + 1) % R][c] = 'v';
                grid2[r][c] = '.';
                moved = true;
            }
        }
    }

    //Console.WriteLine($"Step {step}");
    //Console.WriteLine(print(grid2));

    if (!moved)
    {
        break;
    }
    grid = grid2;
}

Console.WriteLine($"Part1: {step}");
string print(char[][] grid)
{
    StringBuilder builder = new StringBuilder();
    foreach (var row in grid)
    {
        foreach (var ch in row)
        {
            builder.Append(ch);
        }
        builder.AppendLine();
    }

    return builder.ToString();
}