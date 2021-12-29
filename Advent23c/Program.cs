//var states = new Dictionary<(byte a1, byte a2, byte b1, byte b2, byte c1, byte c2, byte d1, byte d2), int>();
//states.Add(c, 0);

using System.Diagnostics;
using System.Text;

class Program
{
    //#############
    //#...........#
    //###B#C#B#D###
    //###D#C#B#A###
    //###D#B#A#C###
    //###A#D#C#A###
    //#############
    
    //room columns are 2, 4, 6, 8
    static string example =
       "..........." //0 -> 10, corridor
     + "BCBD"        //11 -> 14
     + "DCBA"        //15 -> 18
     + "DBAC"        //19 -> 22
     + "ADCA";       //23 -> 26

    static string finalPosition =   "...........ABCDABCDABCDABCD";
    static string input =           "...........CCABDCBADBACDDBA";

    static void Main(string[] args)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        
        var priorityQueue = new PriorityQueue<string, int>();
        var best = new Dictionary<string, int>();

        priorityQueue.Enqueue(input, 0);

        int answer = int.MinValue;
        int movesChecked = 0;
        while (true)
        {
            string state;
            int energy;
            if (!priorityQueue.TryDequeue(out state, out energy))
            {
                Console.WriteLine($"No solution found!!!");
                break;
            }

            for (int startPos = 0; startPos < 27; startPos++)
            {
                for (int endPos = 0; endPos < 27; endPos++)
                {
                    if (startPos == endPos
                        || startPos == 2 || startPos == 4 || startPos == 6 || startPos == 8 || state[startPos] == '.'
                        || endPos == 2 || endPos == 4 || endPos == 6 || endPos == 8 || state[endPos] != '.')
                        continue;

                    movesChecked++;


                    (bool valid, string newState, int moves) = isValidMove(state, startPos, endPos);

                    if (!valid)
                        continue;

                    var newEnergy = energy + (int)Math.Pow(10, 3 - ('D' - state[startPos])) * moves;
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

                    if (newState == finalPosition)
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

    static bool isRoom(int p) => p >= 11;
    static int roomToIndex((int r, int c) room) => 11 + 4 * room.r + room.c;
    static (int r, int c) indexToRoom(int p) => ((p - 11) / 4, (p - 3) % 4);
    static int corridorForRoom(int p) => (p - 3) % 4 * 2 + 2;
    static bool correctRoom(char c, int p) => c switch
    {
        'A' => p >= 11 && (p - 3) % 4 == 0,
        'B' => p >= 12 && (p - 3) % 4 == 1,
        'C' => p >= 13 && (p - 3) % 4 == 2,
        'D' => p >= 14 && (p - 3) % 4 == 3,

    };

    static (bool, string, int) isValidMove(string state, int startPos, int endPos)
    {
        int corridorStart;
        int corridorEnd;
        int spacesMoved = 0;

        if (!isRoom(startPos) && !isRoom(endPos))
            return default;

        if (isRoom(startPos) && isRoom(endPos) && corridorForRoom(startPos) == corridorForRoom(endPos))
            return default;

        if (isRoom(startPos))
        {

            (int r, int c) startRoom = indexToRoom(startPos);
            //if it is in the correct room
            //and all positions lower are correct
            //then no need to move again
            if (correctRoom(state[startPos], startPos))
            {
                bool allCorrect = true;
                for (int r = startRoom.r + 1; r < 4; r++)
                {
                    if (state[roomToIndex((r, startRoom.c))] == state[startPos])
                    {
                        allCorrect &= true;
                    }
                }
                //some positions lower are incorrect
                if (!allCorrect)
                    return default;
            }

            //in an incorrect room, or in a correct room with lower rooms incorrect
            //check the higher room positions are free, in which case it can move
            for (int r = startRoom.r - 1; r >= 0; r--)
            {
                if (state[roomToIndex((r, startRoom.c))] != '.')
                    return default;
            }

            corridorStart = corridorForRoom(startPos);
            spacesMoved += startRoom.r + 1;
        }
        else
        {
            corridorStart = startPos;
        }

        if (isRoom(endPos))
        {
            //can only move into this room if it is the correct one
            if (!correctRoom(state[startPos], endPos))
                return default;

            (int r, int c) endRoom = indexToRoom(endPos);

            //trying to move into the correct room
            //all rooms above the target must be empty
            for (int r = endRoom.r - 1; r >= 0; r--)
            {
                if (state[roomToIndex((r, endRoom.c))] != '.')
                    return default;
            }

            //all rooms below the target must be occupied with correct character
            for (int r = endRoom.r + 1; r < 4; r++)
            {
                if (state[roomToIndex((r, endRoom.c))] != state[startPos])
                    return default;
            }

            corridorEnd = corridorForRoom(endPos);
            spacesMoved += endRoom.r;
        }
        else
        {
            corridorEnd = endPos;
        }

        //we now have valid corridorStart, corridorEnd, spacedMoved
        //simply check the intervening corridor characters
        (corridorStart, corridorEnd) = corridorStart <= corridorEnd ? (corridorStart, corridorEnd) : (corridorEnd, corridorStart);
        for (int p = corridorStart; p <= corridorEnd; p++)
        {
            if (p == startPos)
                continue;

            if (state[p] != '.')
                return default;

            spacesMoved++;
        }

        StringBuilder sb = new StringBuilder(state);
        sb[endPos] = state[startPos];
        sb[startPos] = '.';
        return (true, sb.ToString(), spacesMoved);
    }

    static string _print(string state)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("#############");
        sb.AppendLine();
        sb.Append("#");
        for (int i = 0; i < 11; i++)
        {
            sb.Append(state[i]);
        }
        sb.AppendLine("#");
        for (int i = 0; i < 4; i++)
        {
            sb.Append("###");
            for (int j = 0; j < 4; j++)
            {
                sb.Append(state[11 + i * 4 + j]);
                sb.Append("#");
            }
            sb.AppendLine("##");
        }
        sb.AppendLine("#############");

        return sb.ToString();
    }

    static void print(string state, string newState, int energy, int newEnergy)
    {
        Console.WriteLine($"Energy cost: {energy} -> {newEnergy}");
        Console.WriteLine(_print(state));
        Console.WriteLine("===>");
        Console.WriteLine(_print(newState));
    }
}



