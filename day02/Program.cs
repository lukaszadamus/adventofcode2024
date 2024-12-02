var lines = File.ReadAllLines("input.txt").Select(x => x.Split(' ').Select(x => int.Parse(x)));

var safeReportsCount = lines    
    .Select(x => IsSafe(x, false))
    .Count(x => x == true);

var safeReportsCountWithProblemDampener = lines    
    .Select(x => IsSafe(x, true))
    .Count(x => x == true);

bool IsSafe(IEnumerable<int> levels, bool withProblemDampener)
{
    var tmp = levels
        .Zip(levels.Skip(1), (a, b) => a - b > 0 && a - b <= 3 ? 1 : a - b < 0 && a - b >= -3 ? -1 : 0);
    
    var isSafe = tmp.All(x => x != 0) && tmp.Distinct().Count() == 1;

    if (!isSafe && withProblemDampener)
    {
        for (var index = 0; index < levels.Count(); index++)
        {            
            isSafe = IsSafe(levels.Where((v, i) => i != index), false);
            if (isSafe)
            {     
                break;
            }
        }
    }

    return isSafe;
}

Console.WriteLine($"safeReportsCount {safeReportsCount}");
Console.WriteLine($"safeReportsCountWithProblemDampener {safeReportsCountWithProblemDampener}");
