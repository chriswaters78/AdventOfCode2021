using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2b
{
    class Program
    {
        static void Main(string[] args)
        {
            List<(string, int)> instructions = File.ReadAllLines(args[0]).Select(line => line.Split(' ')).Select(arr => (arr[0], int.Parse(arr[1]))).ToList();

            int hor = 0, depth = 0, aim = 0;
            foreach (var (dir, value) in instructions)
            {
                switch (dir)
                {
                    case "forward":
                        hor += value;
                        depth += aim * value;
                        break;
                    case "down":
                        aim += value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                }
            }

            var result = hor * depth;
        }
    }
}
