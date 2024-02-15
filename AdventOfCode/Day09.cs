namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string _input;
    private readonly Dictionary<string, string> numbers = new Dictionary<string, string>() 
    { { "zero", "ze0o" }, { "one", "o1e" }, { "two", "t2o" }, { "three", "th3ee" }, { "four", "fo4r" }, { "five", "fi5e" }, { "six", "s6x" }, { "seven", "se7en" }, { "eight", "ei8ht" }, { "nine", "ni9e" } };
    
    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    private string ProcessInput1 (string line)
    {
        var convertedList = line.ToCharArray().Where(x => char.IsDigit(x)).Select(y => y - '0').ToList();
        return $"{convertedList.First()}{convertedList.Last()}";
    }

    private string ProcessInput2 (string line)
    {
        foreach (var number in numbers) line = line.Replace(number.Key, number.Value);
        var converted = line.ToCharArray().Where(x => char.IsDigit(x)).Select(y => y - '0').ToList();
        return $"{converted.First()}{converted.Last()}";
    }

    public override ValueTask<string> Solve_1() => new($"Solution to {ClassPrefix} {_input.Split("\n").Select(x => ProcessInput1(x)).Sum(q => int.Parse(q))}, part 1");

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {_input.Split("\n").Select(x => ProcessInput2(x)).Sum(q => int.Parse(q))}, part 2");
}
