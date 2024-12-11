var map = ParseInput();

var result = map
    .Where(x => x.Value == 0)
    .Aggregate(new List<HikingTrail>(), (acc, start) =>
    {
        var hikingTrails = new List<HikingTrail>();
        Discover(map, start.Key, new Stack<Position>(), hikingTrails);
        acc.AddRange(hikingTrails);
        return acc;
    });

var count = result.GroupBy(x => x.Positions.First()).Select(g => g.Count()).Sum();

Console.WriteLine($"Part1: {Score1(result)}");
Console.WriteLine($"Part2: {Score2(result)}");

void Discover(Dictionary<Position, int> map, Position currentPosition, Stack<Position> currentHikingTrail, List<HikingTrail> hikingTrails)
{
    if (IsHead(map, currentPosition))
    {
        currentHikingTrail.Push(currentPosition);
    }
    else if (IsEnd(map, currentPosition))
    {
        var positions = currentHikingTrail.ToList();
        positions.Reverse();
        hikingTrails.Add(new HikingTrail(positions.First(), positions.Last(), positions));
        return;
    }

    foreach (var nextPosition in NextPositions(map, currentPosition))
    {
        currentHikingTrail.Push(nextPosition);
        Discover(map, nextPosition, currentHikingTrail, hikingTrails);
        currentHikingTrail.Pop();
    }
}

bool IsEnd(Dictionary<Position, int> map, Position position)
    => map.TryGetValue(position, out var value) && value == 9;

bool IsHead(Dictionary<Position, int> map, Position position)
    => map.TryGetValue(position, out var value) && value == 0;

bool IsValidNextPosition(Dictionary<Position, int> map, Position nextPosition, int currentHeight)
     => map.TryGetValue(nextPosition, out var value) && value - currentHeight == 1;

List<Position> NextPositions(Dictionary<Position, int> map, Position currentPosition)
{
    var positions = new List<Position>();
    var currentHeight = map[currentPosition];
    var nextPosition = MoveDown(map, currentPosition, currentHeight);
    if (nextPosition != null)
    {
        positions.Add(nextPosition);
    }

    nextPosition = MoveUp(map, currentPosition, currentHeight);
    if (nextPosition != null)
    {
        positions.Add(nextPosition);
    }

    nextPosition = MoveLeft(map, currentPosition, currentHeight);
    if (nextPosition != null)
    {
        positions.Add(nextPosition);
    }

    nextPosition = MoveRight(map, currentPosition, currentHeight);
    if (nextPosition != null)
    {
        positions.Add(nextPosition);
    }
    return positions;
}

Position? MoveDown(Dictionary<Position, int> map, Position position, int height)
{
    var nextPosition = new Position(position.X, position.Y + 1);
    return IsValidNextPosition(map, nextPosition, height) ? nextPosition : null;
}

Position? MoveUp(Dictionary<Position, int> map, Position position, int height)
{
    var nextPosition = new Position(position.X, position.Y - 1);
    return IsValidNextPosition(map, nextPosition, height) ? nextPosition : null;
}

Position? MoveLeft(Dictionary<Position, int> map, Position position, int height)
{
    var nextPosition = new Position(position.X - 1, position.Y);
    return IsValidNextPosition(map, nextPosition, height) ? nextPosition : null;
}

Position? MoveRight(Dictionary<Position, int> map, Position position, int height)
{
    var nextPosition = new Position(position.X + 1, position.Y);
    return IsValidNextPosition(map, nextPosition, height) ? nextPosition : null;
}

int Score1(List<HikingTrail> hikingTrails)
    => hikingTrails.GroupBy(x => x.Head).Select(g => new { g.Key, count = g.Select(x => x.End).Distinct().Count() }).Sum(x => x.count);

int Score2(List<HikingTrail> hikingTrails)
    => hikingTrails.GroupBy(x => x.Positions.First()).Select(g => g.Count()).Sum();

Dictionary<Position, int> ParseInput()
    => File.ReadAllLines("input.txt")
        .Select((x, i) => new { Row = i, Cols = x.Select(x => (int)char.GetNumericValue(x)).ToList() })
        .Aggregate(new Dictionary<Position, int>(), (positions, next) =>
        {
            for (var c = 0; c < next.Cols.Count; c++)
            {
                positions.Add(new Position(c, next.Row), next.Cols[c]);
            }
            return positions;
        });

public record HikingTrail(Position Head, Position End, List<Position> Positions);
public record Position(int X, int Y);

