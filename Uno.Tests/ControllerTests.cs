using System.Collections.Generic;
using Xunit;

namespace Uno.Tests;

public class ControllerTests
{
    [Theory]
    [InlineData(2, new []{2,3,4,12})]
    [InlineData(10, new []{7})]
    public void DoTheThing_CreateValidGameThatDoesntExist(int numOfPlayers, int[] shuffles)
    {
        Controller controller = new Controller();
        string gameName = "My game";
        
        Response response =
            controller.DoTheThing("New game", gameName, numOfPlayers, shuffles, 0, 0);

        Assert.Null(response.ErrorMessage);
        Assert.True(response.WasRequestSuccessful);
        Assert.Null(response.GameInfo);
        Assert.Null(response.Options);
    }
    
    [Fact]
    public void DoTheThing_CreateValidGameThatAlreadyExists()
    {
        Controller controller = new Controller();
        string gameName = "My game";
        controller.DoTheThing("New game", gameName, 4, new[] { 7, 5, 7 }, 0, 0);
        
        Response response =
            controller.DoTheThing("New game", gameName, 2, new[] { 7 }, 0, 0);

        Assert.Equal("This game already exists.", response.ErrorMessage);
        Assert.False(response.WasRequestSuccessful);
        Assert.Null(response.GameInfo);
        Assert.Null(response.Options);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(11)]
    public void DoTheThing_CreateGameWithInvalidNumOfPlayers(int numOfPlayers)
    {
        Controller controller = new Controller();
        string gameName = "My game";
        
        Response response =
            controller.DoTheThing("New game", gameName, numOfPlayers, new[] { 5, 12 }, 0, 0);

        Assert.Equal("The number of players is invalid.", response.ErrorMessage);
        Assert.False(response.WasRequestSuccessful);
        Assert.Null(response.GameInfo);
        Assert.Null(response.Options);
    }
    
    [Theory]
    [InlineData(new int[]{ })]
    [InlineData(new int[]{ 2, 1})]
    [InlineData(new int[]{ 4, 3, 3, 13, 3})]
    public void DoTheThing_CreateGameWithInvalidNumOfShuffles(int[] shuffles)
    {
        Controller controller = new Controller();
        string gameName = "My game";
        
        Response response =
            controller.DoTheThing("New game", gameName, 3, shuffles, 0, 0);

        Assert.Equal("Invalid shuffle.", response.ErrorMessage);
        Assert.False(response.WasRequestSuccessful);
        Assert.Null(response.GameInfo);
        Assert.Null(response.Options);
    }

    [Theory]
    [InlineData("Get game info")]
    [InlineData("Show options")]
    [InlineData("Play")]
    public void DoTheThing_GetSomethingFromGameThatDoesntExist(string command)
    {
        Controller controller = new Controller();
        string gameName = "My game";
        
        Response response =
            controller.DoTheThing(command, gameName, 2, new[] { 7 }, 0, 0);

        Assert.Equal("This game does not exist.", response.ErrorMessage);
        Assert.False(response.WasRequestSuccessful);
        Assert.Null(response.GameInfo);
        Assert.Null(response.Options);
    }

    [Theory]
    [InlineData(2, new[] { 2, 3, 4, 12 }, "yellow/skip", 1, 0, new[]{7,7})]
    [InlineData(10, new[] { 7 }, "yellow/4", 0, 1, new []{7,7,7,7,7,7,7,7,7,7})]
    [InlineData(3, new[] { 10 }, "green/reverse", 0, 2, new[]{7,7,7})]
    [InlineData(5, new[] { 7 }, "red/draw 2", 1, 2, new[]{9,7,7,7,7})]
    [InlineData(4, new[] { 6, 5 }, "multicolor/wild", 0, 0, new[]{7,7,7,7})]
    public void DoTheThing_GetInfoFromNewGame(int numOfPlayers, int[] shuffles, string expCard, int expPlayer,
        int expNextPlayer, int[] expCardsInHands)
    {
        Controller controller = new Controller();
        string gameName = "My game";
        controller.DoTheThing("New game", gameName, numOfPlayers, shuffles, 0, 0);
        
        Response response =
            controller.DoTheThing("Get game info", gameName, numOfPlayers, shuffles, 0, 0);

        Assert.True(response.WasRequestSuccessful);
        Assert.Null(response.ErrorMessage);
        Assert.False(response.GameInfo!.IsGameOver);
        Assert.Equal(expCard, response.GameInfo!.CurrentCard);
        Assert.Equal(expPlayer, (int)response.GameInfo!.CurrentPlayer);
        Assert.Equal(expNextPlayer, (int)response.GameInfo!.NextPlayer);
        Assert.Equal(expCardsInHands, response.GameInfo!.NumOfCardsInHands);
        Assert.Null(response.Options);
        
    }

    [Theory]
    [InlineData("Play", 4, new[] { 6, 5 }, new int[] {  }, new int[] {  }, 1)]
    [InlineData("Show options", 4, new[] { 6, 5 }, new int[] {  }, new int[] {  }, 1)]
    [InlineData("Show options", 3, new[] { 7 }, new[] { 3 }, new[] { 0 }, 0)]
    [InlineData("Play", 3, new[] { 7 }, new[] { 3 }, new[] { 0 }, 0)]
    [InlineData("Show options", 4, new[] { 6, 5 }, new[] { 3 }, new[] { 0 }, 1)]
    [InlineData("Play", 4, new[] { 6, 5 }, new[] { 3 }, new[] { 0 }, 1)]
    [InlineData("Show options", 3, new[] { 3 }, new[] { 4,5 }, new[] { 0,1 }, 2)]
    [InlineData("Play", 3, new[] { 3 }, new[] { 4,5 }, new[] { 0,1 }, 2)]
    [InlineData("Play", 3, new[] { 3 }, new[] { 4,5,4 }, new[] { 0,1,0 }, 0)]
    [InlineData("Show options", 3, new[] { 3 }, new[] { 4,5,4 }, new[] { 0,1,0 }, 2)]
    public void DoTheThing_TryToGetInfoOrPlayForAnotherPlayer(string command, int numOfPlayers, int[] shuffles,
        int[] plays, int[] currentPlayer, int invalidPlayer)
    {
        Controller controller = new Controller();
        string gameName = "My game";
        controller.DoTheThing("New game", gameName, numOfPlayers, shuffles, 0, 0);
        for(int i = 0; i < plays.Length; i++)
            controller.DoTheThing("Play", gameName, numOfPlayers, shuffles, currentPlayer[i], plays[i]);
        
        Response response =
            controller.DoTheThing(command, gameName, numOfPlayers, shuffles, invalidPlayer, 0);

        Assert.Equal("You are not the current player.", response.ErrorMessage);
        Assert.False(response.WasRequestSuccessful);
        Assert.Null(response.GameInfo);
        Assert.Null(response.Options);
    }

    [Theory]
    [InlineData(10, new[] { 2, 3, 5, 7 }, new int[] { 0, 1, 2, 3 }, new []{"Which card do you want to play? (enter -1 to pass)", "0- green/draw 2", "1- blue/1", "2- green/0", "3- yellow/3", "4- multicolor/wild draw 4", "5- yellow/7", "6- red/reverse"})]
    [InlineData(10, new[] { 2, 3, 5, 7 }, new int[] { 0, 1, 2, 3, 4 }, new []{"Select a color: 0- Red, 1- Blue, 2- Yellow, 3- Green."})]
    [InlineData(10, new[] { 3, 5, 7 }, new int[] { -1, 4, 0, -1, 3, 1, -1, -1, 2, 3, -1, -1, 0, -1, 7, -1, -1, 9, -1, 7, -1, -1, -1, -1, -1, 0, -1, 1, 0, 0, 10 }, new []{"Select a color: 0- Red, 1- Blue, 2- Yellow, 3- Green."})]
    [InlineData(2, new[] { 3, 5, 7 }, new int[] { 6, 6, 4, 4, 2, 1, 0, 2, 0, 1, 1, 2, -1, 1, -1, 1, -1, 1, 1 }, new []{"Which card do you want to play? (enter -1 to pass)", "0- red/1"})]
    public void DoTheThing_ShowOptionsAfterSomePlays(int numOfPlayers, int[] shuffles, int[] plays, string[] expOptions)
    {
        Controller controller = new Controller();
        string gameName = "My game";
        controller.DoTheThing("New game", gameName, numOfPlayers, shuffles, 0, 0);
        int currentPlayer =
            (int)controller.DoTheThing("Get game info", gameName, numOfPlayers, shuffles, 0, 0).GameInfo!.CurrentPlayer;
        foreach (var play in plays)
        {
            controller.DoTheThing("Play", gameName, numOfPlayers, shuffles, currentPlayer, play);
            currentPlayer =
                (int)controller.DoTheThing("Get game info", gameName, numOfPlayers, shuffles, currentPlayer, play)
                    .GameInfo!.CurrentPlayer;
        }

        Response response =
            controller.DoTheThing("Show options", gameName, numOfPlayers, shuffles, currentPlayer, 0);

        Assert.Null(response.ErrorMessage);
        Assert.True(response.WasRequestSuccessful);
        Assert.Null(response.GameInfo);
        Assert.Equal(expOptions, response.Options);
    }
    
    [Theory]
    [InlineData(10, new[] { 2, 3, 5, 7 }, new int[] { 0, 1, 2, 3, 4 }, 4, "Your color choice is invalid.")]
    [InlineData(10, new[] { 2, 3, 5, 7 }, new int[] { 0, 1, 2, 3, 4 }, -1, "Your color choice is invalid.")]
    [InlineData(5, new[] { 5, 7 }, new int[] { }, 4, "You cannot play that card.")]
    [InlineData(5, new[] { 5, 7 }, new int[] { }, -2, "Your card choice is invalid.")]
    [InlineData(5, new[] { 5, 7 }, new int[] { }, 8, "Your card choice is invalid.")]
    [InlineData(5, new[] { 5, 7 }, new int[] { }, 5, "You cannot play that card.")]
    [InlineData(10, new[] { 3, 5, 7 }, new [] {5, 5, 5, 5, 3, 2, -1, -1, 0, 0, 1, 0, 0, 5, 5, -1, 4, 6, 2, 3, 2, 1, 2, 3, 5, 8, -1, -1, -1, 0, -1, -1, 1, 1, 5, 5, 5, -1, 2, 2, 1, 5, 4, 0, 1, 3, 0, 6, 5, 1, 1, 4, 3, -1, -1, -1, 0}, 0, "Someone already won this game.")]
    public void DoTheThing_PlayInvalidCards(int numOfPlayers, int[] shuffles, int[] plays, int lastPlay, string errorMessage)
    {
        Controller controller = new Controller();
        string gameName = "My game";
        controller.DoTheThing("New game", gameName, numOfPlayers, shuffles, 0, 0);
        int currentPlayer =
            (int)controller.DoTheThing("Get game info", gameName, numOfPlayers, shuffles, 0, 0).GameInfo!.CurrentPlayer;
        foreach (var play in plays)
        {
            controller.DoTheThing("Play", gameName, numOfPlayers, shuffles, currentPlayer, play);
            currentPlayer =
                (int)controller.DoTheThing("Get game info", gameName, numOfPlayers, shuffles, currentPlayer, play)
                    .GameInfo!.CurrentPlayer;
        }

        Response response =
            controller.DoTheThing("Play", gameName, numOfPlayers, shuffles, currentPlayer, lastPlay);

        Assert.Equal(errorMessage, response.ErrorMessage);
        Assert.False(response.WasRequestSuccessful);
        Assert.Null(response.GameInfo);
        Assert.Null(response.Options);
    }
    
    [Fact]
    public void DoTheThing_GetInfoFromCurrentGame()
    {
        int numOfPlayers = 10;
        int[] shuffles = { 3, 5, 7 };
        int[] plays =
        {
            5, 5, 5, 5, 3, 2, -1, -1, 0, 0, 1, 0, 0, 5, 5, -1, 4, 6, 2, 3, 2, 1, 2, 3, 5, 8, -1, -1, -1, 0, -1, -1, 1,
            1, 5, 5, 5, -1, 2, 2, 1, 5, 4, 0, 1, 3, 0, 6, 5, 1, 1, 4, 3, -1, -1, -1, 0
        };
        Controller controller = new Controller();
        string gameName = "My game";
        controller.DoTheThing("New game", gameName, numOfPlayers, shuffles, 0, 0);
        int currentPlayer =
            (int)controller.DoTheThing("Get game info", gameName, numOfPlayers, shuffles, 0, 0).GameInfo!.CurrentPlayer;
        foreach (var play in plays)
        {
            controller.DoTheThing("Play", gameName, numOfPlayers, shuffles, currentPlayer, play);
            currentPlayer =
                (int)controller.DoTheThing("Get game info", gameName, numOfPlayers, shuffles, currentPlayer, play)
                    .GameInfo!.CurrentPlayer;
        }

        Response response =
            controller.DoTheThing("Get game info", gameName, numOfPlayers, shuffles, currentPlayer, 0);

        Assert.True(response.WasRequestSuccessful);
        Assert.Null(response.ErrorMessage);
        Assert.True(response.GameInfo!.IsGameOver);
        Assert.Equal("green/2", response.GameInfo!.CurrentCard);
        Assert.Equal(2, (int)response.GameInfo!.CurrentPlayer);
        Assert.Equal(3, (int)response.GameInfo!.NextPlayer);
        Assert.Equal(new int[] { 6, 0, 11, 6, 5, 8, 5, 8, 4, 5 }, response.GameInfo!.NumOfCardsInHands);
        Assert.Null(response.Options);
    }

    [Fact]
    public void DoTheThing_PlayThreeGamesSimultaneously()
    {
        string[] gameNames = { "game1", "game2", "game3" };
        int[] numPlayers = { 3, 5, 4 };
        int[][] shuffles = {new[] { 10 }, new[] { 7 }, new[] { 6, 5 }};
        int[][] plays = {
            new[] { 3, 6, 3, 2, 1, 4, 1, 1, 2, 11, 1 },
            new[] { 2, 4, 8, 3, 4, -1, 6, 2, 3, 6, -1 },
            new[] { 2, 6, 2, 1, 5, 2, 0, -1, 4, 0, -1 }
        };
        string[] expCurrentCard = { "blue/wild", "blue/9", "red/wild"};
        int[] expCurrentPlayer = { 0, 4, 2 };
        int[] expNextPlayer = { 2, 0, 3 };
        int[][] expCardsInHand = { new[] { 4, 11, 3 }, new[] { 7, 4, 7, 8, 8 }, new[] { 4, 6, 5, 8 } };
        Controller controller = new Controller();
        int[] currentPlayer = new int[3];
        for (int i = 0; i < 3; i++)
        {
            controller.DoTheThing("New game", gameNames[i], numPlayers[i], shuffles[i], 0, 0);
            currentPlayer[i] =
                (int)controller.DoTheThing("Get game info", gameNames[i], numPlayers[i], shuffles[i], 0, 0).GameInfo!
                    .CurrentPlayer;
        }
        for (int j = 0; j < 11; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                controller.DoTheThing("Play", gameNames[i], numPlayers[i], shuffles[i], currentPlayer[i], plays[i][j]);
                currentPlayer[i] = (int)controller.DoTheThing("Get game info", gameNames[i], numPlayers[i], shuffles[i],
                    currentPlayer[i], plays[i][j]).GameInfo!.CurrentPlayer;
            }
        }

        for(int i = 0; i < 3; i++)
        {
            Response response =
                controller.DoTheThing("Get game info", gameNames[i], numPlayers[i], shuffles[i], currentPlayer[i], 0);
        
            Assert.True(response.WasRequestSuccessful);
            Assert.Null(response.ErrorMessage);
            Assert.False(response.GameInfo!.IsGameOver);
            Assert.Equal(expCurrentCard[i], response.GameInfo!.CurrentCard);
            Assert.Equal(expCurrentPlayer[i], (int)response.GameInfo!.CurrentPlayer);
            Assert.Equal(expNextPlayer[i], (int)response.GameInfo!.NextPlayer);
            Assert.Equal(expCardsInHand[i],response.GameInfo!.NumOfCardsInHands);
            Assert.Null(response.Options);
        }
    }
}