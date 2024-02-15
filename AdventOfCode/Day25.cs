namespace AdventOfCode;

public class Day25 : BaseDay
{
    private readonly List<string> _input;
    private List<(string, string)> edges = new List<(string, string)>();
    private List<string> vertices = new List<string>();
    public Day25()
    {
        _input = File.ReadAllText(InputFilePath).SplitByDoubleNewline();
    }

    private void CreateTools(List<string> tools)
    {
        vertices = new List<string>();
        edges = new List<(string, string)>();
        //var BaseToolList = new List<Tool>();
        foreach (var tool in tools)
        {
            var main = tool.Split(':');
            var pos = main[0].Trim();
            var nodes = main[1].Trim().Split(' ').ToList();

            HashSet<string> con = new HashSet<string>(nodes);

            if (!vertices.Contains(pos)) vertices.Add(pos);

            foreach (var connected in con)
            {
                if (!vertices.Contains(connected)) vertices.Add(connected);
                if (!edges.Contains((pos, connected)) && !edges.Contains((connected, pos))) edges.Add((pos, connected));
            }
        }
    }

    private int Count(List<List<string>> sets)
    {
        int res = 0;
        for (var x = 0; x < edges.Count; x++)
        {
            var s1 = sets.Where(q => q.Contains(edges[x].Item1)).First();
            var s2 = sets.Where(q => q.Contains(edges[x].Item2)).First();
            if (s1 != s2) res++;
        }
        return res;
    }

    private long Actions(string area)
    {
        CreateTools(area.SplitByNewline().ToList());
        List<List<string>> subsets = new List<List<string>>();

        do
        {
            subsets = new List<List<string>>();

            foreach (var vertex in vertices)
            {
                subsets.Add(new List<string>() { vertex });
            }

            int i = 0;
            List<string> s1, s2;

            while (subsets.Count > 2)
            {
                i = new Random().Next() % edges.Count;

                s1 = subsets.Where(s => s.Contains(edges[i].Item1)).First();
                s2 = subsets.Where(s => s.Contains(edges[i].Item2)).First();

                if (s1 == s2) continue;

                subsets.Remove(s2);
                s1.AddRange(s2);
            }

        } while (Count(subsets) != 3);

        return subsets.Aggregate(1, (p, s) => p * s.Count);
    }

    private long ProcessInput1() => Actions(_input.First());

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"N/A");
}
