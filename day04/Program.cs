var input = File.ReadAllLines("input.txt");

// part 1

var pattern1 = new char[] { 'X', 'M', 'A', 'S' };
var revPattern1 = new char[] { 'S', 'A', 'M', 'X' };
var toCheck1 = new List<char[]>();

toCheck1.AddRange(GetRows(input, pattern1.Length));
toCheck1.AddRange(GetCols(input, pattern1.Length));
toCheck1.AddRange(GetDiagonals(input, pattern1.Length));

Console.WriteLine($"Part1: {toCheck1.Where(x => x.SequenceEqual(pattern1) || x.SequenceEqual(revPattern1)).Count()}");

// part 2

var pattern2 = new char[]{'M', 'A', 'S'};
var revPattern2 = new char[]{'S', 'A', 'M'};

Console.WriteLine($"Part2: {GetSlices(input, pattern2.Length).Where(x => 
(x.PrimaryDiagonal.SequenceEqual(pattern2) && x.SecondardyDiagonal.SequenceEqual(pattern2))
|| (x.PrimaryDiagonal.SequenceEqual(revPattern2) && x.SecondardyDiagonal.SequenceEqual(revPattern2))
|| (x.PrimaryDiagonal.SequenceEqual(pattern2) && x.SecondardyDiagonal.SequenceEqual(revPattern2))
|| (x.PrimaryDiagonal.SequenceEqual(revPattern2) && x.SecondardyDiagonal.SequenceEqual(pattern2))
).Count()}");

static List<Slice> GetSlices(string[] input, int size)
{
    var slices = new List<Slice>();

    for (var r = 0; r <= input.Length - size; r++)
    {
        for (var c = 0; c <= input[0].Length - size; c++)
        {
            slices.Add(GetSlice(input, r, c, size));
        }
    }
    return slices;
}

static List<char[]> GetRows(string[] lines, int size)
{
    var rows = new List<char[]>();
    for(var r=0; r<lines.Length; r++)
    {
        for(var c=0; c<=lines[r].Length - size; c++)
        {
            rows.Add(lines[r].Skip(c).Take(size).ToArray());
        }
    }
    return rows;
}

static List<char[]> GetCols(string[] lines, int size)
{
    var cols = new List<char[]>();
    for (var c = 0; c < lines[0].Length; c++)
    {
        for (var r = 0; r <= lines.Length - size; r++)
        {
            cols.Add(lines.Skip(r).Take(size).Select(x => x[c]).ToArray());
        }
    }
    return cols;
}

static List<char[]> GetDiagonals(string[] lines, int size)
{
    
    List<char[]> diagonals = [];

    for (var r = 0; r <= lines.Length - size; r++)
    {
        for (var c = 0; c <= lines[r].Length - size; c++)
        {
            var pd = new List<char>();
            var sd = new List<char>();
            for (int i= 0, j = 0; i < size; i++, j++)
            {
                pd.Add(lines[r+i][c+j]);
                sd.Add(lines[r+i][c+size - j - 1]);
            }
            diagonals.Add([.. pd]);
            diagonals.Add([.. sd]);
        }
    }

    return diagonals;
}

static Slice GetSlice(string[] lines, int row, int col, int size)
{
    var sliced = lines
        .Skip(row)
        .Take(size)
        .Select(x => x.Skip(col).Take(size).ToList())
        .Select(x => x)
        .ToList();

    var pd = new List<char>();
    var sd = new List<char>();

    for (int r = 0, c = 0; r < size; r++, c++)
    {
        pd.Add(sliced[r][c]);
        sd.Add(sliced[r][size - c - 1]);
    }

    return new Slice(pd, sd);
}

record Slice(List<char> PrimaryDiagonal, List<char> SecondardyDiagonal);


