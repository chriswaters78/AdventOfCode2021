using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2a
{
    class Program
    {
        static void Main(string[] args)
        {
            List<(string, int)> instructions = File.ReadAllLines(args[0]).Select(line => line.Split(' ')).Select(arr => (arr[0], int.Parse(arr[1]))).ToList();

            int hor = 0;
            int depth = 0;
            foreach (var (dir, value) in instructions)
            {
                switch (dir)
                {
                    case "forward":
                        hor += value;
                        break;
                    case "down":
                        depth += value;
                        break;
                    case "up":
                        depth -= value;
                        break;
                }
            }

            var result = hor * depth;
        }
    }
}
