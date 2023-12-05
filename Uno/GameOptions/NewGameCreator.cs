namespace Uno;

public static class NewGameCreator
{
    public static Response CreateNewGame(Response response, string gameKey, int numPlayers, 
    int[] shuffles, Dictionary<string, Game> _games)
    {
        bool GameExists = SeeIfGameExists(gameKey, _games);
        bool IsValidNumberOfPlayers = SeeIfThereIsValidNumberOfPlayers(numPlayers);
        bool IsValidShuffle = SeeIfShuffleIsValid(shuffles);
        WriteResponse(response, gameKey, numPlayers, shuffles, _games);

        if (GameExists && IsValidNumberOfPlayers && IsValidShuffle)
        {
            _games[gameKey] = new Game(numPlayers, shuffles);
        }
        return response;
    }

    private static bool SeeIfGameExists(string gameKey, Dictionary<string, Game> _games)
    {
        return !_games.ContainsKey(gameKey);
    }

    private static bool SeeIfThereIsValidNumberOfPlayers(int numPlayers)
    {
        return numPlayers >= 2 && numPlayers <= 10;
    }

    private static bool SeeIfShuffleIsValid(int[] shuffles)
    {
        bool isShuffleValid = shuffles.Length > 0;
        foreach (var shuffle in shuffles)
            isShuffleValid = isShuffleValid && shuffle > 1 && shuffle < 13;
        return isShuffleValid;
    }

    private static void WriteResponse(Response response, string gameKey, int numPlayers, int[] shuffles, Dictionary<string, Game> _games)
    {
        if (!SeeIfGameExists(gameKey, _games))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "This game already exists.";
        }
        if (!SeeIfThereIsValidNumberOfPlayers(numPlayers))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "The number of players is invalid.";
        }
        if (!SeeIfShuffleIsValid(shuffles))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "Invalid shuffle.";
        }
    }
}