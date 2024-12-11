using System.Collections.Concurrent;

var input = File.ReadAllText("input.txt").Split(" ").Select(x => ulong.Parse(x)).ToList();

Console.WriteLine($"Part1: {ParallelLoop(input, 25)}");
Console.WriteLine($"Part2: {ParallelLoop(input, 75)}");

int ParallelLoop(List<ulong> stones, int count)
{
    var numberOfStones = new ConcurrentBag<int>();

    var result = Parallel.ForEach(stones, stone => {
        int stonesCount = 1;
        Blink(stone, 0, count, ref stonesCount);
        numberOfStones.Add(stonesCount);
    });

    return numberOfStones.Sum();
}

void Blink(ulong number, int deep, int maxDeep, ref int stones)
{
    if(deep == maxDeep)
    {
        return;
    }

    if(number == 0)
    {
        number = 1;
        Blink(number, deep + 1, maxDeep, ref stones);
    }
    else
    {
        var digitsNo = Splittable(number);
        if(digitsNo.HasValue)    
        {
            (var left, var right) = Split(number, digitsNo.Value);
            Interlocked.Increment(ref stones);
            Blink(left, deep + 1, maxDeep, ref stones);
            Blink(right, deep + 1, maxDeep, ref stones);
        }
        else
        {
            number *= 2024;
            Blink(number, deep + 1, maxDeep, ref stones);
        }
    }
}

int? Splittable(ulong number)
{
    var digitsNo = (int)Math.Floor(Math.Log10(number) + 1);
    return number != 0 && digitsNo % 2 == 0 ? digitsNo / 2 : null;
}

(ulong left, ulong right) Split(ulong number, int digitsNo)
{
    var pow = (ulong)Math.Pow(10, digitsNo);
    var toSplit = (decimal)number / pow;
    var left = (ulong)Math.Truncate(toSplit);
    var right = (ulong)((toSplit - left) * pow);
    return (left, right);
}