using Utilities;
using static Utilities.Utilities;

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var Actions = (string area, long actionCount) =>
{
    var map = area.SplitByNewline().ToList();
    var gridSize = map.Count;

    var firstPlot = Enumerable.Range(0, gridSize)
        .SelectMany(i => Enumerable.Range(0, gridSize)
            .Where(j => map[i][j] == 'S')
            .Select(j => new Coordinate2D(i, j)))
        .Single();

    var seq = new List<int>();
    var step = 0;
    var plots = new HashSet<Coordinate2D> { firstPlot };
    for (var x = 0; x < actionCount; x++)
    {
        plots = new HashSet<Coordinate2D>(plots.SelectMany(q => new[] { CompassDirection.N, CompassDirection.S, CompassDirection.E, CompassDirection.W }
                    .Select(dir => q.MoveDirection(dir)))
                    .Where(q => map[((q.x % 131) + 131) % 131][((q.y % 131) + 131) % 131] != '#'));
        seq.Add(plots.Count);
    }

    return plots.Count;
};

var MassSteps = (string area, long steps) =>
{
    var map = area.SplitByNewline().ToList();
    var gridSize = map.Count;

    var firstPlot = Enumerable.Range(0, gridSize)
        .SelectMany(i => Enumerable.Range(0, gridSize)
            .Where(j => map[i][j] == 'S')
            .Select(j => new Coordinate2D(i, j)))
        .Single();

    var grids = steps / gridSize;
    var rem = steps % gridSize;
    var seq = new List<int>();
    var step = 0;
    var plots = new HashSet<Coordinate2D> { firstPlot };
    var dirs = new[] { CompassDirection.N, CompassDirection.S, CompassDirection.E, CompassDirection.W };
    for (var x = 0; x < 3; x++)
    {
        var target = x * gridSize + rem;
        while (step < target)
        {
            step++;
            plots = new HashSet<Coordinate2D>(plots
                .SelectMany(it => dirs
                    .Select(dir => it.MoveDirection(dir)))
                        .Where(dest => map[((dest.x % 131) + 131) % 131][((dest.y % 131) + 131) % 131] != '#'));
        }
        seq.Add(plots.Count);
    }

    var c = seq[0];
    var apb = seq[1] - c;
    var faptb = seq[2] - c;
    var ta = faptb - (2 * apb);
    var a = ta / 2;
    var b = apb - a;

    return a * (grids * grids) + b * (grids) + c;
};

var ProcessInputStep1 = (string lines) => Actions(GenerateCollections(lines).First(), 64);

var ProcessInputStep2 = (string lines) => MassSteps(GenerateCollections(lines).First(), 26501365);

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);

public class Broadcaster(List<string> connectedNodes)
{
    public List<string> ConnectedNodes { get; set; } = connectedNodes;
    public long LowPulses { get; set; } = 0;
    public long HighPulses { get; set; } = 0;
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
