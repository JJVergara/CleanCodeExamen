using Uno.Cards;

namespace Uno;

public class Player
{
    private readonly List<Card> _cards = new ();

    public void GiveCard(Card card)
        => _cards.Add(card);

    public Card GetCard(int position)
        => _cards[position];

    public void TakeCard(int position)
        => _cards.RemoveAt(position);

    public int GetNumOfCardsInHand()
        => _cards.Count;

    public bool HasCardsInHand()
        => GetNumOfCardsInHand() > 0;

}