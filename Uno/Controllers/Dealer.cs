using Uno.Cards;
using Uno.DeckUtils;

namespace Uno.Controllers;

public class Dealer
{
    private readonly GameState _gameState;

    public Dealer(GameState gameState, int[] shuffles)
    {
        _gameState = gameState;
        _gameState.DrawPile = new ShuffleableDeck(shuffles, _gameState.DiscardPile);
    }

    public void DealInitialHand(int numOfPlayers)
    {
        CreatePlayers(numOfPlayers);
        DealSevenCardsToEachPlayer();
    }

    private void CreatePlayers(int numOfPlayers)
    {
        _gameState.Players = new List<Player>();
        for(int i = 0; i < numOfPlayers; i++)
            _gameState.Players.Add(new Player());
    }

    private void DealSevenCardsToEachPlayer()
    {
        for (int j = 0; j < 7; j++)
        for (int i = 0; i < _gameState.NumOfPlayers; i++)
            _gameState.Players[i].GiveCard(_gameState.DrawPile.Draw());
    }

    public void PutFirstCardOnDiscardPile()
    {
        Card firstCard = DrawCardThatIsNotWildDrawFour();
        _gameState.DiscardPile.Add(firstCard);
        _gameState.CurrentTarget = firstCard.Clone();
    }

    private Card DrawCardThatIsNotWildDrawFour()
    {
        Card firstCard = _gameState.DrawPile.Draw();
        while (firstCard.Is(Value.WildDrawFour))
        {
            _gameState.DrawPile.Add(firstCard);
            firstCard = _gameState.DrawPile.Draw();
        }

        return firstCard;
    }

    public void GiveCardsToCurrentPlayer(int numOfCards)
    {
        for(int i = 0; i < numOfCards; i++)
            GiveCardToCurrentPlayer();
    }

    public void GiveCardToCurrentPlayer()
    {
        Player currentPlayer = _gameState.CurrentPlayer;
        currentPlayer.GiveCard(_gameState.DrawPile.Draw());
    }
}