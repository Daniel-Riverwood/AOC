using Utilities;


var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var BuildView = (List<Brick> brickList) =>
{
    Dictionary<string, Brick> topBrick = new Dictionary<string, Brick>();

    foreach(var brick in brickList)
    {
        HashSet<Brick> below = new HashSet<Brick>();
        foreach(var coord in brick.GetAllCoords())
        {
            if(topBrick.TryGetValue(coord, out Brick? value)) below.Add(value);

            topBrick[coord] = brick;
        }

        int newz = below.Count == 0 ? 1 : below.Max(q => q.eZ) + 1;
        brick.eZ = brick.eZ - brick.sZ + newz;
        brick.sZ = newz;

        if(below.Count > 0)
        {
            foreach (var belowBricks in below.Where(q => q.eZ == brick.sZ - 1))
            {
                brick.Below.Add(belowBricks);
                belowBricks.Above.Add(brick);
            }
        }
    }
};

var Actions = (string area, bool stepTwo = false) =>
{

    var bricks = area.SplitByNewline().ToList();

    List<Brick> BrickList = new List<Brick>();
    foreach (var brick in bricks)
    {
        BrickList.Add(new Brick(brick));
    }
    BrickList = BrickList.OrderBy(q => q.sZ).ToList();

    BuildView(BrickList);
    
    long res = 0;

    if(stepTwo)
    {
        foreach(var brick in BrickList)
        {
            List<Brick> rem = new List<Brick>();
            Queue<Brick> affected = new Queue<Brick>();
            affected.Enqueue(brick);

            while (affected.Count > 0)
            {
                var next = affected.Dequeue();
                rem.Add(next);
                foreach(var above in next.Above)
                {
                    if(above.Below.All(rem.Contains))
                    {
                        affected.Enqueue(above);
                        res++;
                    }
                }
            }
        }
    }
    else
    {
        foreach (var brick in BrickList)
        {
            bool canRemove = true;
            foreach (var above in brick.Above) canRemove = above.Below.Count == 1 ? false : canRemove;

            if (canRemove) res++;
        }
    }
    
    return res;
};

var ProcessInputStep1 = (string lines) => Actions(GenerateCollections(lines).First());

var ProcessInputStep2 = (string lines) => Actions(GenerateCollections(lines).First(), true);

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);

public record Brick
{
    public int sX { get; set; }
    public int sY { get; set; }
    public int sZ { get; set; }
    public int eX { get; set; }
    public int eY { get; set; }
    public int eZ { get; set; }

    public readonly List<Brick> Above = new List<Brick>();
    public readonly List<Brick> Below = new List<Brick>();

    public Brick(string line)
    {
        var split = line.Split('~');

        var pos1 = split[0].Split(',');
        sX = int.Parse(pos1[0]);
        sY = int.Parse(pos1[1]);
        sZ = int.Parse(pos1[2]);

        var pos2 = split[1].Split(',');
        eX = int.Parse(pos2[0]);
        eY = int.Parse(pos2[1]);
        eZ = int.Parse(pos2[2]);
    }

    public IEnumerable<string> GetAllCoords()
    {
        for (int x = sX; x <= eX; x++)
        {
            for (int y = sY; y <= eY; y++)
            {
                yield return x + "," + y;
            }
        }
    }
}