using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent6a
{
    class Program
    {
        static void Main(string[] args)
        {
            var fishies = File.ReadAllLines(args[0]).Single().Split(',').Select(int.Parse).ToList();

            for (int day = 0; day < 80; day++)
            {
                List<int> newFish = new List<int>();
                foreach (var fish in fishies)
                {
                    if (fish == 0)
                    {
                        newFish.Add(8);
                        newFish.Add(6);
                    }
                    else
                    {
                        newFish.Add(fish - 1);
                    }
                }
                fishies = newFish;
            }
        }
    }
}
