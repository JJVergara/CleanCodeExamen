using Uno.Cards;
using Uno.DeckUtils;

namespace Uno;

public class GameState
{
    public Deck DiscardPile = new();
    public Deck DrawPile = new();
    public List<Player> Players = new List<Player>();
    public Card CurrentTarget;
    public int Direction = 1;
    public int CurrentPlayerId = 0;
    public int PlayerWhoSelectsNextColor = 0;
    public int NumOfPlayers => Players.Count;
    public Player CurrentPlayer => Players[CurrentPlayerId];
}