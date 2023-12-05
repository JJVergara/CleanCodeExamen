using Uno.Cards;

namespace Uno.DeckUtils;

public static class DeckBuilder
{
    private static readonly Color[] Colors = ColorConverter.GetColors();
    private static readonly Value[] Values = ValueConverter.GetValues();
    private static readonly Value[] ValueWithOneCopy = { Value.V0, Value.Wild, Value.WildDrawFour };
    private static Deck _deck = null!;
    
    public static Deck Build()
    {
        // WARNING: Do not change the order in which the deck is created.
        // Otherwise, the test cases will fail.
        _deck = new Deck();
        foreach (var color in Colors)
        foreach (var value in Values)
            AddCardToDeck(color, value);

        return _deck;
    }

    private static void AddCardToDeck(Color color, Value value)
    {
        _deck.Add(new Card(color, value));
        if(!ValueWithOneCopy.Contains(value))
            _deck.Add(new Card(color, value));
    }
}