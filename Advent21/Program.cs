var dice = Enumerable.Range(1, 100);

var p1st = 7;
var p2st = 10;
//var p1st = 4;
//var p2st = 8;

var p1sc = 0;
var p2sc = 0;

var totalRolls = 0;
var dicePos = 1;

bool p1turn = true;
while (true)
{
    var roll = 0;
    for (int i = 0; i < 3; i++)
    {
        roll += dicePos;
        dicePos += 1;
        dicePos = (dicePos - 1) % 100 + 1;
    }
    totalRolls += 3;

    if (p1turn)
    {
        p1st += roll;
        p1st = (p1st - 1) % 10 + 1;
        p1sc += p1st;
    }
    else
    {
        p2st += roll;
        p2st = (p2st - 1) % 10 + 1;
        p2sc += p2st;
    }

    if (p1sc >= 1000 || p2sc >= 1000)
    {
        break;
    }

    p1turn = !p1turn;
}

var answer1 = totalRolls * Math.Min(p1sc, p2sc);
Console.WriteLine($"Part1: {answer1}");