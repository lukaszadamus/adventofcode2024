using System.Text.RegularExpressions;

var setups = ParseInput();

Console.WriteLine($"Part1: {Loop(setups, 0)}");
Console.WriteLine($"Part2: {Loop(setups, 10000000000000)}");

decimal Loop(List<Setup> clawMachines, long add)
{
    decimal tokens = 0;

    foreach (var setup in clawMachines)
    {
        decimal? cost = FindCost(setup, add);
        if (cost != null)
        {
            tokens += cost.Value;
        }
    }
    return tokens;
}

decimal? FindCost(Setup setup, long add)
{
    decimal prizeX = setup.PrizeX + add;
    decimal prizeY = setup.PrizeY + add;

    decimal b = (prizeY * setup.AoffsetX - prizeX * setup.AoffsetY) / (setup.BoffsetY * setup.AoffsetX - setup.BoffsetX * setup.AoffsetY);
    decimal a = (prizeX - b * setup.BoffsetX) / setup.AoffsetX;

    return b > 0 && decimal.IsInteger(b) && a > 0 && decimal.IsInteger(a) ? a * 3M + b * 1M : null;
}

static List<Setup> ParseInput()
{
    var aRegex = new Regex(@"^Button A: X[+](?<x>\d+), Y[+](?<y>\d+)$");
    var bRegex = new Regex(@"^Button B: X[+](?<x>\d+), Y[+](?<y>\d+)$");
    var pRegex = new Regex(@"^Prize: X=(?<x>\d+), Y=(?<y>\d+)$");

    decimal aOffsetX = 0;
    decimal aOffsetY = 0;
    decimal bOffsetX = 0;
    decimal bOffsetY = 0;
    var setups = new List<Setup>();

    foreach (var line in File.ReadAllLines("input.txt"))
    {
        if (aRegex.IsMatch(line))
        {
            var match = aRegex.Match(line);
            aOffsetX = decimal.Parse(match.Groups["x"].Value);
            aOffsetY = decimal.Parse(match.Groups["y"].Value);
        }
        else if (bRegex.IsMatch(line))
        {
            var match = bRegex.Match(line);
            bOffsetX = decimal.Parse(match.Groups["x"].Value);
            bOffsetY = decimal.Parse(match.Groups["y"].Value);
        }
        else if (pRegex.IsMatch(line))
        {
            var match = pRegex.Match(line);
            var prizeX = decimal.Parse(match.Groups["x"].Value);
            var prizeY = decimal.Parse(match.Groups["y"].Value);

            setups.Add(new Setup(aOffsetX, aOffsetY, bOffsetX, bOffsetY, prizeX, prizeY));
        }
    }

    return setups;
}
public record Setup(decimal AoffsetX, decimal AoffsetY, decimal BoffsetX, decimal BoffsetY, decimal PrizeX, decimal PrizeY);