using System.Text;
using Utilities;

Dictionary<int, (int pos, bool isHoriz)> Mirrors = new();

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var CalcPos = (string volcano, int index) =>
{
    var volRows = volcano.SplitByNewline();
    var volCols = string.Join("\n", volcano).SplitIntoColumns();

    for (var x = 1; x < volRows.Count(); x++)
    {
        if (volRows.Take(x).Reverse().Zip(volRows.Skip(x)).All(q => q.First == q.Second))
        {
            if(Mirrors.TryGetValue(index, out (int pos, bool isHoriz) vol))
            {
                if (vol.isHoriz && vol.pos == x) continue;
            }
            Mirrors[index] = (x, true);
            return x * 100;
        }
    }

    for(var x = 1; x < volCols.Count(); x++)
    {
        if(volCols.Take(x).Reverse().Zip(volCols.Skip(x)).All(x => x.First == x.Second))
        {
            if(Mirrors.TryGetValue(index, out (int pos, bool isHoriz) vol))
            {
                if (!vol.isHoriz && vol.pos == x) continue;
            }
            Mirrors[index] = (x, false);
            return x;
        }
    }

    return -1;
};

var ProcessInputStep1 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    int res = 0;
    for(var x = 0; x < cols.Count(); x++)
    {
        var pos = CalcPos(cols[x], x);
        if(pos > -1)
        {
            res += pos;
        }
    }

    return res;
};

var ProcessInputStep2 = (string lines) =>
{
    var cols = GenerateCollections(lines);
    int res = 0;
    for (var x = 0; x < cols.Count(); x++)
    {
        var area = cols[x];
        for (var y = 0; y < area.Length; y++)
        {
            if (!".#".Contains(area[y])) continue;
            StringBuilder newarea = new StringBuilder(area);
            
            newarea[y] = newarea[y] == '.' ? '#' : '.';

            var pos = CalcPos(newarea.ToString(), x);
            if(pos > -1)
            {
                res += pos;
                break;
            }
        }
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

