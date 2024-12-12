using System.Collections.Concurrent;

var input = File.ReadAllText("input.txt").Split(" ").Select(x => ulong.Parse(x)).ToList();

Console.WriteLine($"Part1: {ParallelCount(input, 25)}");
Console.WriteLine($"Part2: {ParallelCount(input, 75)}");

long ParallelCount(List<ulong> stones, int count)
{
    var numberOfStones = new ConcurrentBag<long>();
    var countCache = new ConcurrentDictionary<CacheKey, long>();
    var splitCache = new ConcurrentDictionary<ulong, (ulong left, ulong? right)>();

    var result = Parallel.ForEach(stones, stone => numberOfStones.Add(CountStones(stone, 0, count, countCache, splitCache)));

    return numberOfStones.Sum();
}

long CountStones(ulong number, int deep, int maxDeep, ConcurrentDictionary<CacheKey, long> countCache, ConcurrentDictionary<ulong, (ulong left, ulong? right)> splitCache)
{
    if (deep == maxDeep)
    {
        return 1;
    }

    (ulong left, ulong? right) = Blink(number, splitCache);

    var counter = 0L;

    Count(left);

    if (right.HasValue)
    {
        Count(right.Value);
    }

    return counter;

    void Count(ulong next)
    {
        var cacheKey = new CacheKey(next, deep + 1);

        if (countCache.TryGetValue(cacheKey, out var cached))
        {
            counter += cached;
        }
        else
        {
            var count = CountStones(next, deep + 1, maxDeep, countCache, splitCache);
            countCache[cacheKey] = count;
            counter += count;
        }
    }
}


(ulong left, ulong? right) Blink(ulong number, ConcurrentDictionary<ulong, (ulong left, ulong? right)> splitCache)
{
    var left = number;
    ulong? right = null;

    if (left == 0)
    {
        left = 1;
    }
    else
    {
        (left, right) = Split(number, splitCache);

        if (right == null)
        {
            left *= 2024;
        }
    }
    return (left, right);
}

(ulong left, ulong? right) Split(ulong number, ConcurrentDictionary<ulong, (ulong left, ulong? right)> splitCache)
{
    if (splitCache.TryGetValue(number, out var splitResult))
    {
        return (splitResult.left, splitResult.right);
    }

    var length = (int)Math.Floor(Math.Log10(number) + 1);
    int? digitsNo = number != 0 && length % 2 == 0 ? length / 2 : null;

    if (digitsNo == null)
    {
        splitCache[number] = (number, null);
        return (number, null);
    }

    var pow = (ulong)Math.Pow(10, digitsNo.Value);
    var toSplit = (decimal)number / pow;
    var left = (ulong)Math.Truncate(toSplit);
    var right = (ulong)((toSplit - left) * pow);

    splitCache[number] = (left, right);

    return (left, right);

}

record CacheKey(ulong Number, int Deep);