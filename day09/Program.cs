Console.WriteLine($"Part1: {Part1()}");
Console.WriteLine($"Part2: {Part2()}");

long Part1()
{
    var disk = ParseInput();

    var i = 0;
    var k = disk.Count;
    var process = true;
    while (process)
    {
        k--;
        if (disk[k].Id != null)
        {
            while (true)
            {
                if (disk[i].Id is null)
                {
                    break;
                }
                i++;
            }
            if (i < k)
            {
                disk[i] = disk[k];
                disk[k] = new Block(null, k);
            }
            else
            {
                process = false;
                break;
            }
        }
    }
    return Count(disk);
}

long Part2()
{
    var disk = ParseInput();
    disk.Where(x => x.Id != null)
        .GroupBy(x => x.Id)
        .Select(g => new DiskFile(g.Key!.Value, [.. g]))
        .Reverse()
        .Aggregate(disk, (acc, file) =>
        {
            var fileLength = file.Blocks.Count;
            var i = FindEmptySlot(acc, fileLength);
            if (i != null && i < file.Blocks[0].Index)
            {
                for (int k = i.Value, f = 0; f < fileLength; k++, f++)
                {
                    acc[k] = file.Blocks[f];
                    acc[file.Blocks[f].Index] = new Block(null, file.Blocks[f].Index);
                }                
            }
            return acc;
        });

    return Count(disk);
}

int? FindEmptySlot(List<Block> disk, int ofLength)
{
    var startIndex = disk.First(x => x.Id == null).Index;

    for (var i = startIndex; i < disk.Count; i++)
    {
        if (disk[i].Id == null)
        {
            for (var k = i + 1; k < disk.Count; k++)
            {
                if (k - i == ofLength)
                {
                    return i;
                }
                if (disk[k].Id != null)
                {
                    break;
                }
            }
        }
    }
    return null;
}

long Count(List<Block> disk)
    => disk.Select((x, i) => x.Id != null ? x.Id.Value * (long)i : 0).Sum();

List<Block> ParseInput() =>
    File.ReadAllText("input.txt")
    .Select((x, i) => new { x, i })
    .Aggregate(new List<Block>(), (acc, next) =>
    {
        var blocksNo = (int)char.GetNumericValue(next.x);
        int? id = null;
        var start = acc.Count;
        if (next.i % 2 == 0)
        {
            id = next.i / 2;
        }
        for (var k = 0; k < blocksNo; k++)
        {
            acc.Add(new Block(id, start + k));
        }
        return acc;
    });

public record DiskFile(int Id, List<Block> Blocks);
public record Block(int? Id, int Index);
