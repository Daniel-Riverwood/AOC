using Utilities;
using static Utilities.Utilities;

using Point = Utilities.Point2D<int>;

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var MoveRocks = (string platform, List<Point> directions, int cycles) =>
{
    var volCols = platform.AsMap();
    var rockPos = volCols.Where(q => q.Value == 'O').Select(s => s.Index).ToList();
    var hasSeen = new Dictionary<string, int>();
    
    for(var x = 0; x < cycles; x++)
    {
        foreach(var direction in directions)
        {
            var newPositions = new List<Point>(rockPos.Count);
            foreach(var rockold in rockPos.OrderBy(q => (q.X*-direction.X, q.Y*-direction.Y)))
            {
                var posNew = rockold;
                while (volCols.GetValueOrDefault(posNew + direction, '#') == '.') posNew += direction;
                volCols[rockold] = '.';
                volCols[posNew] = 'O';
                newPositions.Add(posNew);
            } 
            rockPos = newPositions;
        }

        var rockstring = string.Join(";", rockPos);
        if(hasSeen.TryGetValue(rockstring, out var index))
        {
            var rem = x - index;
            x += ((cycles - x) / rem) * rem;
        }
        hasSeen[rockstring] = x;
    }


    return rockPos.Sum(q => volCols.Height - q.Y);
    //for (var x = 0; x < volCols.Count(); x++)
    //{
    //    StringBuilder sb = new StringBuilder(volCols[x]);
    //    var squares = volCols[x].AllIndexesOf("#");
    //    for (var y = 0; y < sb.Length; y++)
    //    {
    //        if (sb[y] == 'O')
    //        {
    //            var inway = squares.Where(q => q < y).ToList();
    //            if (inway.Count() > 0)
    //            {
    //                var firstOpen = sb.ToString().Substring(inway.Last()).IndexOf('.');
    //                if (firstOpen < 0) continue;
    //                if (firstOpen + inway.Last() > y) continue;
    //                else
    //                {
    //                    sb[firstOpen + inway.Last()] = 'O';
    //                    sb[y] = '.';
    //                }
    //            }
    //            else
    //            {
    //                var firstOpen = sb.ToString().IndexOf('.');
    //                if (firstOpen < 0) continue;
    //                if (firstOpen > y) continue;
    //                else
    //                {
    //                    sb[firstOpen] = 'O';
    //                    sb[y] = '.';
    //                }
    //            }
    //        }
    //    }
    //    volCols[x] = sb.ToString();
    //}
    //foreach (var row in volCols)
    //{
    //    PrintOutput(row);
    //}
    //PrintOutput("");
    //return volCols;
};


var ProcessInputStep1 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    long res = 0;
    var platform = cols[0];
    var directions = new List<Point>() { Point.Up };

    res = MoveRocks(platform, directions, 1);

    return res;
};

var ProcessInputStep2 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    long res = 0;
    var platform = cols[0];
    int cycles = 1_000_000_000;
    var directions = new List<Point>() { Point.Up, Point.Left, Point.Down, Point.Right };

    res = MoveRocks(platform, directions, cycles);

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

