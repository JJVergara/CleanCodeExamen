using Uno.Cards;

namespace Uno.Controllers;

public class TurnController
{
    private readonly GameState _gameState;

    public TurnController(GameState gameState)
        => _gameState = gameState;

    public void AdvanceTurn()
        => _gameState.CurrentPlayerId = ComputeNextPlayer();
    
    private int ComputeNextPlayer()
    {
        int currentPlayerId = _gameState.CurrentPlayerId;
        currentPlayerId += _gameState.Direction + _gameState.NumOfPlayers;
        currentPlayerId %= _gameState.NumOfPlayers;
        return currentPlayerId;
    }

    public int GetCurrentPlayerId()
    {
        Card currentTarget = _gameState.CurrentTarget;
        return currentTarget.IsMulticolor()? _gameState.PlayerWhoSelectsNextColor : _gameState.CurrentPlayerId;
    }
    
    public int GetNextPlayerId()
    {
        Card currentTarget = _gameState.CurrentTarget;
        return currentTarget.IsMulticolor()? _gameState.CurrentPlayerId : ComputeNextPlayer();
    }

    public void ChangeDirection()
        => _gameState.Direction *= -1;

    public void UpdatePlayerWhoSelectsColorIfNeeded()
    {
        if (_gameState.CurrentTarget.IsMulticolor())
            _gameState.PlayerWhoSelectsNextColor = _gameState.CurrentPlayerId;
    }
}