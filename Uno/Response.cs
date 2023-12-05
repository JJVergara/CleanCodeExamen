namespace Uno;

public class Response
{
    public bool WasRequestSuccessful = true;
    public string ErrorMessage = "None";
    public GameInfo GameInfo = new GameInfo(true, "None", 0, 0, new int[0]);
    public string[]? Options = null;
}