using Utilities;

using Point = Utilities.Point2D<int>;


var GetDirInd = (Point dir) =>
{
    if (Point.Up == dir) return 3;
    else if (Point.Down == dir) return 1;
    else if (Point.Left == dir) return 2;
    else return 0;
};

void AddToRem (List<List<int>> path, PriorityQueue<(Point pos, int counter, Point dir), long> rem, Dictionary<(Point pos, int counter, Point dir), long> mind, (Point pos, int counter, Point dir) cur, Point dir, int count, bool[,,,] visit)
{
    (Point pos, int count, Point dir) newPos = ((cur.pos.X + dir.X, cur.pos.Y + dir.Y), count, dir);
    if (newPos.pos.X >= 0 && newPos.pos.Y >= 0 && newPos.pos.X < path[0].Count && newPos.pos.Y < path.Count && !visit[newPos.pos.X, newPos.pos.Y, GetDirInd(newPos.dir), newPos.count])
    {
        var newvalue = mind[cur] + path[newPos.pos.Y][newPos.pos.X];
        if (!mind.ContainsKey(newPos))
        {
            mind[newPos] = newvalue;
            rem.Enqueue(newPos, newvalue);
        }
        else if (mind[newPos] > newvalue)
        {
            mind[newPos] = newvalue;
            rem.Enqueue(newPos, newvalue);
        }
    }
}

var GetInput = (string inputFileName) => File.ReadAllLines(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string[] lines) => lines.Select(q => q.Select(s => s - '0').ToList()).ToList();

var FindPath = (List<List<int>> path, Point start, Point direction, int maxRepeat = 2, int minRepeat = 0) =>
{
    long res = 0;
    var mind = new Dictionary<(Point pos, int counter, Point dir), long>();
    mind[((0, 0), 0, direction)] = 0;
    mind[((0, 0), 0, Point.Down)] = 0;
    var visit = new bool[path[0].Count, path.Count, 4, maxRepeat+1];
    var rem = new PriorityQueue<(Point pos, int counter, Point dir), long>();
    rem.Enqueue(((0, 0), 0, direction), 0);
    rem.Enqueue(((0, 0), 0, Point.Down), 0);
    var end = false;
    while (rem.Count > 0 && !end)
    {
        var cur = rem.Dequeue();
        visit[cur.pos.X, cur.pos.Y, GetDirInd(cur.dir), cur.counter] = true;

        if (cur.counter < maxRepeat)
        {
            var dir = cur.dir;
            var count = cur.counter + 1;
            AddToRem(path, rem, mind, cur, dir, count, visit);
        }
        if(cur.counter >= minRepeat)
        {
            if (cur.dir == Point.Right || cur.dir == Point.Left)
            {
                AddToRem(path, rem, mind, cur, Point.Down, 0, visit);
                AddToRem(path, rem, mind, cur, Point.Up, 0, visit);
            }
            else if (cur.dir == Point.Down || cur.dir == Point.Up)
            {
                AddToRem(path, rem, mind, cur, Point.Right, 0, visit);
                AddToRem(path, rem, mind, cur, Point.Left, 0, visit);
            }

            if (cur.pos.X == path[0].Count - 1 && cur.pos.Y == path.Count - 1)
            {
                end = true;
                res = mind[cur];
            }
        }
    }
    return res;
};

var ProcessInputStep1 = (string[] lines) => FindPath(GenerateCollections(lines), (0, 0), Point.Right, 2);

var ProcessInputStep2 = (string[] lines) => FindPath(GenerateCollections(lines), (0, 0), Point.Right, 9, 3);

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);


public class Collection(char cur, Point2D<int> direction)
{
    public char Current { get; set; } = cur;
    public Point2D<int> Direction { get; set; } = direction;
}

