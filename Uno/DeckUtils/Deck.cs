using Uno.Cards;

namespace Uno.DeckUtils;

public class Deck
{
    internal Queue<Card> Cards = new ();

    public void Add(Card card)
        => Cards.Enqueue(card);

    public virtual Card Draw()
        => Cards.Dequeue();

    protected bool IsEmpty()
        => Cards.Count == 0;

    public bool IsNotEmpty()
        => !IsEmpty();
}