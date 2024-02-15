namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly List<string> _input;
    private readonly Collection collection;
    
    public Day03()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
        collection = GenerateCollections(_input);
    }

    private Collection GenerateCollections (List<string> lines)
    {
        var lineArray = lines.ToArray();
        var numCollection = new List<NumberList>();
        var specialCollection = new List<SpecialList>();
        for (int x = 0; x < lineArray.Length; x++)
        {
            var numList = "";
            var indexes = new List<int>();
            for (int y = 0; y < lineArray[x].Length; y++)
            {
                if (char.IsDigit(lineArray[x][y]))
                {
                    indexes.Add(y);
                    numList += lineArray[x][y];
                }
                else if (indexes.Count > 0)
                {
                    numCollection.Add(new NumberList(x, indexes, int.Parse(numList)));
                    indexes = new List<int>();
                    numList = "";
                }

                if (lineArray[x][y] != '.' && !char.IsLetterOrDigit(lineArray[x][y]))
                {
                    specialCollection.Add(new SpecialList(x, y, lineArray[x][y]));
                }
            }

            if (indexes.Count > 0)
            {
                numCollection.Add(new NumberList(x, indexes, int.Parse(numList)));
            }
        }

        return new Collection(numCollection, specialCollection);
    }

    private int ProcessInput1 ()
    {
        var partnum = new List<int>();
        var col = new Collection(collection.NumberCollection, collection.SpecialCollection);

        foreach (var special in col.SpecialCollection)
        {
            var numberList = col.NumberCollection.Where(q =>
                    (q.Line == (special.Line - 1) ||
                     q.Line == (special.Line + 1) ||
                     q.Line == special.Line) &&
                    (q.Indexes.Contains(special.Index) ||
                     q.Indexes.Contains(special.Index - 1) ||
                     q.Indexes.Contains(special.Index + 1))).ToList();

            partnum.AddRange(numberList.Select(q => q.Numbers).ToList());

            col.NumberCollection = col.NumberCollection.Where(q => !numberList.Contains(q)).ToList();
        }

        return partnum.Sum();
    }

    private int ProcessInput2 ()
    {
        var partnum = new List<int>();

        foreach (var special in collection.SpecialCollection)
        {
            var numberList = collection.NumberCollection.Where(q =>
                    (q.Line == (special.Line - 1) ||
                     q.Line == (special.Line + 1) ||
                     q.Line == special.Line) &&
                    (q.Indexes.Contains(special.Index) ||
                     q.Indexes.Contains(special.Index - 1) ||
                     q.Indexes.Contains(special.Index + 1))).ToList();

            var calc = 0;

            if (numberList.Count() == 2)
            {
                calc = numberList[0].Numbers * numberList[1].Numbers;
            }

            partnum.Add(calc);
        }

        return partnum.Sum();
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}

public class Collection(List<NumberList> numberList, List<SpecialList> specialList)
{
    public List<NumberList> NumberCollection { get; set; } = numberList;
    public List<SpecialList> SpecialCollection { get; set; } = specialList;
}

public class NumberList(int Line, List<int> Indexes, int Numbers)
{
    public int Line { get; set; } = Line;
    public List<int> Indexes { get; set; } = Indexes;
    public int Numbers { get; set; } = Numbers;
}

public class SpecialList(int Line, int Index, char Special)
{
    public int Line { get; set; } = Line;
    public int Index { get; set; } = Index;
    public char Special { get; set; } = Special;
}
