using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public static class NewGameCreator
{
    public static Response CreateNewGame(Response response, string gameKey, int numPlayers, int[] shuffles, Dictionary<string, Game> _games)
    {
        /*
        * Para crear un juego nuevo, se deben cumplir 4 condiciones:
        *  1. No existe otro juego con el mismo nombre.
        *  2. El número de jugadores es entre 2 y 10.
        *  3. El mazo es revuelto al menos una vez (i.e., shuffles.Length > 0).
        *  4. Cada revoltura debe usar entre 2 y 12 grupos de cartas (i.e., 1 < shuffles[i] < 13).
        */
        bool GameExists = SeeIfGameExists(response, gameKey, _games);
        bool IsValidNumberOfPlayers = SeeIfThereIsValidNumberOfPlayers(response, numPlayers);
        bool IsValidShuffle = SeeIfShuffleIsValid(response, shuffles);
        if (GameExists && IsValidNumberOfPlayers && IsValidShuffle)
        {
            _games[gameKey] = new Game(numPlayers, shuffles);
        }
        return response;
    }

    public static bool SeeIfGameExists(Response response, string gameKey, Dictionary<string, Game> _games)
    {
        if (_games.ContainsKey(gameKey))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "This game already exists.";
            return false;
        }
        return true;
    }

    public static bool SeeIfThereIsValidNumberOfPlayers(Response response, int numPlayers)
    {
        if (numPlayers < 2 || numPlayers > 10)
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "The number of players is invalid.";
            return false;
        }
        return true;
    }

    public static bool SeeIfShuffleIsValid(Response response, int[] shuffles)
    {
        bool isShuffleValid = shuffles.Length > 0;
        foreach (var shuffle in shuffles)
            isShuffleValid = isShuffleValid && shuffle > 1 && shuffle < 13;
        if (!isShuffleValid)
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "Invalid shuffle.";
        }
        return isShuffleValid;
    }
}