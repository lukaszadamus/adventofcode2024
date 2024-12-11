var input = File.ReadAllText("input.txt").Split(" ").Select(x => ulong.Parse(x)).ToList();

Console.WriteLine($"Part1: {Loop(input, 25)}");
Console.WriteLine($"Part2: {Loop(input, 75)}");

int Loop(List<ulong> stones, int count)
{
    for(int i=0; i<count; i++)
    {
        Blink(input);
        //Console.WriteLine(string.Join(" ", input));
    }
    return stones.Count;
}

void Blink(List<ulong> stones)
{    
    int? digitsNo = 0;
    for (int i = stones.Count - 1; i >= 0; i--)
    {        
        digitsNo = Splittable(stones[i]);
        
        if(stones[i] == 0)
        {
            stones[i] = 1;
        }
        else if(digitsNo.HasValue)
        {
            (ulong left, ulong right) = Split(stones[i], digitsNo.Value);
            stones[i] = right;
            stones.Insert(i, left);
        }
        else
        {
            stones[i] *= 2024;
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