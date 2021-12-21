Dictionary<(bool, int, int, int, int), (long,long)> cache = new Dictionary<(bool, int, int, int, int), (long, long)>();

(long,long) getWays((bool p1Turn, int p1pos, int p2pos, int p1sc, int p2sc) tp)
{
    if (cache.ContainsKey(tp))
    {
        return cache[tp];
    }

    (long p1w,long p2w) result = (0,0);

    if (tp.p1sc >= 21)
    {
        result = (1, 0);
    }
    else if (tp.p2sc >= 21)
    {
        result = (0, 1);
    }
    else
    {
        var rollKeys = from r1 in Enumerable.Range(1, 3)
                       from r2 in Enumerable.Range(1, 3)
                       from r3 in Enumerable.Range(1, 3)
                       let r = r1 + r2 + r3
                       let p1pos = tp.p1Turn ? (tp.p1pos + r - 1) % 10 + 1 : tp.p1pos
                       let p2pos = !tp.p1Turn ? (tp.p2pos + r - 1) % 10 + 1 : tp.p2pos
                       let p1sc = tp.p1Turn ? tp.p1sc + p1pos : tp.p1sc
                       let p2sc = !tp.p1Turn ? tp.p2sc + p2pos : tp.p2sc
                       select (!tp.p1Turn, p1pos, p2pos, p1sc, p2sc);                       

        foreach (var key in rollKeys)
        {
            var res1 = getWays(key);
            result.p1w += res1.Item1;
            result.p2w += res1.Item2;
        }
    }

    cache.Add(tp, result);

    return result;
}

var p1st = 7;
var p2st = 10;
//var p1st = 4;
//var p2st = 8;

(long p1w, long p2w) solve = getWays((true, p1st, p2st, 0, 0));

var answer2 = Math.Max(solve.p1w, solve.p2w);
Console.WriteLine($"Part2: {answer2}");