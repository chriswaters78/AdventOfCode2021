using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent3b
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);
            var noBits = lines.First().Length;
            List<int> instructions = lines.Select(line => Convert.ToInt32(line, 2)).ToList();
            List<BitArray> bits = instructions.Select(i => new BitArray(new[] { i })).ToList();

            List<BitArray> oxySearch = new List<BitArray>(bits);
            List<BitArray> co2Search = new List<BitArray>(bits);
            for (var index = noBits - 1; index >= 0; index--)
            {
                if (oxySearch.Count > 1)
                {
                    var mostCommon = oxySearch.Select(ba => ba.Get(index)).GroupBy(b => b).OrderByDescending(grp => grp.Count()).ThenBy(grp => !grp.Key).Select(grp => grp.Key).First();                    
                    oxySearch = oxySearch.Where(ba => ba.Get(index) == mostCommon).ToList();
                }
                if (co2Search.Count > 1)
                {
                    var mostCommon = co2Search.Select(ba => ba.Get(index)).GroupBy(b => b).OrderByDescending(grp => grp.Count()).ThenBy(grp => !grp.Key).Select(grp => grp.Key).First();
                    co2Search = co2Search.Where(ba => ba.Get(index) == !mostCommon).ToList();
                }
            }

            if (oxySearch.Count > 1 || co2Search.Count > 1)
            {
                throw new Exception("BOOM");
            }

            int[] oxyInt = new int[1];
            oxySearch.Single().CopyTo(oxyInt, 0);
            int[] co2Int = new int[1];
            co2Search.Single().CopyTo(co2Int, 0);

            var answer = oxyInt[0] * co2Int[0];
        }
    }
}
