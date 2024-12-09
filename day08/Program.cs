var input = File.ReadAllLines("input.txt")
    .Select((x, i) => new { Row = i, Cols = x.ToCharArray() })
    .Aggregate(new Dictionary<Position, char>(), (positions, next) =>
    {
        for (var c = 0; c < next.Cols.Length; c++)
        {
            positions.Add(new Position(c, next.Row), next.Cols[c]);
        }
        return positions;
    });

Console.WriteLine($"Part1: {FindAntinodes(input, Part1)}");
Console.WriteLine($"Part2: {FindAntinodes(input, Part2)}");

void Part1(Position position, Position secondPosition, Dictionary<Position, char> map, List<Position> antinodePositions)
{
    var antinodePosition = new Position(2 * (secondPosition.X - position.X) + position.X, 2 * (secondPosition.Y - position.Y) + position.Y);
    if (map.TryGetValue(antinodePosition, out var _))
    {
        antinodePositions.Add(antinodePosition);
    }
}

void Part2(Position position, Position secondPosition, Dictionary<Position, char> map, List<Position> antinodePositions)
{
    var equation = GetLineEquation(position, secondPosition);

    foreach (var antinodePosition in map.Keys)
    {
        if (antinodePosition.X * equation.A + antinodePosition.Y * equation.B + equation.C == 0)
        {
            antinodePositions.Add(antinodePosition);
        }
    }
}

int FindAntinodes(Dictionary<Position, char> map, Action<Position, Position, Dictionary<Position, char>, List<Position>> impl)
{
    var frequencies = map.Where(x => x.Value != '#' && x.Value != '.').Select(x => x.Value).Distinct().ToList();
    var antinodePositions = new List<Position>();

    foreach (var f in frequencies)
    {
        var frequencyPositions = map.Where(x => x.Value == f).Select(x => x.Key).ToList();
        foreach (var position in frequencyPositions)
        {
            foreach (var secondPosition in frequencyPositions.Where(x => x != position))
            {
                impl(position, secondPosition, map, antinodePositions);
            }
        }
    }
    return antinodePositions.Distinct().Count();
}

Equation GetLineEquation(Position p1, Position p2)
    => new(p2.Y - p1.Y, -(p2.X - p1.X), p1.Y * p2.X - p1.X * p2.Y);

public record Equation(int A, int B, int C);

public record Position(int X, int Y);
