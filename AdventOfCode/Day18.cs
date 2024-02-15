namespace AdventOfCode;

using System.Diagnostics.CodeAnalysis;
using Point = Point2D<int>;

public class Day18 : BaseDay
{
    private readonly List<string> _input;
    
    public Day18()
    {
        _input = File.ReadAllText(InputFilePath).SplitByDoubleNewline();
    }

    private Point GetDir(string val) => val switch
    {
        "0" => Point.Right,
        "1" => Point.Down,
        "2" => Point.Left,
        "3" => Point.Up,
        "R" => Point.Right,
        "L" => Point.Left,
        "U" => Point.Up,
        "D" => Point.Down,
        _ => Point.Origin
    };

    private long GetDeter(long x1, long y1, long x2, long y2) => (x1 * y2) - (x2 * y1);

    private long Dig(string Area, bool useCol = false)
    {
        var seenPath = new List<Collection18>();
        var Row = Area.SplitByNewline();
        var start = Point.Origin;
        foreach (var line in Row)
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
                if (cur == null) seenPath.Add(new Collection18((start.X + pathDir.X, start.Y + pathDir.Y), col));
                else seenPath.Add(new Collection18((cur.Pos.X + pathDir.X, cur.Pos.Y + pathDir.Y), col));
            }
        }
        long res = GetDeter(seenPath[seenPath.Count - 1].Pos.X, seenPath[seenPath.Count - 1].Pos.Y, seenPath[0].Pos.X, seenPath[0].Pos.Y);
        for (var x = 1; x < seenPath.Count; x++)
        {
            res += GetDeter(seenPath[x - 1].Pos.X, seenPath[x - 1].Pos.Y, seenPath[x].Pos.X, seenPath[x].Pos.Y);
        }
        return ((res + seenPath.Count) / 2) + 1;
    }

    private long ProcessInput1() => Dig(_input.First());

    private long ProcessInput2() => Dig(_input.First(), true);

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}
public class Collection18(Point p, string color)
{
    public Point Pos { get; set; } = p;
    public string Color { get; set; } = color;
}
