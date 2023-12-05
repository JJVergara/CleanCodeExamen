using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public class Game
{
    private GameState _gameState;
    private TurnController _turnController;
    private Dealer _dealer;
    
    public Game(int numPlayers, int[] shuffles)
    {
        _gameState = new GameState();
        _turnController = new TurnController(_gameState);
        _dealer = new Dealer(_gameState, shuffles);
        _dealer.DealInitialHand(numPlayers);
        _dealer.PutFirstCardOnDiscardPile();
        PlayedCardHandler playedCardHandler = new PlayedCardHandler(_gameState, _turnController, _dealer);
        playedCardHandler.ApplyCardEffect(true);

    }

    public bool GetIfPlayerIsWinner()
    {
        for(int i = 0; i < _gameState.NumOfPlayers; i++)
            if (!_gameState.Players[i].HasCardsInHand())
                return true;
        return false;
    }
    
    public string[]? GetOptionsForCurrentPlayer(int playerId)
    {
        if (playerId != _turnController.GetCurrentPlayerId())
            return null;
        string[] options = GetPlayerOptionsForTurn();
        return options;
    }
    
    public string[] GetPlayerOptionsForTurn()
    {
        List<string> options = new List<string>();
        if (_gameState.CurrentTarget.IsMulticolor())
        {
            options = AddColorSelectionOption(options);
        }
        else
        {
            options = AddCardSelectionOption(options);
        }
        return options.ToArray();
    }

    public List<string> AddColorSelectionOption(List<string> options)
    {
        options.Add("Select a color: 0- Red, 1- Blue, 2- Yellow, 3- Green.");
        return options;
    }

    public List<string> AddCardSelectionOption(List<string> options)
    {
        Player player = _gameState.CurrentPlayer;
        options.Add("Which card do you want to play? (enter -1 to pass)");
        for (int i = 0; i < player.GetNumOfCardsInHand(); i++)
            options.Add($"{i}- {player.GetCard(i)}");
        return options;
    }

    public GameInfo GetGameInfo()
    {
        bool isGameOver = GetIfPlayerIsWinner(); 
        string currentCard = _gameState.CurrentTarget.ToString();
        int currentPlayer = _turnController.GetCurrentPlayerId();
        int nextPlayer = _turnController.GetNextPlayerId();
        int[] numOfCardsInPlayersHands = GetNumberOfCardsInPlayersHands();
        return new GameInfo(isGameOver, currentCard, currentPlayer, nextPlayer, numOfCardsInPlayersHands);
    }

    public int[] GetNumberOfCardsInPlayersHands()
    {
        int[] numOfCardsInPlayersHands = new int[_gameState.NumOfPlayers];
        for (int i = 0; i < _gameState.NumOfPlayers; i++)
            numOfCardsInPlayersHands[i] = _gameState.Players[i].GetNumOfCardsInHand();
        return numOfCardsInPlayersHands;
    }

    public string GetPlayStatus(int playerId, int selectedPlay)
    {
        string status = "";
        if (GetIfPlayerIsWinner())
        {
            status = "Someone already won this game.";
        }
        else if (playerId != _turnController.GetCurrentPlayerId())
        {
            status = "You are not the current player.";
        }
        else if (_gameState.CurrentTarget.IsMulticolor() && SeeIfSelectedColorIsValid(selectedPlay))
        {
            _gameState.CurrentTarget.SetColor(selectedPlay);
            status = "Ok";
        }
        else if (_gameState.CurrentTarget.IsMulticolor() && !SeeIfSelectedColorIsValid(selectedPlay))
        {
            status = "Your color choice is invalid.";
        }
        else
        {
            string PlayStatus = ManageTurn(playerId, selectedPlay);
            status = PlayStatus;
        }
        return status;
    }

    public string ManageTurn(int playerId, int selectedPlay)
    {
        bool cardChoiceIsInvalid = SeeIfCardChoiceIsInvalid(playerId, selectedPlay);
        int idPlay = selectedPlay;
        string status = "";
        PlayedCardHandler playedCardHandler = new PlayedCardHandler(_gameState, _turnController, _dealer);

        if (idPlay != -1 && !cardChoiceIsInvalid)
        {
            status = playedCardHandler.PlayCard(idPlay);
        }
        else if (cardChoiceIsInvalid)
        {
            status = "Your card choice is invalid.";
        }
        else
        {
            ManageEndTurn();
        }
        return status;
    }

    public bool SeeIfCardChoiceIsInvalid(int playerId, int selectedPlay)
    {
        return (selectedPlay < -1 || selectedPlay >= _gameState.Players[playerId].GetNumOfCardsInHand());
    }

    public void ManageEndTurn()
    {
        _dealer.GiveCardToCurrentPlayer();
        _turnController.AdvanceTurn();
    }

    public bool SeeIfSelectedColorIsValid(int selectedPlay)
    {
        if (selectedPlay < 0 || selectedPlay > 3)
        {
            return false;
        }
        _gameState.CurrentTarget.SetColor(selectedPlay);
        return true;
    }
}