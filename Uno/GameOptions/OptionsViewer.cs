using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public static class OptionsViewer
{
    public static Response ShowOptions(Response response, string gameKey, int idPlayer, 
    Dictionary<string, Game> _games)
    {
        bool GameExists = SeeIfGameExists(gameKey, _games);
        if (GameExists)
        {
            SeeIfPlayerIsCurrentPlayer(response, gameKey, idPlayer, _games);
        }
        WriteResponse(response, gameKey, _games);
        return response;
    }

    private static bool SeeIfGameExists(string gameKey, Dictionary<string, Game> _games)
    {
        return _games.ContainsKey(gameKey);
    }

    private static void SeeIfPlayerIsCurrentPlayer(Response response, string gameKey, int idPlayer, 
    Dictionary<string, Game> _games)
    {
        response.Options = _games[gameKey].GetOptionsForCurrentPlayer(idPlayer);
    }

    private static void WriteResponse(Response response, string gameKey, Dictionary<string, Game> _games)
    {
        if (!SeeIfGameExists(gameKey, _games))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "This game does not exist.";
        }
        else if (response.Options == null)
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "You are not the current player.";
        }
    }
}