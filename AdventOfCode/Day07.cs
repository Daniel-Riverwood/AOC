namespace AdventOfCode;
public class Day07 : BaseDay
{
    private readonly List<string> _input;

    public Day07()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
    }
    private Collection7 GenerateCollections(List<string> lines)
    {
        var res = new List<HandMap>();
        var res2 = new List<HandMap2>();
        foreach (var line in lines)
        {
            var temp = line.Split(' ');
            var hand = temp[0];
            var bid = long.Parse(temp[1]);
            res.Add(new HandMap(hand, bid, new List<char>() { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' }));
            res2.Add(new HandMap2(hand, bid, new List<char>() { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' }));
        }

        return new Collection7(res, res2);
    }

    private long ProcessInput1()
    {
        var collection = GenerateCollections(_input);

        long res = 0;

        var sortedHands = collection.Hands.Order().ToList();

        for (var i = 0; i < sortedHands.Count(); i++)
        {
            res += sortedHands[i].Bid * (i + 1);
        }

        return res;
    }

    private long ProcessInput2()
    {
        var collection = GenerateCollections(_input);

        long res = 0;

        var sortedHands = collection.Hands2.Order().ToList();

        for (var i = 0; i < sortedHands.Count(); i++)
        {
            res += sortedHands[i].Bid * (i + 1);
        }

        return res;
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}, part 1");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}, part 2");
}


public class Collection7(List<HandMap> hands, List<HandMap2> hands2)
{
    public List<HandMap> Hands { get; set; } = hands;
    public List<HandMap2> Hands2 { get; set; } = hands2;
}

public class HandMap(string hand, long bid, List<char> cards) : IComparable<HandMap>
{
    public List<char> Cards { get; set; } = cards;
    public string Hand { get; set; } = hand;
    public long Bid { get; set; } = bid;
    public int CompareTo(HandMap other)
    {
        if (HandType < other.HandType) return 1;
        else if (HandType > other.HandType) return -1;

        for (int i = 0; i < Hand.Length; i++)
        {
            if (Cards.IndexOf(Hand[i]) < Cards.IndexOf(other.Hand[i])) return 1;
            else if (Cards.IndexOf(Hand[i]) > Cards.IndexOf(other.Hand[i])) return -1;
        }
        return 0;
    }

    public Hands HandType => ResultingHand();

    private Hands ResultingHand()
    {
        var group = Hand.GroupBy(x => x);

        if (Hand.All(x => x.Equals(Hand[0]))) return Hands.FiveoK;
        else if (group.Any(q => q.Count() == 4)) return Hands.FouroK;
        else if ((group.Any(q => q.Count() == 3) && group.Any(q => q.Count() == 2))) return Hands.FullHouse;
        else if (group.Any(q => q.Count() == 3)) return Hands.ThreeoK;
        else if (group.Where(q => q.Count() == 2).Count() == 2) return Hands.TwoPair;
        else if (group.Any(q => q.Count() == 2)) return Hands.TwooK;
        else return Hands.HighCard;
    }
}
public class HandMap2(string hand, long bid, List<char> cards) : IComparable<HandMap2>
{
    public List<char> Cards { get; set; } = cards;
    public string Hand { get; set; } = hand;
    public long Bid { get; set; } = bid;
    public int CompareTo(HandMap2 other)
    {
        if (HandType < other.HandType) return 1;
        else if (HandType > other.HandType) return -1;

        for (int i = 0; i < Hand.Length; i++)
        {
            if (Cards.IndexOf(Hand[i]) < Cards.IndexOf(other.Hand[i])) return 1;
            else if (Cards.IndexOf(Hand[i]) > Cards.IndexOf(other.Hand[i])) return -1;
        }
        return 0;
    }

    public Hands HandType => ResultingHand();

    private Hands ResultingHand()
    {
        var group = Hand.GroupBy(x => x);

        if (Hand.All(x => x.Equals(Hand[0])) || (Hand.Contains('J') && Hand.Distinct().Count() == 2)) return Hands.FiveoK;
        else if (group.Any(q => q.Count() == 4 ||
            (q.Key != 'J' && q.Count() == 3 && Hand.Contains('J')) ||
            (q.Key != 'J' && q.Count() == 2 && Hand.Count(x => x == 'J') == 2) ||
            (q.Key != 'J' && q.Count() == 1 && Hand.Count(x => x == 'J') == 3))) return Hands.FouroK;
        else if ((group.Any(q => q.Count() == 3) && group.Any(q => q.Count() == 2)) ||
            (group.Where(q => q.Count() == 2 && q.Key != 'J').Count() == 2 && Hand.Contains('J'))) return Hands.FullHouse;
        else if (group.Any(q => q.Count() == 3 ||
            (q.Key != 'J' && q.Count() == 2 && Hand.Contains('J')) ||
            (q.Key != 'J' && q.Count() == 1 && Hand.Count(x => x == 'J') == 2))) return Hands.ThreeoK;
        else if (group.Where(q => q.Count() == 2).Count() == 2) return Hands.TwoPair;
        else if (group.Any(q => q.Count() == 2) || Hand.Contains('J')) return Hands.TwooK;
        else return Hands.HighCard;
    }
}
public enum Hands
{
    FiveoK,
    FouroK,
    FullHouse,
    ThreeoK,
    TwoPair,
    TwooK,
    HighCard
}
