using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Advent10a
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]).ToList();

            long part1Score = 0;
            List<long> part2Scores = new List<long>();
            foreach (var line in lines)
            {
                Stack<char> stack = new Stack<char>(line.Take(1));
                foreach (var c in line.Skip(1))
                {
                    switch (c)
                    {
                        case ')':
                            if (stack.Peek() != '(')
                            {
                                part1Score += 3;
                                goto skip;
                            }
                            stack.Pop();
                            break;
                        case ']':
                            if (stack.Peek() != '[')
                            {
                                part1Score += 57;
                                goto skip;
                            }
                            stack.Pop();
                            break;
                        case '}':
                            if (stack.Peek() != '{')
                            {
                                part1Score += 1197;
                                goto skip;
                            }
                            stack.Pop();
                            break;
                        case '>':
                            if (stack.Peek() != '<')
                            {
                                part1Score += 25137;
                                goto skip;
                            }
                            stack.Pop();
                            break;
                        default:
                            stack.Push(c);
                            break;
                    }
                }

                part2Scores.Add(getScore(stack));

            skip:;
            }

            var part2Score = part2Scores.OrderBy(i => i).Skip(part2Scores.Count / 2).First();
        }
        private static long getScore(Stack<char> stack)
        {
            long score = 0;
            while (stack.Count != 0)
            {
                score *= 5;
                switch (stack.Pop())
                {
                    case '(':
                        score += 1;
                        break;
                    case '[':
                        score += 2;
                        break;
                    case '{':
                        score += 3;
                        break;
                    case '<':
                        score += 4;
                        break;
                }
            }

            return score;
        }
    }

}
