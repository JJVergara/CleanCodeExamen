using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public class Game
{
    // El _gameState es una estructura de datos que contiene todos los parámetros que controlan el juego
    private GameState _gameState;
    // El _turnController nos permite cambiar el turno y ajustar le dirección en que avanzamos
    private TurnController _turnController;
    // El _dealer es un controller que crea el mazo, revuelve el mazo y reparte cartas
    private Dealer _dealer;
    
    public Game(int numPlayers, int[] shuffles)
    {
        _gameState = new GameState();
        _turnController = new TurnController(_gameState);
        _dealer = new Dealer(_gameState, shuffles);
        _dealer.DealInitialHand(numPlayers);
        _dealer.PutFirstCardOnDiscardPile();
        
        // aplicamos el efecto de la carta recién jugada (de tener alguno)
        ApplyCardEffect(true);

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
        List<string> options = new List<string>();
        if (playerId != _turnController.GetCurrentPlayerId())
            return null; // caso de error, el jugador que hizo el request no le toca jugar

        // Si el color buscado es multicolor, entonces las opciones a mostrar son los colores posibles a elegir.
        if (_gameState.CurrentTarget.IsMulticolor())
        {
            options.Add("Select a color: 0- Red, 1- Blue, 2- Yellow, 3- Green.");
        }
        else // en otro caso, se muestran las cartas que el jugador tiene en su mano
        {
            Player player = _gameState.CurrentPlayer;
            options.Add("Which card do you want to play? (enter -1 to pass)");
            for (int i = 0; i < player.GetNumOfCardsInHand(); i++)
                options.Add($"{i}- {player.GetCard(i)}");
        }
        return options.ToArray();
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

    public string Play(int playerId, int selectedPlay)
    {
        if (GetIfPlayerIsWinner())
            return "Someone already won this game.";

        if (playerId != _turnController.GetCurrentPlayerId())
            return "You are not the current player.";

        if (_gameState.CurrentTarget.IsMulticolor() && SeeIfSelectedColorIsValid(selectedPlay))
        {
            _gameState.CurrentTarget.SetColor(selectedPlay);
        }

        else if (_gameState.CurrentTarget.IsMulticolor() && !SeeIfSelectedColorIsValid(selectedPlay))
        {
            return "Your color choice is invalid.";
        }

        else // Caso en que se juega una carta
        {
            if (selectedPlay < -1 || selectedPlay >= _gameState.Players[playerId].GetNumOfCardsInHand())
                return "Your card choice is invalid.";

            /*
             * Si selectedPlay es "-1", entonce se pasa y se roba una carta.
             * En otro caso se juega la carta seleccionada
             */
            int idPlay = selectedPlay;
            if (idPlay != -1)
            {
                Card card = _gameState.CurrentPlayer.GetCard(idPlay);

                // La jugada es válida si su color o valor son iguales a la última carta jugada
                // También la jugada es válida si el color de la carta a jugar es "multicolor"
                if (playIsValid(card))
                {
                    ManagePlayedCard(card, idPlay);
                }
                else
                    return "You cannot play that card.";
            }
            else
            {
                ManageEndTurn();
            }
        }

        return "Ok";
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


    public void ApplyCardEffect(bool isFirstTurn)
    {
        // Si la carta es multicolor, indicamos que el jugador actual debe elegir el color de la carta
        _turnController.UpdatePlayerWhoSelectsColorIfNeeded();

        if (_gameState.CurrentTarget.Is(Value.Reverse))
        {
            _turnController.ChangeDirection();
            if (_gameState.NumOfPlayers == 2) // si hay dos jugadores, reverse funciona como skip
                _turnController.AdvanceTurn();

        }

        if (_gameState.CurrentTarget.Is(Value.Skip))
            _turnController.AdvanceTurn();

        if (_gameState.CurrentTarget.Is(Value.DrawTwo))
        {
            if (isFirstTurn)
            {
                // Si es el primer turno, el jugador que parte roba dos cartas
                _dealer.GiveCardsToCurrentPlayer(2);
                // y luego juega el siguiente jugador
                _turnController.AdvanceTurn();
            }
            else
            {
                // Si jugador actual jugó el +2, avanzamos
                _turnController.AdvanceTurn();
                // ... y hacemos que el siguiente jugador robe dos cartas
                _dealer.GiveCardsToCurrentPlayer(2);
            }

        }

        if (_gameState.CurrentTarget.Is(Value.WildDrawFour))
        {
            _turnController.AdvanceTurn();
            _dealer.GiveCardsToCurrentPlayer(4);
        }
    }
}