using Uno;

/*
 * Puedes usar este código para debuggear tu solución.
 * NO revisaremos la limpieza de este archivo.
 * Si quieres, puedes borrar este código.
 */

string gameName = "My game";

Controller controller = new Controller();

Response response =
    SendRequest(controller, "New game", gameName, 4, new[] { 6, 5 }, 0, 0);


List<int> inputs = new List<int>();

bool isGameOver = false;
while (!isGameOver)
{
    response = SendRequest(controller, "Get game info", gameName, 0, null, 0, 0);

    string currentCard = response.GameInfo!.CurrentCard;
    int currentPlayer = (int)response.GameInfo!.CurrentPlayer;
    int nextPlayer = (int)response.GameInfo!.NextPlayer;
    int[] numOfCardsInPlayersHands = response.GameInfo!.NumOfCardsInHands;

    ShowGameInfo(currentCard, currentPlayer, nextPlayer, numOfCardsInPlayersHands);
    
    Console.Write("INPUTS: ");
    foreach (var input in inputs)
        Console.Write($"{input}, ");
    Console.WriteLine();
    
    response = SendRequest(controller, "Show options", gameName, 0, null, currentPlayer, 0);
    ShowOptions(response.Options!);

    int selectedOption = Convert.ToInt32(Console.ReadLine());
    inputs.Add(selectedOption);
    SendRequest(controller, "Play", gameName, 0, null, currentPlayer, selectedOption);

    response = SendRequest(controller, "Get game info", gameName, 0, null, 0, 0);
    isGameOver = (bool)response.GameInfo!.IsGameOver;
    
}


Response SendRequest(Controller controller, string request,
    string gameKey, int numPlayers, int[] shuffles,
    int idPlayer, int selectedPlay)
{
    Response response =
        controller.ManageGameActions(request, gameKey, numPlayers, shuffles, idPlayer, selectedPlay);

    if(!response.WasRequestSuccessful)
        Console.WriteLine($"[ERROR] {response.ErrorMessage}");
    
    return response;
}

void ShowGameInfo(string currentCard, int currentPlayer, int nextPlayer, int[] numOfCardsInPlayersHands)
{
    Console.WriteLine();
    for(int i = 0; i < numOfCardsInPlayersHands!.Length; i++)
        Console.WriteLine($"Player {i} has {numOfCardsInPlayersHands[i]} cards.");
    Console.WriteLine($"Current player: {currentPlayer}");
    Console.WriteLine($"Next player: {nextPlayer}");
    Console.WriteLine($"Target card {currentCard}");
}

void ShowOptions(string[] options)
{
    foreach (var option in options)
        Console.WriteLine(option);
}