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
        var priorityQueue = new PriorityQueue<(byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4), int>();

        var best = new Dictionary<(byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4), int>();

        //example
        //#############
        //#...........#
        //###B#C#B#D###
        //  #D#C#B#A#
        //  #D#B#A#C#
        //  #A#D#C#A#
        //#########
        //priorityQueue.Enqueue((15, 22, 25, 27, 12, 18, 20, 21, 16, 17, 23, 26, 13, 14, 19, 24), 0);
        //Input
        priorityQueue.Enqueue((20, 22, 25, 27, 18, 21, 23, 24, 12, 16, 17, 26, 13, 14, 15, 19), 0);

        int answer = int.MinValue;
        int movesChecked = 0;
        while (true)
        {
            (byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4) state;
            int energy;
            if (!priorityQueue.TryDequeue(out state, out energy))
            {
                break;
            }

            //Console.WriteLine($"Energy cost: {energy}, best count {best.Count}, state:");
            //Console.WriteLine(print(state));

            //try and move each of the 8 characters
            for (int c = 1; c <= 16; c++)
            {
                //try and move this character to each of these spaces
                for (byte endPos = 1; endPos <= 27; endPos++)
                {
                    if (endPos == 3 || endPos == 5 || endPos == 7 || endPos == 9)
                    {
                        continue;
                    }

                    if (endPos == getCharacterPosition(state, c))
                    {
                        continue;
                    }

                    movesChecked++;
                    //check if the move is valid
                    var (valid, moves) = isValidMove(state, c, endPos);
                    if (!valid)
                    {
                        continue;
                    }
                    //valid move
                    var newEnergy = energy + (int)Math.Pow(10, (c - 1) / 4) * moves;
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

                    if (newState.a1 == 12 && newState.a2 == 13 && newState.a3 == 14 && newState.a4 == 15
                        && newState.b1 == 16 && newState.b2 == 17 && newState.b3 == 18 && newState.b4 == 19
                        && newState.c1 == 20 && newState.c2 == 21 && newState.c3 == 22 && newState.c4 == 23
                        && newState.d1 == 24 && newState.d2 == 25 && newState.d3 == 26 && newState.d4 == 27)
                    {
                        answer = newEnergy;
                        goto quit;
                    }
                }
            }
        }

    quit:;
        Console.WriteLine($"Minimum: {answer} in {watch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Best states recorded: {best.Count}");
        Console.WriteLine($"Moves checked: {movesChecked}");
    }

    static string print((byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4) state)
    {
        char[][] grid = new char[7][];
        grid[0] = Enumerable.Repeat('#', 13).ToArray();
        grid[1] = Enumerable.Repeat('.', 13).ToArray();
        grid[1][0] = '#';
        grid[1][12] = '#';
        grid[2] = Enumerable.Repeat('#', 13).ToArray();
        grid[3] = Enumerable.Repeat('#', 13).ToArray();
        grid[4] = Enumerable.Repeat('#', 13).ToArray();
        grid[5] = Enumerable.Repeat('#', 13).ToArray();
        grid[6] = Enumerable.Repeat('#', 13).ToArray();

        grid[2][3] = '.';
        grid[3][3] = '.';
        grid[4][3] = '.';
        grid[5][3] = '.';
        grid[2][5] = '.';
        grid[3][5] = '.';
        grid[4][5] = '.';
        grid[5][5] = '.';
        grid[2][7] = '.';
        grid[3][7] = '.';
        grid[4][7] = '.';
        grid[5][7] = '.';
        grid[2][9] = '.';
        grid[3][9] = '.';
        grid[4][9] = '.';
        grid[5][9] = '.';

        Action<char, byte> setChar = (c, p) =>
        {
            if (p >= 12)
            {
                switch (p % 4)
                {
                    case 0:
                        grid[2][p / 2 - 3] = c;
                        break;
                    case 1:
                        grid[3][p / 2 - 3] = c;
                        break;
                    case 2:
                        grid[4][p / 2 - 4] = c;
                        break;
                    case 3:
                        grid[5][p / 2 - 4] = c;
                        break;
                }
            }
            else
            {
                grid[1][p] = c;
            }
        };

        setChar('A', state.a1);
        setChar('A', state.a2);
        setChar('A', state.a3);
        setChar('A', state.a4);
        setChar('B', state.b1);
        setChar('B', state.b2);
        setChar('B', state.b3);
        setChar('B', state.b4);
        setChar('C', state.c1);
        setChar('C', state.c2);
        setChar('C', state.c3);
        setChar('C', state.c4);
        setChar('D', state.d1);
        setChar('D', state.d2);
        setChar('D', state.d3);
        setChar('D', state.d4);

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

    static (bool, int) isValidMove((byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4) state, int c, byte endPos)
    {
        byte corridorStart;
        byte corridorEnd;
        int spacesMoved = 0;

        var allCharPositions = new[] { state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4 };
        Dictionary<byte,int> charInEachPositions = allCharPositions.Select((cp, i) => (cp, i)).ToDictionary(tp => tp.cp, tp => tp.i + 1);

        //trying to move character c (1..8) to position p (1..15)
        var startPos = allCharPositions[c - 1];
        if (isRoom(startPos))
        {
            //if it is in the correct room
            //and all positions lower are correct
            //then invalid
            if (correctRooms(c).Contains(startPos))
            {
                int noRoomsBelow = 3 - (startPos % 4);
                bool allCorrect = true;
                for (byte r = (byte) (startPos + 1); r <= startPos + noRoomsBelow; r++)
                {
                    //these lower rooms MUST have a character
                    if (getCharacter(charInEachPositions[r]) != getCharacter(c))
                    {
                        allCorrect = false;
                    }
                }

                if (allCorrect)
                {
                    return (false, 0);
                }
                //some positions lower are incorrect

            }
            //in an incorrect room, or in a correct room with lower rooms incorrect
            //treat the same - check other room positions are free
            int noRoomsAbove = startPos % 4;
            for (byte r = (byte) (startPos - 1); r >= startPos - noRoomsAbove; r--)
            {
                if (charInEachPositions.ContainsKey(r))
                {
                    return (false, 0);
                }
            }

            //we can move out
            corridorStart = corridorForRoom(startPos);
            spacesMoved = spacesMoved + noRoomsAbove + 1;
        }
        else
        {
            corridorStart = startPos;
        }

        if (isRoom(endPos))
        {
            //can only move into this room if it is the correct one
            if (!correctRooms(c).Contains(endPos))
            {
                return (false, 0);
            }

            //trying to move into the correct room
            //all rooms above the target must be empty
            int noRoomsAbove = endPos % 4;
            for (byte r = endPos; r >= endPos - noRoomsAbove; r--)
            {
                if (charInEachPositions.ContainsKey(r))
                {
                    return (false, 0);
                }
            }

            //all rooms below the target must be non-empty
            //and have the correct character
            int noRoomsBelow = 3 - (endPos % 4);
            bool allCorrect = true;
            for (byte r = (byte)(endPos + 1); r <= endPos + noRoomsBelow; r++)
            {
                //these lower rooms MUST have a character
                if (!charInEachPositions.ContainsKey(r) || getCharacter(charInEachPositions[r]) != getCharacter(c))
                {
                    allCorrect = false;
                }
            }

            if (!allCorrect)
            {
                return (false, 0);
            }

            //we can move out
            corridorEnd = corridorForRoom(endPos);
            spacesMoved = spacesMoved + noRoomsAbove + 1;
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

    static int getCharacter(int c)
    {
        return 1 + (c - 1) / 4;
    }

    static byte corridorForRoom(byte roomPos)
    {

        return (byte)((roomPos - (roomPos % 4)) /2  - 3);
    }

    //static int[] otherChar(int c)
    //{
    //    return Enumerable.Range(1 + ((c - 1) / 4) * 4, 4).Where(i => i != c).ToArray();
    //}

    static bool isRoom(int p)
    {
        return p >= 12;
    }

    static byte topRoom(int c)
    {
        return (byte)(8 + getCharacter(c) * 4);
    }

    static byte[] correctRooms(int c)
    {
        return new byte[] { (byte)topRoom(c), (byte)(topRoom(c) + 1), (byte)(topRoom(c) + 2), (byte)(topRoom(c) + 3) };
    }


    static byte getCharacterPosition((byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4) state, int c) => c switch
    {
        1 => state.a1,
        2 => state.a2,
        3 => state.a3,
        4 => state.a4,
        5 => state.b1,
        6 => state.b2,
        7 => state.b3,
        8 => state.b4,
        9 => state.c1,
        10 => state.c2,
        11 => state.c3,
        12 => state.c4,
        13 => state.d1,
        14 => state.d2,
        15 => state.d3,
        16 => state.d4,
    };

    static (byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4) setCharacterPosition((byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4) state, int c, byte p) => c switch
    {
        1 => (p, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        2 => (state.a1, p, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        3 => (state.a1, state.a2, p, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        4 => (state.a1, state.a2, state.a3, p, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        5 => (state.a1, state.a2, state.a3, state.a4, p, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        6 => (state.a1, state.a2, state.a3, state.a4, state.b1, p, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        7 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, p, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        8 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, p, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        9 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, p, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        10 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, p, state.c3, state.c4, state.d1, state.d2, state.d3, state.d4),
        11 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, p, state.c4, state.d1, state.d2, state.d3, state.d4),
        12 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, p, state.d1, state.d2, state.d3, state.d4),
        13 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, p, state.d2, state.d3, state.d4),
        14 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, p, state.d3, state.d4),
        15 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, p, state.d4),
        16 => (state.a1, state.a2, state.a3, state.a4, state.b1, state.b2, state.b3, state.b4, state.c1, state.c2, state.c3, state.c4, state.d1, state.d2, state.d3, p),
    };

    static (byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4) normaliseState((byte a1, byte a2, byte a3, byte a4, byte b1, byte b2, byte b3, byte b4, byte c1, byte c2, byte c3, byte c4, byte d1, byte d2, byte d3, byte d4) state)
    {
        var a = new[] { state.a1, state.a2, state.a3, state.a4 }.OrderBy(p => p).ToArray();
        var b = new[] { state.b1, state.b2, state.b3, state.b4 }.OrderBy(p => p).ToArray();
        var c = new[] { state.c1, state.c2, state.c3, state.c4 }.OrderBy(p => p).ToArray();
        var d = new[] { state.d1, state.d2, state.d3, state.d4 }.OrderBy(p => p).ToArray();

        return (a[0], a[1], a[2], a[3], b[0], b[1], b[2], b[3], c[0], c[1], c[2], c[3], d[0], d[1], d[2], d[3]);
    }
}



