//var states = new Dictionary<(byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2), int>();
//states.Add(c, 0);

using System.Diagnostics;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        var priorityQueue = new PriorityQueue<(byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2), int>();

        Dictionary<(byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2), int> best = new Dictionary<(byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2), int>();

        //example
        //#############
        //#...........#
        //###B#C#B#D###
        //  #A#D#C#A#
        //  #########
        //priorityQueue.Enqueue((13, 19, 12, 16, 14, 17, 15, 18), 0);
        priorityQueue.Enqueue((16, 19, 17, 18, 12, 14, 13, 15), 0);

        int answer = int.MinValue;
        while (true)
        {
            (byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) state;
            int energy;
            if (!priorityQueue.TryDequeue(out state, out energy))
            {
                break;
            }

            Console.WriteLine($"Energy cost: {energy}, best count {best.Count}, state:");
            //Console.WriteLine(print(state));

            //try and move each of the 8 characters
            for (int c = 1; c <= 8; c++)
            {
                //try and move this character to each of these spaces
                for (byte endPos = 1; endPos <= 19; endPos++)
                {
                    if (endPos == 3 || endPos == 5 || endPos == 7 || endPos == 9)
                    {
                        continue;
                    }

                    if (endPos == getCharacterPosition(state,c))
                    {
                        continue;
                    }

                    //check if the move is valid
                    var (valid, moves) = isValidMove(state, c, endPos);
                    if (!valid)
                    {
                        continue;
                    }
                    //valid move
                    var newEnergy = energy + (int)Math.Pow(10, (c - 1) / 2) * moves;
                    var newState = normaliseState(setCharacterPosition(state, c, endPos));

                    //Console.WriteLine($"Energy cost: {energy} -> {newEnergy}");
                    //Console.WriteLine(print(state));
                    //Console.WriteLine("===>");
                    //Console.WriteLine(print(newState));

                    if (best.ContainsKey(newState))
                    {
                        if (best[newState] > newEnergy)
                        {
                            best[newState] = newEnergy;
                            priorityQueue.Enqueue(newState, newEnergy);
                        }
                    }
                    else
                    {
                        best[newState] = newEnergy;
                        priorityQueue.Enqueue(newState, newEnergy);
                    }

                    if (newState.a1 == 12 && newState.a2 == 13 && newState.b1 == 14 && newState.b2 == 15 && newState.c1 == 16 && newState.c2 == 17 && newState.d1 == 18 && newState.d2 == 19)
                    {
                        answer = newEnergy;
                        goto quit;
                    }
                }
            }
        }

    quit:;
        Console.WriteLine($"Minimum: {answer} in {watch.ElapsedMilliseconds}ms");
    }

    static string print((byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) state)
    {
        char[][] grid = new char[5][];
        grid[0] = Enumerable.Repeat('#', 13).ToArray();
        grid[1] = Enumerable.Repeat('.', 13).ToArray();
        grid[1][0] = '#';
        grid[1][12] = '#';
        grid[2] = Enumerable.Repeat('#', 13).ToArray();
        grid[3] = Enumerable.Repeat('#', 13).ToArray();
        grid[4] = Enumerable.Repeat('#', 13).ToArray();

        grid[2][3] = '.';
        grid[3][3] = '.';
        grid[2][5] = '.';
        grid[3][5] = '.';
        grid[2][7] = '.';
        grid[3][7] = '.';
        grid[2][9] = '.';
        grid[3][9] = '.';

        Action<char, byte> setChar = (c, p) =>
        {
            if (p >= 12)
            {
                if (p % 2 == 0)
                {
                    grid[2][p - 9] = c;
                }
                else
                {
                    grid[3][p - 10] = c;
                }
            }
            else
            {
                grid[1][p] = c;
            }
        };

        setChar('A', state.a1);
        setChar('A', state.a2);
        setChar('B', state.b1);
        setChar('B', state.b2);
        setChar('C', state.c1);
        setChar('C', state.c2);
        setChar('D', state.d1);
        setChar('D', state.d2);

        StringBuilder sb = new StringBuilder();
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                sb.Append(grid[r][c]);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    static (bool, int) isValidMove((byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) state, int c, byte endPos)
    {
        byte corridorStart;
        byte corridorEnd;
        int spacesMoved = 0;

        var allCharPositions = new[] { state.a1, state.a2, state.b1, state.b2, state.c1, state.c2, state.d1, state.d2 };

        //trying to move character c (1..8) to position p (1..15)
        var startPos = allCharPositions[c - 1];
        if (isRoom(startPos))
        {
            //if it is in the bottom correct room
            //and is either the bottom one
            //or the top one with the correct character below it
            //then we don't move agian

            //if in correct top room
            if (startPos == topRoom(c))
            {
                //INVALID if value in bottom room is correct
                if (allCharPositions[otherChar(c) - 1] == bottomRoom(c))
                {
                    return (false, 0);
                }
                //otherwise move up 1
                corridorStart = corridorForRoom(startPos);
                spacesMoved++;
            }
            else if (startPos == bottomRoom(c))
            {
                //INVALID, in the correct bottom room
                return (false, 0);
            }
            else
            {
                if (startPos % 2 == 0)
                {
                    //in an incorrect top room
                    corridorStart = (byte)(3 + (startPos - 12));
                    spacesMoved++;
                }
                else
                {
                    //in an incorrect bottom room
                    //if any characters are in top room, invalid
                    if (Enumerable.Range(1, 8).Any(c => allCharPositions[c - 1] == startPos - 1))
                    {
                        return (false, 0);
                    }

                    corridorStart = corridorForRoom(startPos);
                    spacesMoved += 2;
                }
            }
        }
        else
        {
            corridorStart = startPos;
        }

        if (isRoom(endPos))
        {
            //can only move into this room if it is the correct one
            if (bottomRoom(c) != endPos && topRoom(c) != endPos)
            {
                return (false, 0);
            }

            //trying to move into the correct room
            if (topRoom(c) == endPos)
            {
                //can move in only if bottom room contains correct character
                if (allCharPositions[otherChar(c) - 1] == bottomRoom(c))
                {
                    //and top room does not contain any characters
                    if (Enumerable.Range(1, 8).Any(c => allCharPositions[c - 1] == endPos))
                    {
                        return (false, 0);
                    }
                    corridorEnd = corridorForRoom(endPos);
                    spacesMoved++;
                }
                else
                {
                    return (false, 0);
                }
            }
            else if (bottomRoom(c) == endPos)
            {
                //top room and bottom room must be empty
                if (Enumerable.Range(1, 8).Any(c => allCharPositions[c - 1] == endPos - 1 || allCharPositions[c - 1] == endPos))
                {
                    return (false, 0);
                }

                corridorEnd = corridorForRoom(endPos);
                spacesMoved += 2;
            }
            else
            {
                //trying to move to an incorrect room
                return (false, 0);
            }
        }
        else
        {
            corridorEnd = endPos;
        }

        if (corridorStart == startPos && corridorEnd == endPos)
        {
            //can't move corridor to corridor
            return (false, 0);
        }

        //we now have valid corridorStart, corridorEnd, spacedMoved
        //simply check the intervening corridor characters

        (corridorStart, corridorEnd) = corridorStart <= corridorEnd ? (corridorStart, corridorEnd) : (corridorEnd, corridorStart);
        for (byte cp = corridorStart; cp <= corridorEnd; cp++)
        {
            spacesMoved++;
            if (cp != startPos && cp != 3 && cp != 5 && cp != 7 && cp != 9)
            {
                if (allCharPositions.Any(p => p == cp))
                {
                    return (false, 0);
                }
            }
        }
        //we counted start and end, so subtract 1
        spacesMoved--;

        return (true, spacesMoved);
    }

    static byte corridorForRoom(byte roomPos)
    {
        if (roomPos % 2 == 1)
        {
            roomPos--;
        }

        return (byte)(roomPos - 9);
    }

    static int otherChar(int c)
    {
        return c + ((c % 2 == 1) ? 1 : -1);
    }

    static bool isRoom(int p)
    {
        return p >= 12;
    }

    static byte topRoom(int c)
    {
        return (byte) (12 + 2 * ((c - 1) / 2));
    }

    static byte bottomRoom(int c)
    {
        return (byte)(topRoom(c) + 1);
    }

    //byte? getCharacterPosition((byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) state, int c)
    //{
    //    if (state.a1 == c)
    //    {
    //        return 1;
    //    }
    //    if (state.a2 == c)
    //    {
    //        return 2;
    //    }
    //    if (state.b1 == c)
    //    {
    //        return 3;
    //    }
    //    if (state.b2 == c)
    //    {
    //        return 4;
    //    }
    //    if (state.c1 == c)
    //    {
    //        return 5;
    //    }
    //    if (state.c2 == c)
    //    {
    //        return 6;
    //    }
    //    if (state.d1 == c)
    //    {
    //        return 7;
    //    }
    //    if (state.d2 == c)
    //    {
    //        return 8;
    //    }

    //    return null;
    //}

    static byte getCharacterPosition((byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) state, int c) => c switch
    {
        1 => state.a1,
        2 => state.a2,
        3 => state.b1,
        4 => state.b2,
        5 => state.c1,
        6 => state.c2,
        7 => state.d1,
        8 => state.d2,
    };

    static (byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) setCharacterPosition((byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) state, int c, byte p) => c switch
    {
        1 => (p, state.a2, state.b1, state.b2, state.c1, state.c2, state.d1, state.d2),
        2 => (state.a1, p, state.b1, state.b2, state.c1, state.c2, state.d1, state.d2),
        3 => (state.a1, state.a2, p, state.b2, state.c1, state.c2, state.d1, state.d2),
        4 => (state.a1, state.a2, state.b1, p, state.c1, state.c2, state.d1, state.d2),
        5 => (state.a1, state.a2, state.b1, state.b2, p, state.c2, state.d1, state.d2),
        6 => (state.a1, state.a2, state.b1, state.b2, state.c1, p, state.d1, state.d2),
        7 => (state.a1, state.a2, state.b1, state.b2, state.c1, state.c2, p, state.d2),
        8 => (state.a1, state.a2, state.b1, state.b2, state.c1, state.c2, state.d1, p),
    };

    static (byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) normaliseState((byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2) state) =>
    (
        state.a1 <= state.a2 ? state.a1 : state.a2,
        state.a2 > state.a1 ? state.a2 : state.a1,
        state.b1 <= state.b2 ? state.b1 : state.b2,
        state.b2 > state.b1 ? state.b2 : state.b1,
        state.c1 <= state.c2 ? state.c1 : state.c2,
        state.c2 > state.c1 ? state.c2 : state.c1,
        state.d1 <= state.d2 ? state.d1 : state.d2,
        state.d2 > state.d1 ? state.d2 : state.d1
    );
}



