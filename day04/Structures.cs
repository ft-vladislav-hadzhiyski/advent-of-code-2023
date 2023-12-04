using System.Diagnostics;

public class Card(int number, bool isWinning, IEnumerable<int> cards)
{
    public int Number { get; } = number;
    public bool IsWinning { get; } = isWinning;
    public IEnumerable<int> Winning { get; } = cards;

    public int Instances { get; set; } = 0;

    public override string ToString()
    {
        return $"{Number} Winning={IsWinning}:{Winning.Count()} Instances:{Instances}";
    }
}

static class WinningCards
{
    public static void CountInstances(IEnumerable<Card> allCards, IEnumerable<Card> cards)
    {
        foreach (var card in cards)
        {
            if (card.IsWinning)
            {
                var winningCards = allCards.Where(c => card.Winning.Contains(c.Number)).ToList();
                CountInstances(allCards, winningCards);
            }

            card.Instances += 1;
        }
    }
}
