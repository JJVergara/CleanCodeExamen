namespace Uno;

public static class InfoViewer
{
    public static Response ViewInfo(Response response, string gameKey, Dictionary<string, Game> _games)
    {
        WriteResponse(response, gameKey, _games);
        return response;
    }

    private static bool SeeIfGameExists(string gameKey, Dictionary<string, Game> _games)
    {
        return _games.ContainsKey(gameKey);
    }

    private static void WriteResponse(Response response, string gameKey, Dictionary<string, Game> _games)
    {
        if (!SeeIfGameExists(gameKey, _games))
        {
            response.WasRequestSuccessful = false;
            response.ErrorMessage = "This game does not exist.";
        }
        else
        {
            GameInfo info = _games[gameKey].GetGameInfo();
            response.GameInfo = info;
        }
    }
}