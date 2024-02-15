using Utilities;
using static Utilities.Utilities;

using Point = Utilities.Point2D<int>;

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var Energize = (StringMap<char> path, Point start, Point direction) =>
{
    var seenPath = new HashSet<(Point pos, Point)>();
    var rem = new Stack<(Point pos, Point dir)>([(start - direction, direction)]);

    while (rem.TryPop(out (Point pos, Point dir) cur))
    {
        if (!seenPath.Add(cur)) continue;

        var newPos = cur.pos + cur.dir;
        if(!path.Contains(newPos)) continue;

        switch(path[newPos])
        {
            case '\\':
                rem.Push((newPos, (cur.dir.Y, cur.dir.X)));
                break;
            case '/':
                rem.Push((newPos, (-cur.dir.Y, -cur.dir.X)));
                break;
            case '-':
                if (cur.dir == Point.Up || cur.dir == Point.Down)
                {
                    rem.Push((newPos, Point.Left));
                    rem.Push((newPos, Point.Right));
                }
                else
                {
                    rem.Push((newPos, cur.dir));
                }
                break;
            case '|':
                if (cur.dir == Point.Left || cur.dir == Point.Right)
                {
                    rem.Push((newPos, Point.Up));
                    rem.Push((newPos, Point.Down));
                }
                else
                {
                    rem.Push((newPos, cur.dir));
                }
                break;
            default:
                rem.Push((newPos, cur.dir));
                break;
        }
    }
    return seenPath.Select(q => q.pos).Distinct().Count() - 1;
};

var ProcessInputStep1 = (string lines) => Energize(GenerateCollections(lines).First().AsMap(), (0,0), Point.Right);

var ProcessInputStep2 = (string lines) =>
{
    var cols = GenerateCollections(lines).First().AsMap();
    var reslist = new List<long>();

    for(var x = 0; x < cols.Width; x++)
    {
        reslist.Add(Energize(cols, (x, 0), Point.Down));
        reslist.Add(Energize(cols, (x, cols.Height - 1), Point.Up));
    }
    for (var y = 0; y < cols.Height; y++)
    {
        reslist.Add(Energize(cols, (0, y), Point.Right));
        reslist.Add(Energize(cols, (cols.Width-1, y), Point.Left));
    }

    return reslist.Max();
};

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);


public class Collection(char cur, Point2D<int> direction)
{
    public char Current { get; set; } = cur;
    public Point2D<int> Direction { get; set; } = direction;
}

