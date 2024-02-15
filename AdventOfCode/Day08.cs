using System.Xml.Linq;

namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly List<string> _input;
    
    public Day08()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
    }
    private Collection8 GenerateCollections(List<string> lines)
    {
        var res = new List<NodeMap>();
        var steps = lines[0].Select(ch => ch).ToList();

        foreach (var line in lines.Skip(2))
        {
            var temp = line.Split('=');
            var node = temp[0].Trim();
            var nodelist = temp[1].Trim().Split(',');
            var leftnode = nodelist[0].Trim('(').Trim();
            var rightnode = nodelist[1].Trim(')').Trim();
            res.Add(new NodeMap(node, leftnode, rightnode));
        }

        return new Collection8(res, steps);
    }

    private long GCD(long x, long y)
    {
        if (y == 0) return x;
        else return GCD(y, x % y);
    }

    private long LCM(List<long> steps) => steps.Aggregate((x, y) => x * y / GCD(x, y));

    private long ProcessInput1 ()
    {
        var collection = GenerateCollections(_input);
        var found = false;
        var current = "AAA";
        var stepNo = 0;

        long res = 0;
        while (!found)
        {
            res++;

            if (collection.Steps.Count() == stepNo) stepNo = 0;

            var step = collection.Steps[stepNo];
            stepNo++;

            if (step == 'L')
            {
                current = collection.Nodes.FirstOrDefault(q => string.Equals(q.Node, current, StringComparison.InvariantCultureIgnoreCase))?.LeftNode;
            }

            if (step == 'R')
            {
                current = collection.Nodes.FirstOrDefault(q => string.Equals(q.Node, current, StringComparison.InvariantCultureIgnoreCase))?.RightNode;
            }

            if (current == "ZZZ") found = true;
        }

        return res;
    }

    private long ProcessInput2 ()
    {
        var collection = GenerateCollections(_input);
        var found = false;
        var current = new List<string>();
        var stepNo = 0;

        long res = 0;
        var steps = new List<long>();

        collection.Nodes.Where(q => q.Node[2] == 'A').ToList().ForEach(q => current.Add(q.Node));

        for (var i = 0; i < current.Count; i++)
        {
            res = 0;
            found = false;
            var curr = current[i];
            while (!found)
            {
                res++;

                if (collection.Steps.Count == stepNo) stepNo = 0;

                var step = collection.Steps[stepNo];
                stepNo++;

                if (step == 'L')
                {
                    curr = collection.Nodes.Find(q => string.Equals(q.Node, curr, StringComparison.InvariantCultureIgnoreCase))?.LeftNode;
                }

                if (step == 'R')
                {
                    curr = collection.Nodes.Find(q => string.Equals(q.Node, curr, StringComparison.InvariantCultureIgnoreCase))?.RightNode;
                }

                if (curr[2] == 'Z') found = true;
            }
            steps.Add(res);
        }
        var least = LCM(steps);

        return least;
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}, part 1");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}, part 2");
}
public class Collection8(List<NodeMap> nodes, List<char> steps)
{
    public List<NodeMap> Nodes { get; set; } = nodes;
    public List<char> Steps { get; set; } = steps;
}

public class NodeMap(string node, string left, string right)
{
    public string Node { get; set; } = node;
    public string LeftNode { get; set; } = left;
    public string RightNode { get; set; } = right;
}

