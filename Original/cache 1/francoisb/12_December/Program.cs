using System.Collections.Concurrent;
using System.Collections.Immutable;

using Cache = System.Collections.Generic.Dictionary<string, long>;

var ToIntList = (string str, string delimiter = "") =>
{
    if (delimiter == "")
    {
        List<int> result = new();
        foreach (char c in str) if (int.TryParse(c.ToString(), out int n)) result.Add(n);
        return result;
    }
    else
    {
        return str
            .Split(delimiter)
            .Where(n => int.TryParse(n, out int v))
            .Select(n => Convert.ToInt32(n))
            .ToList();
    }
};

var SplitByNewline = (string input, bool blankLines = false, bool shouldTrim = true) => input
       .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
       .Where(s => blankLines || !string.IsNullOrWhiteSpace(s))
       .Select(s => shouldTrim ? s.Trim() : s)
       .ToList();

var End = (string hotSprings, ImmutableStack<int> damaged, Cache cache) => damaged.Any() ? 0 : 1;
long Dot(string hotSprings, ImmutableStack<int> damaged, Cache cache) => Compute(hotSprings[1..], damaged, cache);
long Question(string hotSprings, ImmutableStack<int> damaged, Cache cache) => Compute("." + hotSprings[1..], damaged, cache) + Compute("#" + hotSprings[1..], damaged, cache);

long Hash(string hotSprings, ImmutableStack<int> damaged, Cache cache)
{
    if (!damaged.Any()) return 0;

    var spring = damaged.Peek();
    damaged = damaged.Pop();

    var pot = hotSprings.TakeWhile(q => q == '#' || q == '?').Count();
    if (pot < spring) return 0;
    else if (hotSprings.Length == spring) return Compute("", damaged, cache);
    else if (hotSprings[spring] == '#') return 0;
    else return Compute(hotSprings[(spring + 1)..], damaged, cache);
};

long Compute(string hotSprings, ImmutableStack<int> damaged, Cache cache)
{
    var pattern = hotSprings + ',' + string.Join(",", damaged.Select(q => q.ToString()));

    if (!cache.ContainsKey(pattern))
    {
        cache[pattern] = CheckChar(hotSprings, damaged, cache);
    }
    return cache[pattern];
}

long CheckChar(string hotSprings, ImmutableStack<int> damaged, Cache cache)
{
    return hotSprings.FirstOrDefault() switch
    {
        '.' => Dot(hotSprings, damaged, cache),
        '?' => Question(hotSprings, damaged, cache),
        '#' => Hash(hotSprings, damaged, cache),
        _ => End(hotSprings, damaged, cache)
    };
};

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) =>
{
    var lineList = SplitByNewline(lines);
    var col = new List<Collection>();
    foreach(var line in lineList)
    {
        var splits = line.Split(' ');
        var spring = splits[0];
        var damaged = ToIntList(splits[1], ",");
        col.Add(new Collection(spring, damaged));
    }

    return col;
};

var ProcessInputStep1 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    long pos = 0;
    foreach(var col in cols)
    {
        var newdamaged = col.DamagedSprings.Select(x => x).Reverse();
        pos += Compute(col.HotSprings, ImmutableStack.CreateRange(newdamaged), new Cache());  
    }
    return pos;
};

var ProcessInputStep2 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    ConcurrentBag<long> pos = new ConcurrentBag<long>();
    Parallel.ForEach(cols, new ParallelOptions() { MaxDegreeOfParallelism = 1000 }, (col, i, thread) =>
    {
        var newSpring = col.HotSprings;
        ConcurrentBag<int> damaged = new ConcurrentBag<int>();
        ConcurrentBag<int> test = new ConcurrentBag<int>();
        for (var x = 0; x < 4; x++)
        {
            newSpring += "?" + col.HotSprings;
        }
        var newdamaged = Enumerable.Repeat(col.DamagedSprings, 5).SelectMany(q => q).Reverse();
        pos.Add(Compute(newSpring, ImmutableStack.CreateRange(newdamaged), new Cache()));
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
