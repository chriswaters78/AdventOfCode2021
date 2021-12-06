using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Advent5a
{
    class Program
    {
        static void Main(string[] args)
        {
            const int size = 1000;
            var lines = File.ReadAllLines(args[0]).Select(str => str.Replace(" -> ", ",").Split(',').Select(int.Parse).ToList()).ToList();

            var grid = new int[size, size];

            foreach (var line in lines)
            {
                int x0 = line[0];
                int y0 = line[1];
                int x1 = line[2];
                int y1 = line[3];

                if (y0 == y1)
                {
                    //horiz line
                    for (int x = x0 > x1 ? x1 : x0; x <= (x0 > x1 ? x0 : x1); x++)
                    {
                        grid[x, y0] = ++grid[x, y0];
                    }
                }
                else if (x0 == x1)
                {
                    //vert line
                    for (int y = y0 > y1 ? y1 : y0; y <= (y0 > y1 ? y0 : y1); y++)
                    {
                        grid[x0, y] = ++grid[x0, y];
                    }
                }
                else
                {   //strictly diagonal 
                    int xMag = x0 > x1 ? -1 : 1;
                    int yMag = y0 > y1 ? -1 : 1;
                    int x = x0;
                    int y = y0;
                    for (int i = 0; i <= Math.Abs(y1 - y0); i++)
                    {
                        grid[x, y] = ++grid[x, y];
                        x += xMag;
                        y += yMag;
                    }
                }
            }

            int count = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (grid[x,y] >= 2)
                    {
                        count++;
                    }
                }
            }
        }
    }
}
