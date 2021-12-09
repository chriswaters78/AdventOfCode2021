using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent9a
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(args[0]).Select(str => str.Select(c => int.Parse(c.ToString())).ToList()).ToList();
            var lineLength = input[0].Count;
            var noLines = input.Count;
            input = new[] { Enumerable.Repeat(int.MaxValue, lineLength + 2) }.Concat(
                input.Select(line => new int[] { int.MaxValue }
                .Concat(line)
                .Concat(new[] { int.MaxValue })))
                .Concat(new[] { Enumerable.Repeat(int.MaxValue, lineLength + 2) }).Select(ie => ie.ToList()).ToList();

            List<int> lowPoints = new List<int>();
            for (int i = 1; i < noLines; i++)
            {
                for (int j = 1; j < lineLength; j++)
                {
                    if (input[i-1][j-1] > input[i][j]
                        && input[i + 1][j - 1] > input[i][j]
                        && input[i - 1][j + 1] > input[i][j]
                        && input[i + 1][j + 1] > input[i][j])
                    {
                        lowPoints.Add(input[i][j]);
                    }
                }
            }

            var answer = lowPoints.Sum(i => i + 1);
        }
    }
}
