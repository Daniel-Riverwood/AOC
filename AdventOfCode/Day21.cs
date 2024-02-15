using static AdventOfCode.Utilities;

namespace AdventOfCode;

public class Day21 : BaseDay
{
    private readonly List<string> _input;

    public Day21()
    {
        _input = File.ReadAllText(InputFilePath).SplitByDoubleNewline();
    }

    private long MassSteps(string area, long steps)
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
    }

    public int Actions(string area, long actionCount)
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
    }

    private long ProcessInput1() => Actions(_input.First(), 64);

    private long ProcessInput2() => MassSteps(_input.First(), 26501365);

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}
