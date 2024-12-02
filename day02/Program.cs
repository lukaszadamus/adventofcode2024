
var unsafeReportsCount = 0;
var lines = File.ReadAllLines("input.txt");
var withProblemDampener = true;

foreach (var line in lines)
{
    var levels = line.Split(' ').Select(x => int.Parse(x)).ToList();

    if (IsUnsafe(levels, withProblemDampener))
    {
        unsafeReportsCount++;
    }
}

bool IsUnsafe(List<int> levels, bool withProblemDampener)
{
    var isUnsafe = false;
    int? delta = null;

    for (var i = 1; i < levels.Count; i++)
    {
        if (delta is null)
        {
            delta = levels[i - 1] - levels[i];
            if (delta == 0 || (delta < -3) || (delta > 3))
            {
                isUnsafe = true;
                break;
            }
        }

        var levelsDelta = levels[i - 1] - levels[i];

        if (delta < 0 && (levelsDelta < -3 || levelsDelta >= 0)
            || delta > 0 && (levelsDelta <= 0 || levelsDelta > 3))
        {
            isUnsafe = true;
            break;
        }
    }

    if (withProblemDampener && isUnsafe)
    {
        for (var index = 0; index < levels.Count; index++)
        {
            isUnsafe = IsUnsafe(levels.Where((v, i) => i != index).ToList(), false);
            if (!isUnsafe)
            {
                break;
            }
        }
    }

    return isUnsafe;
}

Console.WriteLine($"safeReportsCount {lines.Length - unsafeReportsCount}");
