namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly List<string> _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
    }

    private int GetNumber (string input, string color, string defaultvalue = "0")
    {
        return int.Parse((input.Split(',')
                .FirstOrDefault(q => q.Contains(color, StringComparison.InvariantCultureIgnoreCase)) ?? defaultvalue)
                    .Replace(" ", "")
                    .Replace(color, "", StringComparison.InvariantCultureIgnoreCase));
    }

    private int ProcessInput1 (List<string> lines, int r, int g, int b)
    {
        var possibleGameSet = 0;
        foreach (var game in lines)
        {
            var split = game.Split(":");
            var gameNumber = int.Parse(split[0].Replace(" ", "").Replace("Game", ""));
            var revealedSets = split[1].Split(";");
            var possibleset = true;
            foreach (var item in revealedSets)
            {
                var red = GetNumber(item, "red", "0");
                var green = GetNumber(item, "green", "0");
                var blue = GetNumber(item, "blue", "0");
                if (red > r || green > g || blue > b) possibleset = false;
            }
            possibleGameSet += possibleset ? gameNumber : 0;
        }
        return possibleGameSet;
    }

    private int ProcessInput2 (List<string> lines)
    {
        List<int> powergame = new List<int>();
        foreach (var game in lines)
        {
            var split = game.Split(":");
            var revealedSets = split[1].Split(";");
            var setTotal = new { red = new List<int>(), green = new List<int>(), blue = new List<int>() };
            foreach (var item in revealedSets)
            {
                setTotal.red.Add(GetNumber(item, "red", "1"));
                setTotal.green.Add(GetNumber(item, "green", "1"));
                setTotal.blue.Add(GetNumber(item, "blue", "1"));
            }
            powergame.Add(setTotal.red.Max() * setTotal.green.Max() * setTotal.blue.Max());
        }
        return powergame.Sum();
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1(_input, 12, 13, 14)}");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2(_input)}");
}
