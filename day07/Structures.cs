enum HandResult
{
    HighCard,
    OnePair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind,
}

class HandComparer : IComparer<Hand>
{

    private static readonly Dictionary<char, int> CardsPower = new()
    {
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'T', 10},
        {'J', 11},
        {'Q', 12},
        {'K', 13},
        {'A', 14}
    };
    public int Compare(Hand? x, Hand? y)
    {
        var hcmp = x.Result.CompareTo(y.Result);

        if (hcmp != 0)
        {
            return hcmp;
        }

        for (var i = 0; i < x.Cards.Length; i += 1)
        {
            var cx = x.Cards[i];
            var cy = y.Cards[i];

            var xp = CardsPower[cx];
            var yp = CardsPower[cy];

            if (xp > yp)
            {
                return 1;
            }
            if (xp < yp)
            {
                return -1;
            }
        }

        return 0;
    }
}

class JackJokerHandComparer : IComparer<Hand>
{
    private static readonly Dictionary<char, int> CardsPower = new()
    {
        {'J', 1},
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'T', 10},
        {'Q', 12},
        {'K', 13},
        {'A', 14}
    };

    public int Compare(Hand? x, Hand? y)
    {
        var hcmp = x.Result.CompareTo(y.Result);

        if (hcmp != 0)
        {
            return hcmp;
        }

        for (var i = 0; i < x.Cards.Length; i += 1)
        {
            var cx = x.Cards[i];
            var cy = y.Cards[i];

            var xp = CardsPower[cx];
            var yp = CardsPower[cy];

            if (xp > yp)
            {
                return 1;
            }
            if (xp < yp)
            {
                return -1;
            }
        }

        return 0;
    }
}

class Hand
{
    private static readonly Dictionary<char, int> CardsPowerWithoutJ = new()
    {
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'T', 10},
        {'Q', 12},
        {'K', 13},
        {'A', 14}
    };

    private Hand(char[] cards, HandResult result, int winning)
    {
        Cards = cards;
        Value = new string(cards);
        Result = result;
        Winning = winning;
    }
    public char[] Cards { get; private set; }
    public string Value { get; private set; }
    public HandResult Result { get; private set; }
    public int Winning { get; private set; }

    public override string ToString()
    {
        return $"{Value}:{Result}";
    }

    public static Hand Create(char[] cards, int winning, bool jackJoker = false)
    {
        var result = HandResult.HighCard;
        var groups = cards.GroupBy(c => c).ToList();
        var jokersGroup = groups.SingleOrDefault(gr => gr.Key == 'J');

        if (jackJoker && jokersGroup is not null)
        {
            var groupsWithoutJ = groups.Where(gr => gr.Key != 'J').ToList();
            groupsWithoutJ.Sort((x, y) =>
            {
                var lengthComp = x.Count().CompareTo(y.Count());
                if (lengthComp == 0)
                {
                    return CardsPowerWithoutJ[x.Key].CompareTo(CardsPowerWithoutJ[y.Key]);
                }

                return lengthComp;
            });

            if (groupsWithoutJ.Count > 0)
            {
                var longestGroupKey = groupsWithoutJ.Last().Key;

                var cardsWithoutJ = cards.Select(card =>
                {
                    if (card == 'J')
                    {
                        return longestGroupKey;
                    }

                    return card;
                }).ToArray();

                groups = cardsWithoutJ.GroupBy(c => c).ToList();
            }
        }

        if (groups.Any(g => g.Count() == 5))
        {
            result = HandResult.FiveOfAKind;
        }
        else if (groups.Any(g => g.Count() == 4))
        {
            result = HandResult.FourOfAKind;
        }
        else if (groups.Any(g => g.Count() == 2) && groups.Any(g => g.Count() == 3))
        {
            result = HandResult.FullHouse;
        }
        else if (groups.Any(g => g.Count() == 3))
        {
            result = HandResult.ThreeOfAKind;
        }
        else if (groups.Where(g => g.Count() == 2).Count() == 2)
        {
            result = HandResult.TwoPairs;
        }
        else if (groups.Where(g => g.Count() == 2).Count() == 1 && groups.Where(g => g.Count() != 2).All(g => g.Count() == 1))
        {
            result = HandResult.OnePair;
        }

        return new Hand(cards, result, winning);
    }
}
