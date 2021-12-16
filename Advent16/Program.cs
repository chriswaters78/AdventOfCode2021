using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Advent16
{
    class Program
    {

        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var bits = File.ReadAllText(args[0])
                    .Aggregate(new StringBuilder(), (sb, ch) => { 
                            sb.Append(Convert.ToString(Convert.ToInt32(ch.ToString(), 16), 2).PadLeft(4, '0')); 
                            return sb; 
                        }, 
                        sb => sb.ToString())
                    .Select(ch => ch == '1' ? true : false).ToArray();

            (_, long answer1, long answer2) = getNextPacket(bits);

            watch.Stop();
            Console.WriteLine($"Part 1: {answer1}");
            Console.WriteLine($"Part 2: {answer2}");
            Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds}ms");
        }

        private static (int bitsConsumed, long versionTotal, long value) getNextPacket(bool[] bits)
        {
            int cps = 0;
            long version = getIntFromArray(bits[cps..(cps + 3)]);            
            cps += 3;

            long type = getIntFromArray(bits[cps..(cps + 3)]);
            cps += 3;

            if (type == 4)
            {
                int totalLiteralBits = 0;
                //110 100 10111 11110 00101 000
                //literal, 5 bits groups, first bit 1 until last one where first bit is 0
                List<bool> literal = new List<bool>();
                do
                {
                    literal.AddRange(bits[(cps + totalLiteralBits + 1)..(cps + totalLiteralBits + 5)]);
                    totalLiteralBits += 5;
                }
                while (bits[cps + totalLiteralBits - 5]);

                long value = getIntFromArray(literal.ToArray());
                if ((totalLiteralBits / 5 * 4) % 4 != 0)
                {
                    totalLiteralBits += 4 - ((totalLiteralBits / 5 * 4) % 4);
                }

                cps += totalLiteralBits;

                return (cps, version, value);
            }
            List<(int bitsConsumed, long versionTotal, long value)> packets = new List<(int, long, long)>();
            cps++;
            //operator
            switch (bits[cps-1])
            {
                case true:
                    //length type 1, number of sub packets
                    long numberSubPackets = getIntFromArray(bits[cps..(cps + 11)]);
                    cps += 11;

                    for (int i = 0; i < numberSubPackets; i++)
                    {
                        var packet = getNextPacket(bits[cps..]);
                        packets.Add(packet);
                        cps += packet.bitsConsumed;
                    }
                    break;
                case false:
                    long packetLength = getIntFromArray(bits[cps..(cps + 15)]);
                    cps += 15;
                    int totalSubPLength = 0;
                    while (totalSubPLength < packetLength)
                    {
                        var packet = getNextPacket(bits[(cps + totalSubPLength)..]);
                        packets.Add(packet);
                        totalSubPLength += packet.bitsConsumed;
                    }
                    cps += totalSubPLength;
                    break;
            }

            version += packets.Sum(pk => pk.versionTotal);
            return (cps, version, type switch
            {
                0 => packets.Select(pk => pk.value).Sum(),
                1 => packets.Select(pk => pk.value).Aggregate(1L, (ac, v) => ac * v),
                2 => packets.Select(pk => pk.value).Min(),
                3 => packets.Select(pk => pk.value).Max(),
                5 => packets.First().value > packets.Skip(1).First().value ? 1 : 0,
                6 => packets.First().value < packets.Skip(1).First().value ? 1 : 0,
                _ => packets.First().value == packets.Skip(1).First().value ? 1 : 0
            });
        }

        private static long getIntFromArray(bool[] bits)
        {
            long result = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    result |= 1L << bits.Length - 1 - i;
                }
            }
            return result;
        }
    }
}
