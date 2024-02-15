namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly List<string> _input;
    private readonly Collection2 collection;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
        collection = GenerateCollections(_input);
    }

    private Collection2 GenerateCollections (List<string> lines) 
    {
        var numCollection = new List<NumberList2>();
        foreach (var line in lines)
        {
            var splitLine = line.Split(':');
            var secondSplit = splitLine[1].Split('|');
            var game = int.Parse(splitLine[0].Replace("Card ", "", StringComparison.InvariantCultureIgnoreCase).Trim());
            var winninglist = secondSplit[0].Trim().Split(' ').Where(q => !string.IsNullOrWhiteSpace(q)).Select(q => int.Parse(q)).ToList();
            var selectedList = secondSplit[1].Trim().Split(' ').Where(q => !string.IsNullOrWhiteSpace(q)).Select(q => int.Parse(q)).ToList();

            numCollection.Add(new NumberList2(game, winninglist, selectedList));
        }

        return new Collection2(numCollection);
    }

    private int ProcessInput1 ()
    {
        return collection.NumberCollection.Select(q => {
            var matching = q.SelectedNumbers.Where(s => q.WinningNumbers.Contains(s));
            return Convert.ToInt32(Math.Pow(2, matching.Count() - 1));
        }).Sum();
    }

    private int ProcessInput2 ()
    {
        var winnings = 0;
        int iter = 0;

        while (iter < collection.NumberCollection.Count)
        {
            var game = collection.NumberCollection[iter];
            var matching = game.SelectedNumbers.Where(q => game.WinningNumbers.Contains(q)).ToList();

            for (var i = 1; i <= matching.Count; i++)
            {
                var newGame = collection.NumberCollection.Find(q => q.GameNumber == game.GameNumber + i);
                if (newGame == null) break;
                collection.NumberCollection.Add(newGame);
            }

            iter++;
        }
        winnings = collection.NumberCollection.Count;

        return winnings;
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}, part 1");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}, part 2");
}
public class Collection2(List<NumberList2> numberList)
{
    public List<NumberList2> NumberCollection { get; set; } = numberList;
}

public class NumberList2(int gameNumber, List<int> winningNumbers, List<int> selectedNumbers)
{
    public int GameNumber { get; set; } = gameNumber;
    public List<int> WinningNumbers { get; set; } = winningNumbers;
    public List<int> SelectedNumbers { get; set; } = selectedNumbers;
}
