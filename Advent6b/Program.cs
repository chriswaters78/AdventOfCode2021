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
            var fishies = File.ReadAllLines(args[0]).Single().Split(',').Select(int.Parse).GroupBy(i => i).ToDictionary(grp => grp.Key, grp => (long) grp.Count());

            for (int age = 0; age <= 8; age++)
            {
                if (!fishies.ContainsKey(age))
                {
                    fishies[age] = 0;
                }
            }

            for (int day = 0; day < 256; day++)
            {
                Dictionary<int, long> newFishies = Enumerable.Range(0, 9).ToDictionary(i => i, _ => (long) 0);
                foreach (var (age, count) in fishies)
                {
                    if (age == 0)
                    {
                        newFishies[8] = newFishies[8] + count;
                        newFishies[6] = newFishies[6] + count;
                    }
                    else if (count > 0)
                    {
                        newFishies[age - 1] = newFishies[age-1] + count;
                    }
                }

                fishies = newFishies;
            }

            var answer = fishies.Select(kvp => kvp.Value).Sum();
        }
    }
}
