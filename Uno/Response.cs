namespace Uno;

public class Response
{
    // Los valores son nulos cuando el request no pidió esos valores o cuando la request no fue exitosa
    public bool WasRequestSuccessful = true; // verdadero cuando el request fue exitoso. 
    public string? ErrorMessage = null; // una descripción del error que ocurrió cuando el request falla.
    public GameInfo? GameInfo = null; // contiene mucha información sobre el juego actual
    public string[]? Options = null; // arreglo con las opciones que el jugador actual puede elegir.
}