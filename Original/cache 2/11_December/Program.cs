var GetInput = (string inputFileName) =>
{
    List<string> result = new List<string>();
    StreamReader sr = new StreamReader(inputFileName);
    var line = sr.ReadLine();
    while (line != null)
    {
        result.Add(line);
        line = sr.ReadLine();
    }
    sr.Close();
    return result;
};

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (List<string> lines) =>
{
    var lineArray = lines.ToArray();
    var numCollection = new List<NumberList>();
    var specialCollection = new List<SpecialList>();
    for (int x = 0; x < lineArray.Length; x++)
    {
        for (int y = 0; y < lineArray[x].Length; y++)
        {
            if (lineArray[x][y] == '#')
            {
                numCollection.Add(new NumberList(x, y));
            }
            else
            {
                specialCollection.Add(new SpecialList(x, y));
            }
        }
    }

    return new Collection(numCollection, specialCollection);
};

var Shortest = (NumberList start, NumberList end) => Math.Abs(start.Index - end.Index) + Math.Abs(start.Line - end.Line);

var ProcessInputStep1 = (List<string> lines) =>
{
    var newList = new List<string>();
    var posList = new List<int>();
    var hasExpansion = false;
    for(var x = 0; x < lines.Count(); x++)
    {
        if (!lines[x].Contains('#'))
        {
            hasExpansion = true;
        }

        if (hasExpansion)
        {
            newList.Add(lines[x]);
            hasExpansion = false;
        }
        newList.Add(lines[x]);

        for(var y = 0; y < lines[x].Length; y++)
        {
            if (lines[x][y] == '#')
            {
                posList.Add(y);
            }
        }
    }

    var lastInsert = 0;
    foreach(var pos in Enumerable.Range(0, lines.First().Length).Where(q => !posList.Contains(q)))
    {
        for (var x = 0; x < newList.Count(); x++)
        {
            newList[x] = newList[x].Insert(pos+lastInsert, ".");
        }
        lastInsert++;
    }

    var collection = GenerateCollections(newList);

    var path = new List<long>();
    for(var x = 0; x < collection.NumberCollection.Count(); x++)
    {
        for(var y = x + 1; y < collection.NumberCollection.Count(); y++)
        {
            path.Add(Shortest(collection.NumberCollection[x], collection.NumberCollection[y]));
        }
    }

    return path;
};

var ProcessInputStep2 = (List<string> lines, int iterations) =>
{
    HashSet<NumberList> res = new HashSet<NumberList>();
    
    var newList = new List<string>();
    var posList = new List<int>();
    var emptyrows = new List<int>();
    var emptycols = new List<int>();
    var hasExpansion = false;
    for (var x = 0; x < lines.Count(); x++)
    {
        if (!lines[x].Contains('#'))
        {
            hasExpansion = true;
        }

        if (hasExpansion)
        {
            emptyrows.Add(x);
            hasExpansion = false;
        }
        newList.Add(lines[x]);

        for (var y = 0; y < lines[x].Length; y++)
        {
            if (lines[x][y] == '#')
            {
                posList.Add(y);
            }
        }
    }

    emptycols.AddRange(Enumerable.Range(0, lines.First().Length).Where(q => !posList.Contains(q)));

    var collection = GenerateCollections(newList); 

    var path = new List<long>();
    foreach(var galaxy in collection.NumberCollection)
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

    return path;
};

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues.Sum();
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input, 999999);
result = "Step 2: " + CalValues2.Sum();
PrintOutput(result);


public class Collection(List<NumberList> numberList, List<SpecialList> specialList)
{
    public List<NumberList> NumberCollection { get; set; } = numberList;
    public List<SpecialList> SpecialCollection { get; set; } = specialList;
}

public class NumberList(long Line, long Index)
{
    public long Line { get; set; } = Line;
    public long Index { get; set; } = Index;
}

public class SpecialList(long Line, long Index)
{
    public long Line { get; set; } = Line;
    public long Index { get; set; } = Index;
}

