using Utilities;
using static Utilities.Utilities;

var PathList = new List<List<Path>>();
var Start = new Coordinate2D(0, 0);
var End = new Coordinate2D(0, 0);
int Longest = 0;
var P2 = false;

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var isValid = (int x, int y, CompassDirection dir) =>
{
    if (PathList[x][y].visited == 1) return 0;
    
    if(P2)
    {
        if (PathList[x][y].path == '#') return 0;
        return 1;
    }
    else
    {
        switch (PathList[x][y].path)
        {
            case '>':
                if (dir == CompassDirection.E) return 1;
                return 0;
            case '<':
                if (dir == CompassDirection.W) return 1;
                return 0;
            case '^':
                if (dir == CompassDirection.N) return 1;
                return 0;
            case 'v':
                if (dir == CompassDirection.S) return 1;
                return 0;
            case '.':
                return 1;
        }
    }
    return -1;
};

void LongestPath (int x, int y, CompassDirection dir, int visited)
{
    PathList[x][y].visited = 1;

    if (x == End.x && y == End.y)
    {
        if(visited > Longest)
        {
            Longest = visited;
            PrintOutput(Longest.ToString());
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

var Actions = (string area) =>
{
    var map = area.SplitByNewline().ToList();
    var gridSize = map.Count;

    Start = Enumerable.Range(0, map[0].Count()).Where(j => map[0][j] == '.')
            .Select(j => new Coordinate2D(0, j)).Single();

    End = Enumerable.Range(0, gridSize).Where(j => map[gridSize-1][j] == '.')
            .Select(j => new Coordinate2D(gridSize-1, j)).Single();

    PathList = map.Select(q => q.ToArray()).ToArray().Select(q => q.Select(e => new Path() { path = e, visited = e == '#' ? 1 : 0 }).ToList()).ToList();

    PathList[Start.x][Start.y].visited = 1;

    LongestPath(Start.x + 1, Start.y, CompassDirection.S, 1);

    return Longest;
};


var ProcessInputStep1 = (string lines) => Actions(GenerateCollections(lines).First());

var ProcessInputStep2 = (string lines) => Actions(GenerateCollections(lines).First());

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
P2 = true;
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);

public class Path
{
    public char path { get; set; }
    public int visited { get; set; }
}

public class Nodes(string nodeName, List<string> connectedNodes, bool isFlipFlop, bool isConjunction)
{
    public string NodeName { get; set; } = nodeName;
    public List<string> connectedNodes { get; set; } = connectedNodes;
    public List<Nodes> InputNodes { get; set; } = new List<Nodes>();
    public bool isFlipFlop { get; set; } = isFlipFlop;
    public bool isConjunction { get; set; } = isConjunction;
    public bool OnOff { get; set; }
    public bool HighLow { get; set; } = false;
}
