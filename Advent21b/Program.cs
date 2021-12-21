using System.Collections.Concurrent;
using System.Diagnostics;

var watch = new Stopwatch();
watch.Start();

var cache = new ConcurrentDictionary<(bool, int, int, int, int), (long, long)>();

(long,long) getWays((bool p1Turn, int p1pos, int p2pos, int p1sc, int p2sc) tp) => cache.GetOrAdd(tp, _ => tp switch
{
    _ when tp.p1sc >= 21 => (1, 0),
    _ when tp.p2sc >= 21 => (0, 1),
    _ => 
            (  from r1 in Enumerable.Range(1, 3)
                from r2 in Enumerable.Range(1, 3)
                from r3 in Enumerable.Range(1, 3)
                let r = r1 + r2 + r3
                let p1pos = tp.p1Turn ? (tp.p1pos + r - 1) % 10 + 1 : tp.p1pos
                let p2pos = !tp.p1Turn ? (tp.p2pos + r - 1) % 10 + 1 : tp.p2pos
                let p1sc = tp.p1Turn ? tp.p1sc + p1pos : tp.p1sc
                let p2sc = !tp.p1Turn ? tp.p2sc + p2pos : tp.p2sc
                select(!tp.p1Turn, p1pos, p2pos, p1sc, p2sc)).Aggregate((0L, 0L), (acc, key) => (acc.Item1 + getWays(key).Item1, acc.Item2 + getWays(key).Item2)),
});

(long p1w, long p2w) answer = getWays((true, int.Parse(args[0]), int.Parse(args[1]), 0, 0));

Console.WriteLine($"Part 2: {Math.Max(answer.p1w, answer.p2w)} [{watch.ElapsedMilliseconds}ms]");

