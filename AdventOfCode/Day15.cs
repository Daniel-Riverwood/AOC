using System.Linq;

namespace AdventOfCode;

public class Day15 : BaseDay
{
    private readonly List<string> _input;
    public Day15()
    {
        _input = File.ReadAllText(InputFilePath).SplitByDoubleNewline();
    }

    private List<int> HASHConvert(List<string> cols)
    {
        var reslist = new List<int>();
        foreach (var col in cols)
        {
            var charlist = col.Split(',');
            foreach (var convert in charlist)
            {
                var shortres = 0;
                var s = convert.ToCharArray();
                for (var x = 0; x < s.Length; x++)
                {
                    shortres += s[x];
                    shortres = shortres * 17;
                    shortres = shortres % 256;
                }
                reslist.Add(shortres);
            }
        }
        return reslist;
    }

    private long ProcessInput1 () => HASHConvert(_input).Sum();

    private long ProcessInput2 ()
    {
        var boxList = new Dictionary<int, List<Collection15>>();
        for (var x = 0; x < 256; x++)
        {
            boxList[x] = new List<Collection15>();
        }
        foreach (var col in _input)
        {
            var charList = col.Split(",");
            foreach (var convert in charList)
            {
                var shortres = 0;
                var s = new char[convert.Length];
                if (convert.Contains('-')) s = convert.Split('-')[0].ToCharArray();
                if (convert.Contains('=')) s = convert.Split('=')[0].ToCharArray();
                for (var x = 0; x < s.Length; x++)
                {
                    shortres += s[x];
                    shortres = shortres * 17;
                    shortres = shortres % 256;
                }
                if (convert.Contains('-'))
                {
                    var rem = boxList[shortres].LastOrDefault(q => q.Pre == convert.Split('-')[0]);
                    if (rem == null) continue;
                    boxList[shortres].Remove(rem);
                }
                else if (convert.Contains("="))
                {
                    var split = convert.Split('=');
                    var newLens = new Collection15(split[0], Convert.ToInt32(split[1]));
                    var existing = boxList[shortres].Find(q => q.Pre == newLens.Pre);
                    if (existing == null)
                    {
                        boxList[shortres].Add(newLens);
                    }
                    else
                    {
                        var index = boxList[shortres].IndexOf(existing);
                        boxList[shortres][index] = newLens;
                    }
                }
            }
        }


        return boxList.Sum(q => q.Value.Sum(e => (q.Key + 1) * (q.Value.IndexOf(e) + 1) * (e.Post)));
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}
public class Collection15(string pre, int post)
{
    public string Pre { get; set; } = pre;
    public int Post { get; set; } = post;
}
