

namespace AdventOfCode;
using Point = AdventOfCode.Point2D<int>;

public class Day14 : BaseDay
{
    private readonly List<string> _input;
    
    public Day14()
    {
        _input = File.ReadAllText(InputFilePath).SplitByDoubleNewline();
    }

    private long MoveRocks(string platform, List<Point> directions, int cycles)
    {
        var volCols = platform.AsMap();
        var rockPos = volCols.Where(q => q.Value == 'O').Select(s => s.Index).ToList();
        var hasSeen = new Dictionary<string, int>();

        for (var x = 0; x < cycles; x++)
        {
            foreach (var direction in directions)
            {
                var newPositions = new List<Point>(rockPos.Count);
                foreach (var rockold in rockPos.OrderBy(q => (q.X * -direction.X, q.Y * -direction.Y)))
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
            if (hasSeen.TryGetValue(rockstring, out var index))
            {
                var rem = x - index;
                x += ((cycles - x) / rem) * rem;
            }
            hasSeen[rockstring] = x;
        }


        return rockPos.Sum(q => volCols.Height - q.Y);
    }

    private long ProcessInput1 ()
    {
        var platform = _input[0];
        var directions = new List<Point>() { Point.Up };
        return MoveRocks(platform, directions, 1);
    }

    private long ProcessInput2 ()
    {
        var platform = _input[0];
        int cycles = 1_000_000_000;
        var directions = new List<Point>() { Point.Up, Point.Left, Point.Down, Point.Right };
        return MoveRocks(platform, directions, cycles);
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}
