using System.IO;
using System.Runtime.ConstrainedExecution;

namespace AdventOfCode;

public class Day19 : BaseDay
{
    private readonly List<string> _input;
    
    public Day19()
    {
        _input = File.ReadAllText(InputFilePath).SplitByDoubleNewline();
    }

    private List<Workflow> GenerateWorkflows(string lines) => lines.SplitByNewline().Select(x =>
    {
        var name = x.Split('{')[0];
        var critValues = x.Split('{')[1].Trim('}').Split(',');
        var lastCase = critValues.Last();
        critValues = critValues.Take(critValues.Length - 1).ToArray();
        var critList = critValues.Select(q => new Criteria(q[0].ToString(), q[1] == '>' ? false : true, Convert.ToInt32(q.Substring(2, q.IndexOf(":") - 2)), q.Split(":")[1])).ToList();

        return new Workflow(name, critList, lastCase);
    }).ToList();
    private List<Part> GenerateParts(string lines) => lines.SplitByNewline().Select(q =>
    {
        var col = q.Trim('{').Trim('}').Split(',');
        var x = Convert.ToInt32(col[0].Trim('x').Trim('='));
        var m = Convert.ToInt32(col[1].Trim('m').Trim('='));
        var a = Convert.ToInt32(col[2].Trim('a').Trim('='));
        var s = Convert.ToInt32(col[3].Trim('s').Trim('='));
        return new Part(x, m, a, s, false);
    }).ToList();

    private long Sort(List<string> input, string start)
    {
        var workflows = GenerateWorkflows(input.First());
        var parts = GenerateParts(input.Last());
        var completed = false;
        var partpos = 0;
        var cur = workflows.First(q => q.Name == start);
        while (!completed)
        {
            var curPart = parts[partpos];
        var res = "";
        parts[partpos].FlowSequence.Add(cur.Name);
            foreach (var crit in cur.Criterias)
            {
                switch (crit.CritType)
                {
                    case "x":
                        if (crit.LessGreater)
                        {
                            if (parts[partpos].X<crit.Value)
                            {
                                res = crit.Dest;
                            }
                        }
                        else
                        {
                            if (parts[partpos].X > crit.Value)
                            {
                                res = crit.Dest;
                            }
                        }
                        break;
                                        case "m":
                            if (crit.LessGreater)
                            {
                                if (parts[partpos].M < crit.Value)
                                {
                                    res = crit.Dest;
                                }
                            }
                            else
                            {
                                if (parts[partpos].M > crit.Value)
                                {
                                    res = crit.Dest;
                                }
                            }
                            break;
                        case "a":
                            if (crit.LessGreater)
                            {
                                if (parts[partpos].A < crit.Value)
                                {
                                    res = crit.Dest;
                                }
                            }
                            else
                            {
                                if (parts[partpos].A > crit.Value)
                                {
                                    res = crit.Dest;
                                }
                            }
                            break;
                        case "s":
                            if (crit.LessGreater)
                            {
                                if (parts[partpos].S < crit.Value)
                                {
                                    res = crit.Dest;
                                }
                            }
                            else
                            {
                                if (parts[partpos].S > crit.Value)
                                {
                                    res = crit.Dest;
                                }
                            }
                            break;
                        default:
                            res = cur.OtherCase;
                            break;
                        }
                        if (!string.IsNullOrWhiteSpace(res)) break;
                                }

                                if (string.IsNullOrWhiteSpace(res)) res = cur.OtherCase;

                        if (res == "R")
                        {
                            parts[partpos].Accepted = false;
                            partpos++;
                            cur = workflows.First(q => q.Name == start);
                        }
                        else if (res == "A")
                        {
                            parts[partpos].Accepted = true;
                            partpos++;
                            cur = workflows.First(q => q.Name == start);
                        }
                        else
                        {
                            cur = workflows.First(q => q.Name == res);
                        }

                        if (partpos == parts.Count)
                        {
                            completed = true;
                        }
        }


        return parts.Where(q => q.Accepted).Sum(q => q.X + q.M + q.A + q.S);
    }
    private long calc(DictMultiRange<string> set, string next, List<Workflow> workflows)
    {
        long valid = 0;
        var cur = workflows.First(q => q.Name == next);
        foreach (var crit in cur.Criterias)
        {
            DictMultiRange<string> newset = new(set);
            if (crit.LessGreater)
            {
                if (set.Ranges[crit.CritType].Start < crit.Value)
                {
                    newset.Ranges[crit.CritType].End = Math.Min(newset.Ranges[crit.CritType].End, crit.Value - 1);
                    if (crit.Dest == "A") valid += newset.len;
                    else if (crit.Dest != "R") valid += calc(newset, crit.Dest, workflows);

                    set.Ranges[crit.CritType].Start = crit.Value;
                }
            }
            else
            {
                if (set.Ranges[crit.CritType].End > crit.Value)
                {
                    newset.Ranges[crit.CritType].Start = Math.Max(newset.Ranges[crit.CritType].Start, crit.Value + 1);
                    if (crit.Dest == "A") valid += newset.len;
                    else if (crit.Dest != "R") valid += calc(newset, crit.Dest, workflows);

                    set.Ranges[crit.CritType].End = crit.Value;
                }

            }
        }

        if (cur.OtherCase == "A") valid += set.len;
        else if (cur.OtherCase != "R") valid += calc(set, cur.OtherCase, workflows);

        return valid;
    }

    private long Comb(List<string> input)
    {
        var workflows = GenerateWorkflows(input.First());
        DictMultiRange<string> start = new() { Ranges = new() { { "x", new(1, 4000) }, { "m", new(1, 4000) }, { "a", new(1, 4000) }, { "s", new(1, 4000) } } };

        long allowed = calc(start, "in", workflows);


        return allowed;
    }

    private long ProcessInput1() => Sort(_input, "in");

    private long ProcessInput2() => Comb(_input);

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}");
}
public class Workflow(string Name, List<Criteria> criterias, string otherCase)
{
    public string Name { get; set; } = Name;
    public List<Criteria> Criterias { get; set; } = criterias;
    public string OtherCase { get; set; } = otherCase;
}

public class Criteria(string critType, bool lessGreater, int value, string dest)
{
    public string CritType { get; set; } = critType;
    public bool LessGreater { get; set; } = lessGreater;
    public int Value { get; set; } = value;
    public string Dest { get; set; } = dest;
}

public class Part(int x, int m, int a, int s, bool accepted)
{
    public int X { get; set; } = x;
    public int M { get; set; } = m;
    public int A { get; set; } = a;
    public int S { get; set; } = s;
    public bool Accepted { get; set; } = accepted;
    public List<string> FlowSequence { get; set; } = new List<string>();
}