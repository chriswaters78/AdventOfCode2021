using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent4a
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);
            var calls = lines[0].Split(',').Select(str => int.Parse(str)).ToList();

            var boards = new List<int[][]>();
            for (int i = 0; i < (lines.Length - 1) / 6; i++)
            {
                int[][] board = new int[5][];
                for (int j = 0; j < 5; j++)
                {
                    var lineNo = i * 6 + 2 + j;
                    board[j] = lines[lineNo].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                }
                boards.Add(board);
            }

            int answer = -1;
            foreach (var call in calls)
            {
                foreach (var board in boards)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (board[i][j] == call)
                            {
                                board[i][j] = -1;
                            }
                        }
                    }

                    //check if won
                    foreach (bool horiz in new[] { true, false })
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (horiz)
                            {
                                if (board[i].All(number => number == -1))
                                {
                                    //won
                                    answer = board.SelectMany(line => line).Where(number => number != -1).Sum() * call;
                                    goto won;
                                }
                            }
                            else
                            {
                                if (Enumerable.Range(0,5).Select(row => board[row][i]).All(number => number == -1))
                                {
                                    //won
                                    answer = board.SelectMany(line => line).Where(number => number != -1).Sum() * call;
                                    goto won;
                                }
                            }
                        }
                    }
                }
            }

        won:;

        }
    }
}
