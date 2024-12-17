using System.Runtime.CompilerServices;

Console.WriteLine($"Part1: {Part1(ParseInput("input1.txt"))}");

int Part1(Puzzle puzzle)
{
    //Print(puzzle.Map);
    foreach (var move in puzzle.Moves)
    {
        if (move == '^' && CanUp(puzzle.Map.RobotPosition, puzzle.Map))
        {
            Shift(puzzle.Map.RobotPosition, puzzle.Map, 0, -1);
        }
        else if (move == '>' && CanRight(puzzle.Map.RobotPosition, puzzle.Map))
        {
            Shift(puzzle.Map.RobotPosition, puzzle.Map, 1, 0);
        }
        else if (move == 'v' && CanDown(puzzle.Map.RobotPosition, puzzle.Map))
        {
            Shift(puzzle.Map.RobotPosition, puzzle.Map, 0, 1);
        }
        else if (move == '<' && CanLeft(puzzle.Map.RobotPosition, puzzle.Map))
        {
            Shift(puzzle.Map.RobotPosition, puzzle.Map, -1, 0);
        }
        //Print(puzzle.Map);
    }
    return puzzle.Map.Tiles.Where(x => x.Value == 'O').Select(x => 100 * x.Key.Y + x.Key.X).Sum();
}

void Print(Map map)
{
    foreach (var line in map.Tiles.GroupBy(x => x.Key.Y).Select(g => new string(g.Select(x => x.Value).ToArray())))
    {
        Console.WriteLine(line);
    }
}

List<Position> ToShift(Position current, Map map, int directionX, int directionY)
{

    var toShift = new List<Position>();

    while (true)
    {
        if (map.Tiles[current] == '.')
        {
            break;
        }
        toShift.Add(current);
        current = new Position(current.X + directionX, current.Y + directionY);
    }

    toShift.Reverse();

    return toShift;
}

List<char> ToWall(Position current, Map map, int directionX, int directionY)
{

    var tiles = new List<char>();

    while (true)
    {
        if (map.Tiles[current] == '#')
        {
            break;
        }
        tiles.Add(map.Tiles[current]);
        current = new Position(current.X + directionX, current.Y + directionY);
    }

    return tiles;
}



void Shift(Position robot, Map map, int directionX, int directionY)
{
    foreach (var tile in ToShift(robot, map, directionX, directionY))
    {
        ShiftTile(tile, map, directionX, directionY);
    }
}

void ShiftTile(Position position, Map map, int shiftX, int shiftY)
{    
    var shifted = new Position(position.X + shiftX, position.Y + shiftY);
    //Console.WriteLine($"{map.Tiles[position]} to {map.Tiles[shifted]} by ({shiftX},{shiftY})");
    map.Tiles[shifted] = map.Tiles[position];
    map.Tiles[position] = '.';
    if (map.Tiles[shifted] == '@')
    {
        map.RobotPosition = shifted;
    }
}

bool CanUp(Position robotPosition, Map map)
    => ToWall(robotPosition, map, 0, -1).Any(x => x == '.');
bool CanRight(Position robotPosition, Map map)
    => ToWall(robotPosition, map, 1, 0).Any(x => x == '.');
bool CanDown(Position robotPosition, Map map)
    => ToWall(robotPosition, map, 0, 1).Any(x => x == '.');
bool CanLeft(Position robotPosition, Map map)
    => ToWall(robotPosition, map, -1, 0).Any(x => x == '.');

Puzzle ParseInput(string file)
{
    var parsingMap = true;
    var mapY = 0;
    var tiles = new Dictionary<Position, char>();
    var robotPosition = new Position(0, 0);
    var moves = new List<char>();

    foreach (var line in File.ReadAllLines(file))
    {
        if (line == "")
        {
            parsingMap = false;
        }
        else
        {
            if (parsingMap)
            {
                var chars = line.ToCharArray();
                for (var x = 0; x < chars.Length; x++)
                {
                    tiles[new Position(x, mapY)] = chars[x];
                    if (chars[x] == '@')
                    {
                        robotPosition = new Position(x, mapY);
                    }
                }
                mapY++;
            }
            else
            {
                moves.AddRange(line.ToCharArray().ToList());
            }
        }
    }

    return new Puzzle(new Map { Tiles = tiles, RobotPosition = robotPosition }, moves);
}

public record Position(int X, int Y);
public record Map
{
    public required Dictionary<Position, char> Tiles { get; init; }
    public required Position RobotPosition { get; set; }
}
public record Puzzle(Map Map, List<char> Moves);