using System.Text;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    private readonly string _input;
    private Dictionary<int, (int pos, bool isHoriz)> Mirrors = new();

    public Day13()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    private List<string> GenerateCollections(string input) => input.SplitByDoubleNewline();

    private long CalcPos(string volcano, int index)
    {
        var volRows = volcano.SplitByNewline();
        var volCols = string.Join("\n", volcano).SplitIntoColumns();

        for (var x = 1; x < volRows.Count(); x++)
        {
            if (volRows.Take(x).Reverse().Zip(volRows.Skip(x)).All(q => q.First == q.Second))
            {
                if (Mirrors.TryGetValue(index, out (int pos, bool isHoriz) vol))
                {
                    if (vol.isHoriz && vol.pos == x) continue;
                }
                Mirrors[index] = (x, true);
                return x * 100;
            }
        }

        for (var x = 1; x < volCols.Count(); x++)
        {
            if (volCols.Take(x).Reverse().Zip(volCols.Skip(x)).All(x => x.First == x.Second))
            {
                if (Mirrors.TryGetValue(index, out (int pos, bool isHoriz) vol))
                {
                    if (!vol.isHoriz && vol.pos == x) continue;
                }
                Mirrors[index] = (x, false);
                return x;
            }
        }

        return -1;
    }
    private long ProcessInput1()
    {
        var cols = GenerateCollections(_input);
        long res = 0;
        for (var x = 0; x < cols.Count(); x++)
        {
            var pos = CalcPos(cols[x], x);
            if (pos > -1)
            {
                res += pos;
            }
        }

        return res;
    }

    private long ProcessInput2 ()
    {
        var cols = GenerateCollections(_input);
        long res = 0;
        for (var x = 0; x < cols.Count(); x++)
        {
            var area = cols[x];
            for (var y = 0; y < area.Length; y++)
            {
                if (!".#".Contains(area[y])) continue;
                StringBuilder newarea = new StringBuilder(area);

                newarea[y] = newarea[y] == '.' ? '#' : '.';

                var pos = CalcPos(newarea.ToString(), x);
                if (pos > -1)
                {
                    res += pos;
                    break;
                }
            }
        }

        return res;
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}
