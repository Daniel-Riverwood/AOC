using System.Collections.Concurrent;
using Utilities;

int Calc(string hotSpring, List<int> remaining)
{
    if (string.IsNullOrWhiteSpace(hotSpring))
    {
        if (remaining.Count() == 0) return 1;
        else return 0;
    }

    switch(hotSpring.First())
    {
        case '.':
            return Calc(hotSpring.Substring(1), remaining);
        case '#':
            return perm(remaining, hotSpring);
        case '?':
            return Calc(hotSpring.Substring(1), remaining) + perm(remaining, hotSpring);
        default:
            return 0;
    }
}

int perm (List<int> remaining, string hotSpring)
{
    if (remaining.Count() == 0)
    {
        return 0;
    }

    int pos = remaining[0];
    if (hotSpring.Count() < pos) return 0;
    for (var x = 0; x < pos; x++)
    {
        if (hotSpring[x] == '.') return 0;
    }
    if(hotSpring.Count() == pos)
    {
        if (remaining.Count() == 1) return 1;
        else return 0;
    }
    if (hotSpring[pos] == '#') return 0;
    return Calc(hotSpring.Substring(pos + 1), remaining.GetRange(1, remaining.Count() -1));
}

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) =>
{
    var lineList = lines.SplitByNewline();
    var col = new List<Collection>();
    foreach(var line in lineList)
    {
        var splits = line.Split(' ');
        var spring = splits[0];
        var damaged = splits[1].ToIntList(",");
        col.Add(new Collection(spring, damaged));
    }

    return col;
};

var ProcessInputStep1 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    int pos = 0;
    foreach(var col in cols)
    {
        pos += Calc(col.HotSprings, col.DamagedSprings);  
    }
    return pos;
};

var ProcessInputStep2 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    ConcurrentBag<int> pos = new ConcurrentBag<int>();
    ConcurrentBag<int> test = new ConcurrentBag<int>();
    Parallel.ForEach(cols, new ParallelOptions() { MaxDegreeOfParallelism = 1000 }, (col, i, thread) =>
    {
        var newSpring = col.HotSprings;
        for (var x = 0; x < 4; x++)
        {
            newSpring += "?" + col.HotSprings;
        }
        col.HotSprings = newSpring;
        var damaged = Enumerable.Repeat(col.DamagedSprings, 5).SelectMany(q => q).ToList();
        col.DamagedSprings = damaged;
        pos.Add(Calc(col.HotSprings, col.DamagedSprings));
        test.Add(1);
        PrintOutput($"{cols.Count() - test.Count()} Remaining");
    });
    return pos.Sum();
};

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);


public class Collection(string hotsprings, List<int> damaged)
{
    public string HotSprings { get; set; } = hotsprings;
    public List<int> DamagedSprings { get; set; } = damaged;
}
