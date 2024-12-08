var map = File.ReadAllLines("input.txt")
    .Select((x, i) => new { Row = i, Cols = x.ToCharArray() })
    .Aggregate(new Dictionary<Position, char>(), (positions, next) =>
    {
        for (var c = 0; c < next.Cols.Length; c++)
        {
            positions.Add(new Position(c, next.Row), next.Cols[c]);
        }
        return positions;
    });

var startPosition = map.Where(x => x.Value == '^').Select(x => x.Key).First();
var firstMove = new Position(0, -1);

var part1 = Part1(startPosition, firstMove, map, []);
Console.WriteLine($"Part1: {part1.Select(x => x.Position).Distinct().Count()}");

var part2 = new List<Position>();
for (var i = 1; i < part1.Count; i++)
{
    var position = part1[i].Position;

    if (map[position] == '.' && !part2.Contains(position))
    {
        map[position] = '#';

        if (Part2(startPosition, firstMove, map, []))
        {
            part2.Add(position);
        }

        map[position] = '.';
    }
}
Console.WriteLine($"Part2: {part2.Distinct().Count()}");

static List<Complex> Part1(Position currentPosition, Position move, Dictionary<Position, char> map, List<Complex> route)
{
    var next = new Position(currentPosition.X + move.X, currentPosition.Y + move.Y);

    if (!map.TryGetValue(next, out var c))
    {
        route.Add(new(currentPosition, move));
        return route;
    }

    if (c == '#')
    {
        move = NextMove(move);
        return Part1(currentPosition, move, map, route);
    }
    else
    {
        route.Add(new(currentPosition, move));
        return Part1(next, move, map, route);
    }
}

static bool Part2(Position currentPosition, Position move, Dictionary<Position, char> map, List<Complex> route)
{
    var next = new Position(currentPosition.X + move.X, currentPosition.Y + move.Y);

    if (!map.TryGetValue(next, out var c))
    {
        route.Add(new Complex(currentPosition, move));
        return false;
    }
    else if (route.Contains(new Complex(currentPosition, move)))
    {
        return true;
    }

    if (c == '#')
    {
        move = NextMove(move);
        return Part2(currentPosition, move, map, route);
    }
    else
    {
        route.Add(new Complex(currentPosition, move));
        return Part2(next, move, map, route);
    }
}

static Position NextMove(Position currentMove) => currentMove switch
{
    (0, -1) => new Position(1, 0),
    (1, 0) => new Position(0, 1),
    (0, 1) => new Position(-1, 0),
    (-1, 0) => new Position(0, -1),
    _ => throw new NotImplementedException(),
};

record Complex(Position Position, Position Move);

record Position(int X, int Y);