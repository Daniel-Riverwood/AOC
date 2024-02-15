using Utilities;

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var HASHConvert = (List<string> cols) =>
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
};

var ProcessInputStep1 = (string lines) => HASHConvert(GenerateCollections(lines)).Sum();

var ProcessInputStep2 = (string lines) =>
{
    var cols = GenerateCollections(lines);

    var boxList = new Dictionary<int, List<Collection>>();
    for(var x = 0; x < 256; x++)
    {
        boxList[x] = new List<Collection>();
    }
    foreach(var col in cols)
    {
        var charList = col.Split(",");
        foreach(var convert in charList)
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
            if(convert.Contains('-'))
            {
                var rem = boxList[shortres].LastOrDefault(q => q.Pre == convert.Split('-')[0]);
                if (rem == null) continue;
                boxList[shortres].Remove(rem);
            }
            else if(convert.Contains("="))
            {
                var split = convert.Split('=');
                var newLens = new Collection(split[0], Convert.ToInt32(split[1]));
                var existing = boxList[shortres].FirstOrDefault(q => q.Pre == newLens.Pre);
                if(existing == null)
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


    return boxList.Sum(q => q.Value.Sum(e => (q.Key+1)*(q.Value.IndexOf(e)+1)*(e.Post)));
};

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);


public class Collection(string pre, int post)
{
    public string Pre { get; set; } = pre;
    public int Post { get; set; } = post;
}

