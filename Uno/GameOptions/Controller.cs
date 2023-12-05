namespace Uno;

public class Controller
{
    private readonly Dictionary<string, Game> _games = new ();
    public Response ManageGameActions(string request, string gameKey, int numPlayers, int[] shuffles, int idPlayer,
        int selectedPlay)
    {
        Response response = new Response();
        
        switch (request)
        {
            case "New game":
                response = NewGameCreator.CreateNewGame(response, gameKey, numPlayers, shuffles, _games);
                break;
            case "Get game info":
                response = InfoViewer.ViewInfo(response, gameKey, _games);
                break;
            case "Show options":
                response = OptionsViewer.ShowOptions(response, gameKey, idPlayer, _games);
                break;
            case "Play":
                response = PlayCard.Play(response, gameKey, idPlayer, selectedPlay, _games);
                break;
        }
        return response;
    }
}