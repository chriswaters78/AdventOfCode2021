using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Advent7a
{
    class Program
    {
        static void Main(string[] args)
        {
            var positions = File.ReadAllLines(args[0]).Single().Split(',').Select(int.Parse).OrderBy(i => i).ToList();

            int bestCost = int.MaxValue;
            int bestPosition = -1;
            var costs = preCalcCosts();
            for (int position  = 0; position < positions.Count; position++)
            {
                int cost = 0;
                foreach (var p in positions)
                {
                    //cost += costs[Math.Abs(p - position)];
                    cost += Math.Abs(p - position);
                }

                if (cost < bestCost)
                {
                    bestCost = cost;
                    bestPosition = position;
                }
            }
        }

        private static Dictionary<int,int> preCalcCosts()
        {
            Dictionary<int, int> costs = new Dictionary<int, int>();
            costs[0] = 0;
            for (int i = 1; i < 2000; i++)
            {
                costs[i] = costs[i-1] + i;
            }

            return costs;
        }
    }
}
