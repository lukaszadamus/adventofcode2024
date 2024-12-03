using System.Text.RegularExpressions;


var lexer = new Lexer();
var tokens = lexer.Instructions(File.ReadAllText("input.txt"));

var computer = new Computer(tokens);
computer.Run();

Console.WriteLine($"part1: {computer.DoRegistry + computer.DontRegistry}");
Console.WriteLine($"part2: {computer.DoRegistry}");

class Computer(MatchCollection instructions)
{
    private readonly MatchCollection _instructions = instructions;
    public long DoRegistry { get; private set; } = 0;
    public long DontRegistry { get; private set; } = 0;
    public bool Do { get; private set; } = true;

    public void Run()
    {
        foreach (Match instruction in _instructions)
        {
            var operation = instruction.Groups["operation"].Value;

            switch (operation)
            {
                case "do":
                    Do = true;
                    break;
                case "don't":
                    Do = false;
                    break;
                case "mul":
                    var result = int.Parse(instruction.Groups["left"].Value) * int.Parse(instruction.Groups["right"].Value);
                    if (Do)
                    {
                        DoRegistry += result;
                    }
                    else
                    {
                        DontRegistry += result;
                    }

                    break;
                default:
                    break;
            }
        }
    }
}

partial class Lexer
{
    private readonly Regex _regex = MyRegex();
    [GeneratedRegex(@"(?<operation>mul|do|don't)(\(\)|\((?<left>\d{1,3}),(?<right>\d{1,3})\))", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex MyRegex();
    public MatchCollection Instructions(string input) => _regex.Matches(input);
}