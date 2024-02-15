namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly List<string> _input;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
    }

    private Collection11 GenerateCollections(List<string> lines)
    {
        var lineArray = lines.ToArray();
        var numCollection = new List<NumberList11>();
        var specialCollection = new List<SpecialList11>();
        for (int x = 0; x < lineArray.Length; x++)
        {
            for (int y = 0; y < lineArray[x].Length; y++)
            {
                if (lineArray[x][y] == '#')
                {
                    numCollection.Add(new NumberList11(x, y));
                }
                else
                {
                    specialCollection.Add(new SpecialList11(x, y));
                }
            }
        }

        return new Collection11(numCollection, specialCollection);
    }

    private long Shortest (NumberList11 start, NumberList11 end) => Math.Abs(start.Index - end.Index) + Math.Abs(start.Line - end.Line);

    private long ProcessInput1()
    {
        var newList = new List<string>();
        var posList = new List<int>();
        var hasExpansion = false;
        for (var x = 0; x < _input.Count; x++)
        {
            if (!_input[x].Contains('#'))
            {
                hasExpansion = true;
            }

            if (hasExpansion)
            {
                newList.Add(_input[x]);
                hasExpansion = false;
            }
            newList.Add(_input[x]);

            for (var y = 0; y < _input[x].Length; y++)
            {
                if (_input[x][y] == '#')
                {
                    posList.Add(y);
                }
            }
        }

        var lastInsert = 0;
        foreach (var pos in Enumerable.Range(0, _input.First().Length).Where(q => !posList.Contains(q)))
        {
            for (var x = 0; x < newList.Count(); x++)
            {
                newList[x] = newList[x].Insert(pos + lastInsert, ".");
            }
            lastInsert++;
        }

        var collection = GenerateCollections(newList);

        var path = new List<long>();
        for (var x = 0; x < collection.NumberCollection.Count(); x++)
        {
            for (var y = x + 1; y < collection.NumberCollection.Count(); y++)
            {
                path.Add(Shortest(collection.NumberCollection[x], collection.NumberCollection[y]));
            }
        }

        return path.Sum();
    }

    private long ProcessInput2(int iterations)
    {
        HashSet<NumberList11> res = new HashSet<NumberList11>();

        var newList = new List<string>();
        var posList = new List<int>();
        var emptyrows = new List<int>();
        var emptycols = new List<int>();
        var hasExpansion = false;
        for (var x = 0; x < _input.Count(); x++)
        {
            if (!_input[x].Contains('#'))
            {
                hasExpansion = true;
            }

            if (hasExpansion)
            {
                emptyrows.Add(x);
                hasExpansion = false;
            }
            newList.Add(_input[x]);

            for (var y = 0; y < _input[x].Length; y++)
            {
                if (_input[x][y] == '#')
                {
                    posList.Add(y);
                }
            }
        }

        emptycols.AddRange(Enumerable.Range(0, _input.First().Length).Where(q => !posList.Contains(q)));

        var collection = GenerateCollections(newList);

        var path = new List<long>();
        foreach (var galaxy in collection.NumberCollection)
        {
            galaxy.Line = galaxy.Line + (iterations * emptyrows.Count(q => q < galaxy.Line));
            galaxy.Index = galaxy.Index + (iterations * emptycols.Count(x => x < galaxy.Index));
        }

        for (var x = 0; x < collection.NumberCollection.Count(); x++)
        {
            for (var y = x + 1; y < collection.NumberCollection.Count(); y++)
            {
                path.Add(Shortest(collection.NumberCollection[x], collection.NumberCollection[y]));
            }
        }

        return path.Sum();
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2(999999)}");
}

public class Collection11(List<NumberList11> numberList, List<SpecialList11> specialList)
{
    public List<NumberList11> NumberCollection { get; set; } = numberList;
    public List<SpecialList11> SpecialCollection { get; set; } = specialList;
}

public class NumberList11(long Line, long Index)
{
    public long Line { get; set; } = Line;
    public long Index { get; set; } = Index;
}

public class SpecialList11(long Line, long Index)
{
    public long Line { get; set; } = Line;
    public long Index { get; set; } = Index;
}
