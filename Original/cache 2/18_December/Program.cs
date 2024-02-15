using Utilities;
using static Utilities.Utilities;

using Point = Utilities.Point2D<int>;

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var GetDir = (string val) =>
{
    if (val == "0") return Point.Right;
    else if (val == "1") return Point.Down;
    else if (val == "2") return Point.Left;
    else if (val == "3") return Point.Up;
    else if (val == "R") return Point.Right;
    else if (val == "L") return Point.Left;
    else if (val == "U") return Point.Up;
    else if (val == "D") return Point.Down;
    return Point.Origin;
};

var GetDeter = (long x1, long y1, long x2, long y2) => (x1 * y2) - (x2 * y1);

var Dig = (string Area, bool useCol = false) =>
{
    var seenPath = new List<Collection>();
    var Row = Area.SplitByNewline();
    var start = Point.Origin;
    foreach(var line in Row)
    {
        var dig = line.Split(' ');
        var dir = dig[0];
        var amount = Convert.ToInt32(dig[1]);
        var col = dig[2].Trim('(').Trim(')');
        var pathDir = Point.Origin;
        if (useCol)
        {
            pathDir = GetDir(col.Last().ToString());
            amount = Convert.ToInt32(string.Join("", col.Split("#")[1].Take(col.Length - 2)), 16);
        }
        else
        {
            pathDir = GetDir(dir);
        }


        for (var x = 0; x < amount; x++)
        {
            var cur = seenPath.LastOrDefault();
            if(cur == null) seenPath.Add(new Collection((start.X + pathDir.X, start.Y + pathDir.Y), col));
            else seenPath.Add(new Collection((cur.Pos.X + pathDir.X, cur.Pos.Y + pathDir.Y), col));
        }
    }
    long res = GetDeter(seenPath[seenPath.Count - 1].Pos.X, seenPath[seenPath.Count -1].Pos.Y, seenPath[0].Pos.X, seenPath[0].Pos.Y);
    for(var x = 1; x < seenPath.Count; x++)
    {
        res += GetDeter(seenPath[x - 1].Pos.X, seenPath[x - 1].Pos.Y, seenPath[x].Pos.X, seenPath[x].Pos.Y);
    }
    return ((res+seenPath.Count) / 2) + 1;
};

var ProcessInputStep1 = (string lines) => Dig(GenerateCollections(lines).First());

var ProcessInputStep2 = (string lines) => Dig(GenerateCollections(lines).First(), true);

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);

public class Collection(Point p, string color)
{
    public Point Pos { get; set; } = p;
    public string Color { get; set; } = color;
}