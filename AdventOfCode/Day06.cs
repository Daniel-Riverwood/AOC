using System.Collections.Concurrent;

namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly List<string> _input;
    public Day06()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
    }
    private Collection4 GenerateCollections(List<string> lines, bool step2 = false)
    {
        var timeLine = lines[0].Split(':')[1];
        var distLine = lines[1].Split(':')[1];
        var times = timeLine.Trim().Split(' ').Where(q => !string.IsNullOrWhiteSpace(q)).ToList();
        var distances = distLine.Trim().Split(' ').Where(q => !string.IsNullOrWhiteSpace(q)).ToList();

        var races = new List<Map2>();

        for (int i = 0; i < times.Count(); i++)
        {
            var map = new Map2(int.Parse(times[i]), int.Parse(distances[i]));

            races.Add(map);
        }


        return new Collection4(races);
    }

    private long ProcessInput1 ()
    {
        var collection = GenerateCollections(_input);
        var result = new ConcurrentBag<long>();

        Parallel.ForEach(collection.Races, (race, i, thread) =>
        {
            var wins = 0;
            for (int x = 0; x < race.Time; x++)
            {
                var dist = x * (race.Time - x);
                if (dist > race.Distance)
                {
                    wins++;
                }
            }

            result.Add(wins);
        });

        return result.Aggregate((x, y) => x * y);
    }

    private long ProcessInput2 ()
    {
        var collection = GenerateCollections(_input);
        var map = new Map2(Convert.ToInt64(string.Join("", collection.Races.Select(q => q.Time.ToString()))), Convert.ToInt64(string.Join("", collection.Races.Select(q => q.Distance.ToString()))));

        var wins = 0;
        for (int x = 0; x < map.Time; x++)
        {
            var dist = x * (map.Time - x);
            if (dist > map.Distance)
            {
                wins++;
            }
        }

        return wins;
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}, part 1");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}, part 2");
}
public class Collection4(List<Map2> races)
{
    public List<Map2> Races { get; set; } = races;
}

public class Map2(long time, long distance)
{
    public long Time { get; set; } = time;
    public long Distance { get; set; } = distance;
}