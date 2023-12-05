using Uno.Cards;
using Uno.Controllers;

namespace Uno;

public static class PlayCard
{
    public static Response Play(Response response, string gameKey, int idPlayer, int selectedPlay, 
    Dictionary<string, Game> _games)
    {
        bool GameExists = SeeIfGameExists(gameKey, _games);
        if (GameExists)
        {
            SeeIfPlayIsOk(response, gameKey, idPlayer, selectedPlay, _games);
        }
        WriteResponse(response, gameKey, _games);
        return response;
    }

    private static bool SeeIfGameExists(string gameKey, Dictionary<string, Game> _games)
    {
        return _games.ContainsKey(gameKey);
    }

    private static bool SeeIfPlayIsOk(Response response, string gameKey, int idPlayer, int selectedPlay, 
    Dictionary<string, Game> _games)
    {
        string result = _games[gameKey].GetPlayStatus(idPlayer, selectedPlay);
        if (result != "Ok")
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = result;
            return false;
        }
        return true;
    }

    private static void WriteResponse(Response response, string gameKey, 
    Dictionary<string, Game> _games)
    {
        if (!SeeIfGameExists(gameKey, _games))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "This game does not exist.";
        }
    }
}