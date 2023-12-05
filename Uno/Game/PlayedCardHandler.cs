using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public class PlayedCardHandler
{
    private GameState _gameState;
    private TurnController _turnController;
    private Dealer _dealer;

    public PlayedCardHandler(GameState gameState, TurnController turnController, Dealer dealer)
    {
        _gameState = gameState;
        _turnController = turnController;
        _dealer = dealer;
    }

    public string PlayCard(int idPlay)
    {
        string status = "";
        Card card = _gameState.CurrentPlayer.GetCard(idPlay);
        if (playIsValid(card))
        {
            ManagePlayedCard(card, idPlay);
            status = "Ok";
        }
        else
            status = "You cannot play that card.";
        return status;
    }

    public bool playIsValid(Card card)
    {
        return (_gameState.CurrentTarget.DoTheyHaveTheSameColorOrValue(card) || card.IsMulticolor());
    }

    public void ManagePlayedCard(Card card, int idPlay)
    {
        _gameState.CurrentPlayer.TakeCard(idPlay);
        _gameState.DiscardPile.Add(card);
        _gameState.CurrentTarget = card.Clone();
        ApplyCardEffect(false);
        _turnController.AdvanceTurn();
    }

    public void ApplyCardEffect(bool isFirstTurn)
    {
        _turnController.UpdatePlayerWhoSelectsColorIfNeeded();
        Value cardValue = _gameState.CurrentTarget.GetValue();
        GameEffects gameEffects = new GameEffects(_gameState, _turnController, _dealer);

        switch (cardValue)
        {
            case Value.Reverse:
                gameEffects.HandleReverseEffect();
                break;
            case Value.Skip:
                gameEffects.HandleSkipEffect();
                break;
            case Value.DrawTwo:
                gameEffects.HandleDrawTwoEffect(isFirstTurn);
                break;
            case Value.WildDrawFour:
                gameEffects.HandleWildDrawFourEffect();
                break;
        }
    }
}