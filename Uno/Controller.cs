using System.Diagnostics;

namespace Uno;

public class Controller
{
    // Acá guardamos todos los juegos que han sido creados a partir de sus nombres.
    private readonly Dictionary<string, Game> _games = new ();

    /// <summary>
    /// This method enables the creation of a new game, retrieval of information about an existing game,
    /// exploration of available play options for the current player, and submission of a play.
    /// Each action has specific conditions for success (e.g., we cannot get information from a game that doesn't exist).
    /// </summary>
    /// <param name="request">
    /// Action to be executed, which can be:
    /// <list type="bullet">
    ///     <item> <description><c>"New game"</c>: Creates a new game.</description> </item>
    ///     <item> <description><c>"Get game info"</c>: Retrieves information about a game.</description> </item>
    ///     <item> <description><c>"Show options"</c>: Displays possible plays for the current player.</description> </item>
    ///     <item> <description><c>"Play"</c>: Submits a play.</description> </item>
    /// </list>
    /// </param>
    /// <param name="gameKey">
    /// Name of the game. It is expected that a game with this name does not exist
    /// when creating a new game. In any other case, the game should already exist.
    /// </param>
    /// <param name="numPlayers">
    /// Indicates the number of players. This parameter is only relevant when creating a new game.
    /// </param>
    /// <param name="shuffles">
    /// An array indicating how many times to shuffle the deck. This parameter is only relevant when
    /// creating a new game.
    /// </param>
    /// <param name="idPlayer">
    /// The ID of the player making the request. This ID is only used for <c>"Show options"</c> and <c>"Play"</c>.
    /// </param>
    /// <param name="selectedPlay">
    /// The ID of the play to be performed. It is only used when <c>request</c> is <c>"Play"</c>.
    /// </param>
    /// <returns>
    /// A <c>Response</c> object containing the requested data.
    /// </returns>
    public Response DoTheThing(string request, string gameKey, int numPlayers, int[] shuffles, int idPlayer,
        int selectedPlay)
    {
        Response response = new Response();
        
        if (request == "New game")
        {
            /*
             * Para crear un juego nuevo, se deben cumplir 4 condiciones:
             *  1. No existe otro juego con el mismo nombre.
             *  2. El número de jugadores es entre 2 y 10.
             *  3. El mazo es revuelto al menos una vez (i.e., shuffles.Length > 0).
             *  4. Cada revoltura debe usar entre 2 y 12 grupos de cartas (i.e., 1 < shuffles[i] < 13).
             */
            if (_games.ContainsKey(gameKey))
            {
                response.WasRequestSuccessful = false;
                response.ErrorMessage = "This game already exists.";
            }
            else if (numPlayers < 2 || numPlayers > 10)
            {
                response.WasRequestSuccessful = false;
                response.ErrorMessage = "The number of players is invalid.";
            }
            else
            {
                bool isShuffleValid = shuffles.Length > 0;
                foreach (var shuffle in shuffles)
                    isShuffleValid = isShuffleValid && shuffle > 1 && shuffle < 13;

                if (!isShuffleValid)
                {
                    response.WasRequestSuccessful = false;
                    response.ErrorMessage = "Invalid shuffle.";
                }
                else
                {
                    _games[gameKey] = new Game(numPlayers, shuffles);
                }
            }
        }
        if (request == "Get game info")
        {
            /*
             * Para obtener la información de un juego actual la única condición es que ese juego exista.
             */
            if (!_games.ContainsKey(gameKey))
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
        if (request == "Show options")
        {
            /*
             * Acá mostramos las opciones de jugadas que puede realizar el jugador actual.
             * Existen dos tipos de jugadas. Normalmente, el jugador actual debe elegir
             * la siguiente carta a jugar. En ese caso las opciones consisten en elegir una
             * de las cartas que tiene en la mano. El otro caso es cuando el jugador debe
             * elegir un color. Este caso se da cuando la última carta jugada es un Wild.
             *
             * Además existen dos casos de error. El primero es cuando el juego no existe.
             * El segundo es cuando el jugador que hizo el request NO es el jugador que
             * tiene el turno actual.
             */
            if (!_games.ContainsKey(gameKey))
            {
                response.WasRequestSuccessful = false;
                response.ErrorMessage = "This game does not exist.";
            }
            else
            {
                response.Options = _games[gameKey].GetOptionsForCurrentPlayer(idPlayer);
                if (response.Options == null)
                {
                    response.WasRequestSuccessful = false;
                    response.ErrorMessage = "You are not the current player.";
                }
            }
        }

        if (request == "Play")
        {
            /*
             * Acá es cuando se realiza una jugada (que puede ser intentar jugar una carta o elegir un color).
             * Existen varios casos de error, comenzando porque no exista el juego.
             * El resto de los casos de error incluyen que el jugador que hizo el request no sea el jugador que
             * tiene el turno, que haya elegido una jugada inválida, etc.
             * ... pero si la jugada fue exitosa se retorna "ok"
             */
            if (!_games.ContainsKey(gameKey))
            {
                response.WasRequestSuccessful = false;
                response.ErrorMessage = "This game does not exist.";
            }
            else
            {
                string result = _games[gameKey].Play(idPlayer, selectedPlay);
                if (result != "Ok")
                {
                    response.WasRequestSuccessful = false;
                    response.ErrorMessage = result;
                }
            }

        }

        return response;
    }
}