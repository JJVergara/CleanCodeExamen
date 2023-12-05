using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public static class OptionsViewer
{
    public static Response ShowOptions(Response response, string gameKey, int idPlayer, Dictionary<string, Game> _games)
    {
        bool GameExists = SeeIfGameExists(gameKey, _games);
        if (!GameExists)
        {
            
        }
        else
        {
            //Volver a Esto
            response.Options = _games[gameKey].GetOptionsForCurrentPlayer(idPlayer);
            if (response.Options == null)
            {
                response.WasRequestSuccessful = false;
                response.ErrorMessage = "You are not the current player.";
            }
        }
        WriteResponse(response, gameKey, idPlayer, _games);
        return response;
    }

    private static bool SeeIfGameExists(string gameKey, Dictionary<string, Game> _games)
    {
        return _games.ContainsKey(gameKey);
    }

    private static bool SeeIfPlayerIsCurrentPlayer(Response response, string gameKey, int idPlayer, Dictionary<string, Game> _games)
    {
        response.Options = _games[gameKey].GetOptionsForCurrentPlayer(idPlayer);
        if (response.Options == null)
        {
            return false;
        }
        return true;
    }

    private static void WriteResponse(Response response, string gameKey, int idPlayer, Dictionary<string, Game> _games)
    {
        if (!SeeIfGameExists(gameKey, _games))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "This game does not exist.";
        }
        else if (!SeeIfPlayerIsCurrentPlayer(response, gameKey, idPlayer, _games))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "You are not the current player.";
        }
    }
}