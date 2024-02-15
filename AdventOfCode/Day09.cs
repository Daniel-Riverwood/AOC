using System.Collections.Concurrent;

namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly List<string> _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
    }
    private Collection9 GenerateCollections(List<string> lines)
    {
        var res = new List<Map9>();
        var steps = lines[0].Select(ch => ch).ToList();

        foreach (var line in lines)
        {
            var temp = line.Split(' ').Select(x => Convert.ToInt64(x)).ToList();
            res.Add(new Map9(new List<List<long>>() { temp }));
        }

        return new Collection9(res);
    }
    private long ProcessInput1 ()
    {
        var collection = GenerateCollections(_input);
        ConcurrentBag<long> totals = new ConcurrentBag<long>();
        Parallel.ForEach(collection.Maps, (map, i, thread) =>
        {
            var complete = false;
            var x = 0;
            while (!complete)
            {
                long prev = 0;
                long div = 0;
                List<long> nextnode = new List<long>();
                for (var y = 1; y < map.Nodes[x].Count(); y++)
                {
                    div = map.Nodes[x][y] - map.Nodes[x][y - 1];
                    nextnode.Add(div);
                }

                map.Nodes.Add(nextnode);
                if (nextnode.All(x => x == 0)) complete = true;
                x++;
            }

            for (var y = map.Nodes.Count() - 1; y > 0; y--)
            {
                var cur = map.Nodes[y].Last();
                if (cur == 0) continue;
                var prev = map.Nodes[y - 1].Last();
                map.Nodes[y - 1].Add(cur + prev);
            }

            totals.Add(map.Nodes.First().Last());
        });

        return totals.Sum();
    }

    private long ProcessInput2 ()
    {
        var collection = GenerateCollections(_input);
        ConcurrentBag<long> totals = new ConcurrentBag<long>();
        Parallel.ForEach(collection.Maps, (map, i, thread) =>
        {
            var complete = false;
            var x = 0;
            while (!complete)
            {
                long prev = 0;
                long div = 0;
                List<long> nextnode = new List<long>();
                for (var y = 1; y < map.Nodes[x].Count(); y++)
                {
                    div = map.Nodes[x][y] - map.Nodes[x][y - 1];
                    nextnode.Add(div);
                }

                map.Nodes.Add(nextnode);
                if (nextnode.All(x => x == 0)) complete = true;
                x++;
            }

            for (var y = map.Nodes.Count() - 1; y > 0; y--)
            {
                var cur = map.Nodes[y].First();
                if (cur == 0) continue;
                var prev = map.Nodes[y - 1].First();
                map.Nodes[y - 1] = map.Nodes[y - 1].Prepend(prev - cur).ToList();
            }

            totals.Add(map.Nodes.First().First());
        });

        return totals.Sum();
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}, part 1");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}, part 2");
}
public class Collection9(List<Map9> maps)
{
    public List<Map9> Maps { get; set; } = maps;
}

public class Map9(List<List<long>> nodes)
{
    public List<List<long>> Nodes { get; set; } = nodes;
}