using static AdventOfCode.Utilities;

namespace AdventOfCode;

public class Day23 : BaseDay
{
    private readonly List<string> _input;
    private List<List<Path>> PathList = new List<List<Path>>();
    private Coordinate2D Start = new Coordinate2D(0, 0);
    private Coordinate2D End = new Coordinate2D(0, 0);
    private int Longest = 0;
    private bool P2 = false;
    public Day23()
    {
        _input = File.ReadAllText(InputFilePath).SplitByDoubleNewline();
    }

    private long isValid(int x, int y, CompassDirection dir)
    {
        if (PathList[x][y].visited == 1) return 0;

        if (P2)
        {
            if (PathList[x][y].path == '#') return 0;
            return 1;
        }
        else
        {
            return PathList[x][y].path switch
            {
                '>' => dir == CompassDirection.E ? 1 : 0,
                '<' => dir == CompassDirection.W ? 1 : 0,
                '^' => dir == CompassDirection.N ? 1 : 0,
                'v' => dir == CompassDirection.S ? 1 : 0,
                '.' => 1,
                _ => -1
            };
        }
    }

    private void LongestPath(int x, int y, CompassDirection dir, int visited)
    {
        PathList[x][y].visited = 1;

        if (x == End.x && y == End.y)
        {
            if (visited > Longest)
            {
                Longest = visited;
            }
            PathList[x][y].visited = 0;
            return;
        }

        if (isValid(x - 1, y, CompassDirection.N) == 1) LongestPath(x - 1, y, CompassDirection.N, visited + 1);

        if (isValid(x + 1, y, CompassDirection.S) == 1) LongestPath(x + 1, y, CompassDirection.S, visited + 1);

        if (isValid(x, y + 1, CompassDirection.E) == 1) LongestPath(x, y + 1, CompassDirection.E, visited + 1);

        if (isValid(x, y - 1, CompassDirection.W) == 1) LongestPath(x, y - 1, CompassDirection.W, visited + 1);

        PathList[x][y].visited = 0;
    }

    private long Actions(string area)
    {
        var map = area.SplitByNewline().ToList();
        var gridSize = map.Count;

        Start = Enumerable.Range(0, map[0].Count()).Where(j => map[0][j] == '.')
                .Select(j => new Coordinate2D(0, j)).Single();

        End = Enumerable.Range(0, gridSize).Where(j => map[gridSize - 1][j] == '.')
                .Select(j => new Coordinate2D(gridSize - 1, j)).Single();

        PathList = map.Select(q => q.ToArray()).ToArray().Select(q => q.Select(e => new Path() { path = e, visited = e == '#' ? 1 : 0 }).ToList()).ToList();

        PathList[Start.x][Start.y].visited = 1;

        LongestPath(Start.x + 1, Start.y, CompassDirection.S, 1);

        return Longest;
    }

    private long ProcessInput1() => Actions(_input.First());

    private long ProcessInput2()
    {
        P2 = true;
        return Actions(_input.First());
    }
    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}
public class Path
{
    public char path { get; set; }
    public int visited { get; set; }
}

public class Nodes23(string nodeName, List<string> connectedNodes, bool isFlipFlop, bool isConjunction)
{
    public string NodeName { get; set; } = nodeName;
    public List<string> connectedNodes { get; set; } = connectedNodes;
    public List<Nodes23> InputNodes { get; set; } = new List<Nodes23>();
    public bool isFlipFlop { get; set; } = isFlipFlop;
    public bool isConjunction { get; set; } = isConjunction;
    public bool OnOff { get; set; }
    public bool HighLow { get; set; } = false;
}