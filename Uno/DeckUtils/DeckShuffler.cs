using Uno.Cards;

namespace Uno.DeckUtils;

public class DeckShuffler
{
    private readonly int[] _shuffles;
    private Deck _deck;
    private Queue<Card>[] _buckets;

    public DeckShuffler(int[] shuffles)
        => _shuffles = shuffles;

    public Deck Shuffle(Deck deck)
    {
        _deck = deck;
        foreach (int shuffle in _shuffles)
            Shuffle(shuffle);
        return _deck;
    }

    private void Shuffle(int nBuckets)
    {
        CreateBuckets(nBuckets);
        DealCardsToBuckets();
        JoinBucketsToCreateNewDeck();
    }

    private void CreateBuckets(int nBuckets)
    {
        _buckets = new Queue<Card>[nBuckets];
        for (int i = 0; i < nBuckets; i++)
            _buckets[i] = new Queue<Card>();
    }

    private void DealCardsToBuckets()
    {
        for (int i = 0; _deck.IsNotEmpty(); i++)
            _buckets[i%_buckets.Length].Enqueue(_deck.Draw());
    }

    private void JoinBucketsToCreateNewDeck()
    {
        Deck deck = new Deck();
        for (int i = 0; i < _buckets.Length; i++)
            while(_buckets[i].Count > 0)
                deck.Add(_buckets[i].Dequeue());
        _deck = deck;
    }

}