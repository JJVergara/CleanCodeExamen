using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public static class OptionsViewer
{
    public static void ShowOptions(Response response, string gameKey, int idPlayer, Dictionary<string, Game> _games)
    {
        bool GameExists = SeeIfGameExists(gameKey, _games);
        bool IsCurrentPlayer = SeeIfPlayerIsCurrentPlayer(response, gameKey, idPlayer, _games);
        if (GameExists && IsCurrentPlayer)
        {
            GameInfo info = _games[gameKey].GetGameInfo();
            response.GameInfo = info;
        }
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

    //No se usa pero por casualidad yo creo

    // private static void WriteResponse(Response response, string gameKey, int idPlayer, Dictionary<string, Game> _games)
    // {
    //     if (!SeeIfGameExists(gameKey, _games))
    //     {
    //         response.WasRequestSuccessful = false;
    //         response.ErrorMessage = "This game does not exist.";
    //     }
    //     else if (SeeIfPlayerIsCurrentPlayer(response, gameKey, idPlayer, _games))
    //     {
    //         response.WasRequestSuccessful = false;
    //         response.ErrorMessage = "You are not the current player.";
    //     }
    //     else
    //     {
    //         GameInfo info = _games[gameKey].GetGameInfo();
    //         response.GameInfo = info;
    //     }
    // }
}