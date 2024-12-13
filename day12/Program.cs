var regions = DiscoverRegions(ParseInput());

Console.WriteLine($"Part1: {Price1(regions)}");
Console.WriteLine($"Part2: {Price2(regions)}");


int Price1(List<Region> regions)
{
    var price = 0;
    foreach (var r in regions)
    {
        foreach (var p in r.Plots)
        {
            price += p.FenceLength * r.Plots.Count;
        }
    }

    return price;
}

int Price2(List<Region> regions)
{
    var price = 0;
    foreach (var region in regions)
    {
        var edges = 0;
        edges += region.Plots
            .Where(x => x.NorthNeighbor != null)
            .GroupBy(x => x.NorthNeighbor!.Y)
            .Select(g => new {g.Key, segments = CountSegments(g.Select(x => x.NorthNeighbor!.X).ToList())})
            .Select(x => x.segments).Sum();
        edges += region.Plots
            .Where(x => x.EastNeighbor != null)
            .GroupBy(x => x.EastNeighbor!.X)
            .Select(g => new {g.Key, segments = CountSegments(g.Select(x => x.EastNeighbor!.Y).ToList())})
            .Select(x => x.segments).Sum();
        edges += region.Plots
            .Where(x => x.SouthNeighbour != null)
            .GroupBy(x => x.SouthNeighbour!.Y)
            .Select(g => new {g.Key, segments = CountSegments(g.Select(x => x.SouthNeighbour!.X).ToList())})
            .Select(x => x.segments).Sum();
        edges += region.Plots
            .Where(x => x.WestNeighbor != null)
            .GroupBy(x => x.WestNeighbor!.X)
            .Select(g => new {g.Key, segments = CountSegments(g.Select(x => x.WestNeighbor!.Y).ToList())})
            .Select(x => x.segments).Sum();

        price += region.Plots.Count * edges;
    }

    return price;

    int CountSegments(List<int> sequence)
    {
        sequence.Sort();
        int count = 1;
        int prev = sequence.First();

        for (var i = 0; i < sequence.Count; i++)
        {
            if(sequence[i] - prev > 1)
            {
                count++;
            }
            prev = sequence[i];
        }

        return count;
    }
}

List<Region> DiscoverRegions(Dictionary<Position, Plot> map)
{
    var regions = new List<Region>();
    var cache = new HashSet<Position>();

    foreach (var p in map)
    {
        if (cache.Contains(p.Key))
        {
            continue;
        }

        var plots = new List<Plot>();

        Walk(p.Key, map, plots, cache);

        if (plots.Count > 0)
        {
            regions.Add(new Region(plots));
        }
    }

    return regions;
}

void Walk(Position position, Dictionary<Position, Plot> map, List<Plot> plots, HashSet<Position> cache)
{
    if (cache.Contains(position))
    {
        return;
    }

    var currentPlot = map[position];

    plots.Add(currentPlot);
    cache.Add(position);

    var neighbors = GetNeighbors(position, map.ToDictionary(k => k.Key, v => v.Value.PlantType))
                        .Where(x => map[x].PlantType == currentPlot.PlantType);

    foreach (var neighbor in neighbors)
    {
        Walk(neighbor, map, plots, cache);
    }
}

Dictionary<Position, Plot> ParseInput()
{
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

    var plots = new Dictionary<Position, Plot>();

    foreach (var p in map)
    {
        var plantType = map[p.Key];
        var neighbors = GetNeighbors(p.Key, map).Where(x => map[x] == plantType);
        plots.Add(p.Key, new Plot(
            plantType,
            4 - neighbors.Count(),
            GetNeighbor(p.Key.X, p.Key.Y - 1, plantType, map),
            GetNeighbor(p.Key.X + 1, p.Key.Y, plantType, map),
            GetNeighbor(p.Key.X, p.Key.Y + 1, plantType, map),
            GetNeighbor(p.Key.X - 1, p.Key.Y, plantType, map)));
    }

    return plots;
}

Position? GetNeighbor(int x, int y, char plantType, Dictionary<Position, char> map)
{
    var neighbor = new Position(x, y);
    return !map.TryGetValue(neighbor, out char neighborPlantType) ? new Position(x, y) : neighborPlantType != plantType ? neighbor : null;
}

List<Position> GetNeighbors(Position position, Dictionary<Position, char> map)
{
    var neighbors = new List<Position>();

    Add(position.X, position.Y - 1);
    Add(position.X + 1, position.Y);
    Add(position.X, position.Y + 1);
    Add(position.X - 1, position.Y);

    return neighbors;

    void Add(int x, int y)
    {
        var nextPosition = new Position(x, y);
        if (map.ContainsKey(nextPosition))
        {
            neighbors.Add(nextPosition);
        }
    }
}

public record Position(int X, int Y);
public record Plot(char PlantType, int FenceLength, Position? NorthNeighbor, Position? EastNeighbor, Position? SouthNeighbour, Position? WestNeighbor);
public record Region(List<Plot> Plots);
