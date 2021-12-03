using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent3a
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);
            var noBits = lines.First().Length;
            List<int> instructions = lines.Select(line => Convert.ToInt32(line, 2)).ToList();
            List<BitArray> bits = instructions.Select(i => new BitArray(new[] { i })).ToList();

            BitArray gamma = new BitArray(32);
            BitArray epsilon = new BitArray(32);
            for (var index = 0; index < noBits; index++)
            {
                var mostCommon = bits.Select(ba => ba.Get(index)).GroupBy(b => b).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                gamma.Set(index, mostCommon);
                epsilon.Set(index, !mostCommon);
            }

            int[] gammaInt = new int[1];
            gamma.CopyTo(gammaInt, 0);
            int[] epsilonInt = new int[1];
            epsilon.CopyTo(epsilonInt, 0);

            var answer = gammaInt[0] * epsilonInt[0];
        }
    }
}
