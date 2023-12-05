namespace Uno;

public class GameInfo
{
    public bool IsGameOver { get; }
    public string CurrentCard { get; }
    public int CurrentPlayer { get; }
    public int NextPlayer { get; }
    public int[] NumOfCardsInHands { get; }

    public GameInfo(bool isGameOver, string currentCard, int currentPlayer, int nextPlayer, int[] numOfCardsInHands)
    {
        IsGameOver = isGameOver;
        CurrentCard = currentCard;
        CurrentPlayer = currentPlayer;
        NextPlayer = nextPlayer;
        NumOfCardsInHands = numOfCardsInHands;
    }
}