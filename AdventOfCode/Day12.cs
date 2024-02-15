namespace AdventOfCode;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using Cache = System.Collections.Generic.Dictionary<string, long>;

public class Day12 : BaseDay
{
    private readonly string _input;

    public Day12()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    private List<int> ToIntList (string str, string delimiter = "")
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
    }
    private long End(string hotSprings, ImmutableStack<int> damaged, Cache cache) => damaged.Any() ? 0 : 1;
    private long Dot(string hotSprings, ImmutableStack<int> damaged, Cache cache) => Compute(hotSprings[1..], damaged, cache);
    private long Question(string hotSprings, ImmutableStack<int> damaged, Cache cache) => Compute("." + hotSprings[1..], damaged, cache) + Compute("#" + hotSprings[1..], damaged, cache);

    private long Hash(string hotSprings, ImmutableStack<int> damaged, Cache cache)
    {
        if (!damaged.Any()) return 0;

        var spring = damaged.Peek();
        damaged = damaged.Pop();

        var pot = hotSprings.TakeWhile(q => q == '#' || q == '?').Count();
        if (pot < spring) return 0;
        else if (hotSprings.Length == spring) return Compute("", damaged, cache);
        else if (hotSprings[spring] == '#') return 0;
        else return Compute(hotSprings[(spring + 1)..], damaged, cache);
    }

    private long Compute(string hotSprings, ImmutableStack<int> damaged, Cache cache)
    {
        var pattern = hotSprings + ',' + string.Join(",", damaged.Select(q => q.ToString()));

        if (!cache.ContainsKey(pattern))
        {
            cache[pattern] = CheckChar(hotSprings, damaged, cache);
        }
        return cache[pattern];
    }

    private long CheckChar(string hotSprings, ImmutableStack<int> damaged, Cache cache)
    {
        return hotSprings.FirstOrDefault() switch
        {
            '.' => Dot(hotSprings, damaged, cache),
            '?' => Question(hotSprings, damaged, cache),
            '#' => Hash(hotSprings, damaged, cache),
            _ => End(hotSprings, damaged, cache)
        };
    }

    private List<Collection12> GenerateCollections(string lines)
    {
        var lineList = lines.SplitByNewline();
        var col = new List<Collection12>();
        foreach (var line in lineList)
        {
            var splits = line.Split(' ');
            var spring = splits[0];
            var damaged = ToIntList(splits[1], ",");
            col.Add(new Collection12(spring, damaged));
        }

        return col;
    }

    private long ProcessInput1()
    {
        var cols = GenerateCollections(_input);
        long pos = 0;
        foreach (var col in cols)
        {
            var newdamaged = col.DamagedSprings.Select(x => x).Reverse();
            pos += Compute(col.HotSprings, ImmutableStack.CreateRange(newdamaged), new Cache());
        }
        return pos;
    }

    private long ProcessInput2()
    {
        var cols = GenerateCollections(_input);
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
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}

public class Collection12(string hotsprings, List<int> damaged)
{
    public string HotSprings { get; set; } = hotsprings;
    public List<int> DamagedSprings { get; set; } = damaged;
}
