using System.Text;
using Utilities;

Dictionary<int, (int pos, bool isHoriz)> Mirrors = new();
var Up = (0, -1);
var Down = (0, 1);
var Left = (-1, 0);
var Right = (1, 0);

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var MoveRocks = (string platform) =>
{
    var volCols = string.Join("\n", platform).SplitIntoColumns();

    volCols.Select(q =>
    {
        while(q.Contains(".O"))
        {
            q = q.Replace(".O", "O.");
        }
        return q;
    });

    return volCols;
};

var ProcessInputStep1 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    long res = 0;
    var platform = MoveRocks(cols[0]);

    foreach (var row in platform)
    {
        var indexes = row.AllIndexesOf("O");
        res += indexes.Sum(q => row.Length - q);
    }

    return res;
};

var ProcessInputStep2 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    long res = 0;
    var platform = MoveRocks(cols[0]);

    for (var x = 0; x < 1000000000 - 1; x++)
    {
        var volCols = string.Join("\n", platform).SplitIntoColumns();
        platform = MoveRocks(string.Join("\n", volCols));


    }

    foreach (var row in platform)
    {
        var indexes = row.AllIndexesOf("O");
        res += indexes.Sum(q => row.Length - q);
    }

    return res;
};

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);


public class Collection(List<string> volcano)
{
    public List<string> Volcano { get; set; } = volcano;
    public int MirrorPos { get; set; }
    public bool isVert { get; set; }
    public bool isHoriz { get; set; }
}
public class Map(int index, int line)
{
    public int Index { get; set; } = index;
    public int Line { get; set; } = line;
}

