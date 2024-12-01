var listA = new List<int>();
var listB = new List<int>();

foreach(var line in File.ReadAllLines("input.txt"))
{
    var ids = line.Split("   ");
    listA.Add(int.Parse(ids[0]));
    listB.Add(int.Parse(ids[1]));
}    

listA.Sort();
listB.Sort();

var totalDistance = 0L;
var similarityScore = 0L;

for(var i=0; i<listA.Count; i++)
{
    totalDistance += Math.Abs(listA[i]-listB[i]);
    similarityScore += listA[i] * listB.Where(x => x == listA[i]).Count();
}

Console.WriteLine($"totalDistance {totalDistance}");
Console.WriteLine($"similarityScore {similarityScore}");