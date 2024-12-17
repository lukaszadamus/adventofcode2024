using System.Text;
using System.Text.RegularExpressions;

var input = ParseInput();
Console.WriteLine($"Part1: {Part1(input, 101, 103)}");

input = ParseInput();
Console.WriteLine($"Part2: {Part2(input, 101, 103)}");

int Part1(List<Robot> robots, int xSize, int ySize)
{
    for (var s = 0; s < 100; s++)
    {
        foreach (var robot in robots)
        {
            robot.Move(xSize, ySize);
        }
    }

    return robots
        .Where(x => x.Quadrant(xSize, ySize) != null)
        .GroupBy(x => x.Quadrant(xSize, ySize))
        .Select(g => g.Count())
        .Aggregate(1, (acc, next) => acc * next);

}

int Part2(List<Robot> robots, int xSize, int ySize)
{
    var second = 0;
    while (true)
    {        
        second++;
        foreach (var robot in robots)
        {
            robot.Move(xSize, ySize);
        }
        var lines = GetLines(xSize, ySize, robots);
        if(lines.Any(x => x.Contains("#################")))
        {
            Print(xSize, ySize, robots, second, lines);
            return second;
        }                
    }
}

static List<Robot> ParseInput()
{
    var regex = new Regex(@"p=(?<x>-?\d+),(?<y>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)");

    var robots = new List<Robot>();

    foreach (var (line, i) in File.ReadAllLines("input.txt").Select((value, i) => ( value, i )))
    {
        if (regex.IsMatch(line))
        {
            var match = regex.Match(line);
            var x = int.Parse(match.Groups["x"].Value);
            var y = int.Parse(match.Groups["y"].Value);
            var vx = int.Parse(match.Groups["vx"].Value);
            var vy = int.Parse(match.Groups["vy"].Value);

            robots.Add(new Robot(x, y, vx, vy, i));
        }
    }

    return robots;
}

List<string> GetLines(int xSize, int ySize, List<Robot> robots)
{
    var lines = new List<string>();

    for(var y=0; y<ySize; y++)
    {
        var sb = new StringBuilder();
        for(var x=0; x<xSize; x++)
        {            
            var robot = robots.FirstOrDefault(r => r.X == x && r.Y == y);
            if(robot is null)
            {
                sb.Append(".");
            }
            else
            {
                sb.Append(robot.Id);
            }

        }
        lines.Add(sb.ToString());
    }

    return lines;
}

void Print(int xSize, int ySize, List<Robot> robots, int frame, List<string>? lines = null)
{
    Console.WriteLine(frame);    

    lines ??= GetLines(xSize, ySize, robots);

    foreach(var line in GetLines(xSize, ySize, robots))
    {
        Console.WriteLine(line);
    }
    Console.WriteLine("");    
}

public record Velocity(int X, int Y);
public class Robot
{
    public char Id { get; private set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Velocity Velocity { get; set; } = new Velocity(0, 0);

    public Robot(int x, int y, int vx, int vy, int id)
    {
        X = x;
        Y = y;
        Velocity = new Velocity(vx, vy);
        Id = Convert.ToChar(35);
    }

    private int Mid(int size)
        => (size - 1) / 2;

    public bool IsInQ1(int xSize, int ySize)
        => X < Mid(xSize) && Y < Mid(ySize);
    public bool IsInQ2(int xSize, int ySize)
        => X > Mid(xSize) && Y < Mid(ySize);
    public bool IsInQ3(int xSize, int ySize)
        => X > Mid(xSize) && Y > Mid(ySize);
    public bool IsInQ4(int xSize, int ySize)
        => X < Mid(xSize) && Y > Mid(ySize);

    public int? Quadrant(int xSize, int ySize)
    {
        if (IsInQ1(xSize, ySize))
        {
            return 1;
        }
        else if (IsInQ2(xSize, ySize))
        {
            return 2;
        }
        else if (IsInQ3(xSize, ySize))
        {
            return 3;
        }
        else if (IsInQ4(xSize, ySize))
        {
            return 4;
        }
        else
        {
            return null;
        }
    }

    public void Move(int xSize, int ySize)
    {
        var x = X + Velocity.X;
        var y = Y + Velocity.Y;

        X = Calculate(x, xSize);
        Y = Calculate(y, ySize);

        static int Calculate(int coordinate, int size)
        {
            if (coordinate > 0)
            {
                return coordinate % size;
            }
            else if (coordinate < 0)
            {
                var nc = size - Math.Abs(coordinate) % size;
                return nc == size ? 0 : nc;
            }
            else
            {
                return coordinate;
            }
        }
    }
}
