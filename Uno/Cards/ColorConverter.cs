namespace Uno.Cards;

public static class ColorConverter
{
    private static readonly Dictionary<Color, string> ColorToStr;
    private static readonly Color[] Colors = {Color.Red, Color.Blue, Color.Yellow, Color.Green};
    
    static ColorConverter()
    {
        ColorToStr = new();
        ColorToStr[Color.Blue] = "blue";
        ColorToStr[Color.Red] = "red";
        ColorToStr[Color.Green] = "green";
        ColorToStr[Color.Yellow] = "yellow";
        ColorToStr[Color.MultiColor] = "multicolor";
    }

    public static Color[] GetColors()
        => Colors;

    public static string Convert(Color color)
        => ColorToStr[color];

    public static Color Convert(int colorId)
        => Colors[colorId];
}