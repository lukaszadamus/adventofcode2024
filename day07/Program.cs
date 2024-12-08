var input = File.ReadAllLines("input.txt")
    .Select(x => x.Split(": "))
    .Select(x => new Equation(long.Parse(x[0]), [.. x[1].Split(" ").Select(x => long.Parse(x))]));

Console.WriteLine($"Part1: {Count(input, ['+', '*'])}");
Console.WriteLine($"Part2: {Count(input, ['+', '*', '|'])}");

long Count(IEnumerable<Equation> equations, IEnumerable<char> allowedOperators)
{
    var count = 0L;
    foreach (var equation in equations)
    {
        count += Evaluate(equation, allowedOperators);
    }
    return count;
}

long Evaluate(Equation equation, IEnumerable<char> allowedOperators)
{
    equation.Numbers.Reverse();
    var combinations = GetOperatorsCombinations(equation.Numbers.Count - 1 , allowedOperators);
    foreach (var operators in combinations)
    {
        var numbers = new Stack<long>(equation.Numbers);

        while (operators.Count > 0)
        {
            var result = 0L;
            var left = numbers.Pop();
            var right = numbers.Pop();
            result += operators.Pop() switch
            {
                '*' => left * right,
                '+' => left + right,
                '|' => long.Parse($"{left}{right}"),
                _ => throw new NotImplementedException(),
            };
            numbers.Push(result);

            if (result > equation.Result)
            {
                break;
            }

        }
        if (numbers.Pop() == equation.Result && numbers.Count == 0)
        {
            return equation.Result;
        }
    }
    return 0;
}

List<Stack<char>> GetOperatorsCombinations(int numberOfOperators, IEnumerable<char> operators)
{
    var result = new List<Stack<char>>();

    Find("", 0);

    return result;

    void Find(string operations, int level)
    {
        level += 1;
        foreach(var op in operators)
        {
            if(operations.Length + 1 == numberOfOperators)
            {
                result.Add(new Stack<char>(operations+op));
            }
            if(level < numberOfOperators)
            {
                Find(operations + op, level);
            }
        }
    }
}

record Equation(long Result, List<long> Numbers);
