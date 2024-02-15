using Utilities;

var NodeDict = new Dictionary<string, Nodes>();

var ConnDict = new Dictionary<string, long>();

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var GenerateBroadcaster = (string lines) => {
    var col = lines.SplitByNewline();
    var bc = col.First(q => q.Contains("broadcaster", StringComparison.OrdinalIgnoreCase));

    var nodes = col.Where(q => !q.Contains("broadcaster", StringComparison.OrdinalIgnoreCase));
    var nodeList = bc.Split("->")[1].Trim().Split(',').Select(q => q.Trim()).ToList();
    foreach (var node in nodes)
    {
        var f = node.Split("->");
        var name = f[0].Substring(1).Trim();
        var isFlipFlop = f[0][0] == '%' ? true : false;
        var isConj = f[0][0] == '&' ? true : false;
        var connected = f[1].Trim().Split(',').Select(q => q.Trim()).ToList();
        NodeDict[name] = new Nodes(name, connected, isFlipFlop, isConj);
    }

    foreach(var node in NodeDict.Where(q => q.Value.isConjunction))
    {
        node.Value.InputNodes.AddRange(NodeDict.Where(q => q.Value.connectedNodes.Contains(node.Value.NodeName)).Select(q => q.Value));
    }

    return new Broadcaster(nodeList);
};

var Actions = (Broadcaster start, long actionCount, bool steptwo) =>
{
    var range = steptwo ? 1000000000 : 1000;
    Queue<Nodes> actionQueue = new Queue<Nodes>();
    var step = 0;
    while(step < range)
    {
        step++;
        start.LowPulses++;
        //PrintOutput($"button -false -> broadcaster");
        foreach (var node in start.ConnectedNodes)
        {
            var newNode = new Nodes(NodeDict[node].NodeName, NodeDict[node].connectedNodes, NodeDict[node].isFlipFlop, NodeDict[node].isConjunction);
            start.LowPulses++;
            newNode.OnOff = NodeDict[node].OnOff;
            newNode.HighLow = false;
            //PrintOutput($"broadcaster -false -> {newNode.NodeName}");
            actionQueue.Enqueue(newNode);
        }

        while (actionQueue.Count > 0)
        {
            var cur = actionQueue.Dequeue();

            if (cur.isFlipFlop && !cur.HighLow)
            {
                cur.OnOff = !cur.OnOff;

                if (!cur.OnOff) cur.HighLow = false;
                else cur.HighLow = true;

                NodeDict[cur.NodeName].OnOff = cur.OnOff;

                foreach (var con in cur.connectedNodes)
                {
                    if (cur.HighLow) start.HighPulses++;
                    else start.LowPulses++;
                    if (NodeDict.TryGetValue(con, out var nextnode))
                    {
                        var newNode = new Nodes(nextnode.NodeName, nextnode.connectedNodes, nextnode.isFlipFlop, nextnode.isConjunction);
                        newNode.InputNodes = nextnode.InputNodes;
                        newNode.OnOff = nextnode.OnOff;
                        newNode.HighLow = cur.HighLow;

                        if (newNode.isConjunction)
                        {
                            if (newNode.connectedNodes.Contains("rx"))
                            {
                                if (!ConnDict.ContainsKey(cur.NodeName) && cur.HighLow && steptwo)
                                {
                                    ConnDict[cur.NodeName] = step;
                                }

                                if (ConnDict.Count == newNode.InputNodes.Count)
                                {
                                    return ConnDict.Values.Aggregate((x, y) => Utilities.Utilities.FindLCM(x, y));
                                }
                            }
                            var conjNode = newNode.InputNodes.FirstOrDefault(q => q.NodeName == cur.NodeName);
                            if (conjNode != null)
                            {
                                var ind = newNode.InputNodes.IndexOf(conjNode);
                                newNode.InputNodes[ind].HighLow = cur.HighLow;
                            }
                        }
                        //PrintOutput($"{cur.NodeName} -{cur.HighLow} -> {newNode.NodeName}");
                        actionQueue.Enqueue(newNode);
                    }
                    else
                    {
                        //PrintOutput($"{cur.NodeName} -{cur.HighLow} -> output");
                        continue;
                    }
                }
            }

            if (cur.isConjunction)
            {
                if (cur.InputNodes.All(q => q.HighLow)) cur.OnOff = true;
                else cur.OnOff = false;

                if (!cur.OnOff) cur.HighLow = true;
                else cur.HighLow = false;

                NodeDict[cur.NodeName].OnOff = cur.OnOff;
                NodeDict[cur.NodeName].HighLow = cur.HighLow;

                foreach (var con in cur.connectedNodes)
                {
                    if (cur.HighLow) start.HighPulses++;
                    else start.LowPulses++;
                    if (NodeDict.TryGetValue(con, out var nextnode))
                    {
                        var newNode = new Nodes(nextnode.NodeName, nextnode.connectedNodes, nextnode.isFlipFlop, nextnode.isConjunction);
                        newNode.InputNodes = nextnode.InputNodes;
                        newNode.OnOff = nextnode.OnOff;
                        newNode.HighLow = cur.HighLow;
                        if (newNode.isConjunction)
                        {
                            if (newNode.connectedNodes.Contains("rx"))
                            {
                                if (!ConnDict.ContainsKey(cur.NodeName) && cur.HighLow && steptwo)
                                {
                                    ConnDict[cur.NodeName] = step;
                                }

                                if (ConnDict.Count == newNode.InputNodes.Count)
                                {
                                    return ConnDict.Values.Aggregate((x, y) => Utilities.Utilities.FindLCM(x, y));
                                }
                            }
                            var conjNode = newNode.InputNodes.FirstOrDefault(q => q.NodeName == cur.NodeName);
                            if (conjNode != null)
                            {
                                var ind = newNode.InputNodes.IndexOf(conjNode);
                                newNode.InputNodes[ind].HighLow = cur.HighLow;
                            }
                        }
                        //PrintOutput($"{cur.NodeName} -{cur.HighLow} -> {newNode.NodeName}");
                        actionQueue.Enqueue(newNode);
                    }
                    else
                    {
                        if (con == "rx" && steptwo && !cur.HighLow)
                            return step;
                        //PrintOutput($"{cur.NodeName} -{cur.HighLow} -> output");
                        continue;
                    }
                }
            }
        }
    }

    return start.LowPulses * start.HighPulses;
};


var ProcessInputStep1 = (string lines) => Actions(GenerateBroadcaster(GenerateCollections(lines).First()), 1000, false);

var ProcessInputStep2 = (string lines) => Actions(GenerateBroadcaster(GenerateCollections(lines).First()), 1000, true);

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);

public class Broadcaster(List<string> connectedNodes)
{
    public List<string> ConnectedNodes { get; set; } = connectedNodes;
    public long LowPulses { get; set; } = 0;
    public long HighPulses { get; set; } = 0;
}

public class Nodes(string nodeName, List<string> connectedNodes, bool isFlipFlop, bool isConjunction)
{
    public string NodeName { get; set; } = nodeName;
    public List<string> connectedNodes { get; set; } = connectedNodes;
    public List<Nodes> InputNodes { get; set; } = new List<Nodes>();
    public bool isFlipFlop { get; set; } = isFlipFlop;
    public bool isConjunction { get; set; } = isConjunction;
    public bool OnOff { get; set; }
    public bool HighLow { get; set; } = false;
}
