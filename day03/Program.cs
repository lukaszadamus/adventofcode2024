using System.Text.RegularExpressions;

var rxMul1 = new Regex(@"((?<mul>mul)[(]{1}(?<left>\d{1,3})[,]{1}(?<right>\d{1,3})[)]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
var result1 = rxMul1.Matches(File.ReadAllText("input.txt")).Select(x => int.Parse(x.Groups["left"].Value) * int.Parse(x.Groups["right"].Value)).Sum();
Console.WriteLine(result1);

var rxMul2 = new Regex(@"(?<do>do[(]{1}[)]{1})|(?<dont>don't[(]{1}[)]{1})|((?<mul>mul)[(]{1}(?<left>\d{1,3})[,]{1}(?<right>\d{1,3})[)]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
var count = true;

var result2 = rxMul2.Matches(File.ReadAllText("input.txt")).Select(x =>
{
    var value = 0;
    if (x.Value == "do()")
    {
        count = true;
    }
    else if (x.Value == "don't()")
    {
        count = false;
    }
    else
    {
        value = count ? int.Parse(x.Groups["left"].Value) * int.Parse(x.Groups["right"].Value) : 0;
    }
    return value;
}).Sum();
Console.WriteLine(result2);
