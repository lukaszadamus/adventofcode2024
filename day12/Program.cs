var regions = DiscoverRegions(ParseInput());

Console.WriteLine($"Part1: {Price(regions)}");

int Price(List<Region> regions)
{
    var price = 0;
    foreach(var r in regions)
    {
        foreach(var p in r.Plots)
        {
            price += p.FenceLength * r.Plots.Count;
        }
    }

    return price;
}

List<Region> DiscoverRegions(Dictionary<Position, Plot> map)
{
    var regions = new List<Region>();
    var cache = new HashSet<Position>();
    
    foreach(var p in map)
    {
        if(cache.Contains(p.Key))
        {
            continue;
        }

        var plots = new List<Plot>();

        Walk(p.Key, map, plots, cache);

        if(plots.Any())
        {
            regions.Add(new Region(plots));
        }
    }

    return regions;
}

void Walk(Position position, Dictionary<Position, Plot> map, List<Plot> plots, HashSet<Position> cache)
{
    if(cache.Contains(position))
    {
        return;
    }

    var currentPlot = map[position];

    plots.Add(currentPlot);
    cache.Add(position); 

    var neighbors = GetNeighbors(position, map.ToDictionary(k => k.Key, v => v.Value.PlantType))
                        .Where(x => map[x].PlantType == currentPlot.PlantType);

    foreach(var neighbor in neighbors)
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
        var neighbors = GetNeighbors(p.Key, map).Where(x => map[x] == map[p.Key]);        
        plots.Add(p.Key, new Plot(map[p.Key], 4 - neighbors.Count()));
    }

    return plots;
}

List<Position> GetNeighbors(Position position, Dictionary<Position, char> map)
{
    var neighbors = new List<Position>();

    Add(position.X, position.Y - 1);
    Add(position.X + 1, position.Y);
    Add(position.X, position.Y + 1);
    Add(position.X -1, position.Y);
    
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
public record Plot(char PlantType, int FenceLength);
public record Region(List<Plot> Plots);
