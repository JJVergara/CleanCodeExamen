using Uno.Cards;

namespace Uno.DeckUtils;

public class ShuffleableDeck : Deck
{
    private readonly DeckShuffler _shuffler;
    private readonly Deck _discardPile;

    public ShuffleableDeck(int[] shuffles, Deck discardPile)
    {
        _discardPile = discardPile;
        _shuffler = new DeckShuffler(shuffles);
        Deck deck = DeckBuilder.Build();
        deck = _shuffler.Shuffle(deck);
        Cards = deck.Cards;
    }

    public override Card Draw()
    {
        if (IsEmpty())
            UseDiscardPileAsDrawPile();
        
        return base.Draw();
    }
    
    private void UseDiscardPileAsDrawPile()
    {
        Deck deck = _shuffler.Shuffle(_discardPile);
        Cards = deck.Cards;
        _discardPile.Cards = new Queue<Card>();
    }
}