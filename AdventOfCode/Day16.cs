using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;

namespace AdventOfCode;

using Point = AdventOfCode.Point2D<int>;

public class Day16 : BaseDay
{
    private readonly List<string> _input;
    
    public Day16()
    {
        _input = File.ReadAllText(InputFilePath).SplitByDoubleNewline();
    }

    private long Energize(StringMap<char> path, Point start, Point direction)
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
    }

    private long ProcessInput1() => Energize(_input.First().AsMap(), (0, 0), Point.Right);

    private long ProcessInput2 ()
    {
        var cols = _input.First().AsMap();
        var reslist = new List<long>();

        for (var x = 0; x < cols.Width; x++)
        {
            reslist.Add(Energize(cols, (x, 0), Point.Down));
            reslist.Add(Energize(cols, (x, cols.Height - 1), Point.Up));
        }
        for (var y = 0; y < cols.Height; y++)
        {
            reslist.Add(Energize(cols, (0, y), Point.Right));
            reslist.Add(Energize(cols, (cols.Width - 1, y), Point.Left));
        }

        return reslist.Max();
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}
