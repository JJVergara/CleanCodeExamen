using Uno.Cards;
using Uno.DeckUtils;
using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public class GameEffects
{
    private readonly GameState _gameState;
    private readonly TurnController _turnController;
    private readonly Dealer _dealer;

    public GameEffects(GameState gameState, TurnController turnController, Dealer dealer)
    {
        _gameState = gameState;
        _turnController = turnController;
        _dealer = dealer;
    }

    public void HandleReverseEffect()
    {
        _turnController.ChangeDirection();
        if (_gameState.NumOfPlayers == 2) // si hay dos jugadores, reverse funciona como skip
        {
            _turnController.AdvanceTurn();
        }
    }

    public void HandleSkipEffect()
    {
        _turnController.AdvanceTurn();
    }

    public void HandleDrawTwoEffect(bool isFirstTurn)
    {
        if (isFirstTurn)
        {
            _dealer.GiveCardsToCurrentPlayer(2);
            _turnController.AdvanceTurn();
        }
        else
        {
            _turnController.AdvanceTurn();
            _dealer.GiveCardsToCurrentPlayer(2);
        }
    }

    public void HandleWildDrawFourEffect()
    {
        _turnController.AdvanceTurn();
        _dealer.GiveCardsToCurrentPlayer(4);
    }

}