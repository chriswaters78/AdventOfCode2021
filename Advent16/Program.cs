using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent16
{
    class Program
    {
        static long versionTotal = 0;

        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var hex = File.ReadAllText(args[0]);
            var bits = new bool[hex.Length * 4];

            int c = 0;
            foreach (var ch in hex)
            {
                var i = Convert.ToInt32(ch.ToString(), 16);
                var binary = Convert.ToString(i, 2).PadLeft(4,'0');
                foreach (var chb in binary)
                {
                    bits[c] = chb == '1' ? true : false;
                    c += 1;
                }
            }

            int cps = 0;

            while (true)
            {
                (int cps2, long val2) = getNextPacket(bits[0..]);
                cps += cps2;

                if (cps >= hex.Length - 3 || (!bits[cps] && !bits[cps + 1] && !bits[cps + 1]))
                {
                    //no more packets
                    break;
                }
            }


            watch.Stop();



            Console.WriteLine($"Part 1: {0}");
            Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds}ms");
        }

        private static (int bitsConsumed, long value) getNextPacket(bool[] bits)
        {
            long value = -1;
            int cps = 0;
            //first 3 bits are version
            long version = getIntFromArray(bits[cps..(cps + 3)]);
            versionTotal += version;

            cps += 3;

            long type = getIntFromArray(bits[cps..(cps + 3)]);
            cps += 3;

            if (type == 4)
            {
                int totalLiteralBits = 0;
                int groups = 0;
                //110 100 10111 11110 00101 000
                //literal, 5 bits groups, first bit 1 until last one where first bit is 0
                List<bool> literal = new List<bool>();
                while (bits[cps + totalLiteralBits])
                {
                    literal.AddRange(bits[(cps + totalLiteralBits + 1)..(cps + totalLiteralBits + 5)]);
                    totalLiteralBits += 5;
                    groups++;
                }
                literal.AddRange(bits[(cps + totalLiteralBits + 1)..(cps + totalLiteralBits + 5)]);
                totalLiteralBits += 5;
                groups++;

                value = getIntFromArray(literal.ToArray());
                if ((groups * 4) % 4 != 0)
                {
                    totalLiteralBits += 4 - ((groups * 4) % 4);
                }

                cps += totalLiteralBits;
            }
            else
            {
                List<long> values = new List<long>();
                //operator
                if (bits[cps])
                {
                    //length type 1, number of sub packets
                    cps++;
                    long numberSubPackets = getIntFromArray(bits[cps..(cps + 11)]);
                    cps += 11;

                    for (int i = 0; i < numberSubPackets; i++)
                    {
                        (int cps2, long val2) = getNextPacket(bits[cps..]);
                        values.Add(val2);
                        cps += cps2;
                    }
                    //done with this sub-packet
                }
                else
                {
                    //length type 0, total length
                    cps++;
                    long length = getIntFromArray(bits[cps..(cps + 15)]);
                    cps += 15;
                    int totalSubPLength = 0;
                    while (totalSubPLength < length)
                    {
                        (int cps2, long val2) = getNextPacket(bits[(cps + totalSubPLength)..]);
                        values.Add(val2);
                        totalSubPLength += cps2;
                    }
                    cps += totalSubPLength;
                }

                switch (type)
                {
                    case 0:
                        value = values.Sum();
                        break;
                    case 1:
                        value = values.Aggregate(1L, (ac, v) => ac * v);
                        break;
                    case 2:
                        value = values.Min();
                        break;
                    case 3:
                        value = values.Max();
                        break;
                    case 5:
                        value = values.First() > values.Skip(1).First() ? 1 : 0;
                        break;
                    case 6:
                        value = values.First() < values.Skip(1).First() ? 1 : 0;
                        break;
                    case 7:
                        value = values.First() == values.Skip(1).First() ? 1 : 0;
                        break;
                }
            }

            return (cps, value);
        }

        private static long getIntFromArray(bool[] bits)
        {
            long result = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    result += Convert.ToInt64(Math.Pow(2, bits.Length - 1 - i));
                }
            }

            return result;
        }
    }
}
