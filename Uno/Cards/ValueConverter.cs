namespace Uno.Cards;

public static class ValueConverter
{
    private static readonly Dictionary<Value, string> ValueToStr;
    private static readonly Value[] Values =
    {
        Value.V1, Value.V2, Value.V3, Value.V4, Value.V5, Value.V6, Value.V7, Value.V8, Value.V9,
        Value.Skip, Value.Reverse, Value.DrawTwo, Value.V0, Value.Wild, Value.WildDrawFour
    };
    
    static ValueConverter()
    {
        ValueToStr = new();
        ValueToStr[Value.V0] = "0";
        ValueToStr[Value.V1] = "1";
        ValueToStr[Value.V2] = "2";
        ValueToStr[Value.V3] = "3";
        ValueToStr[Value.V4] = "4";
        ValueToStr[Value.V5] = "5";
        ValueToStr[Value.V6] = "6";
        ValueToStr[Value.V7] = "7";
        ValueToStr[Value.V8] = "8";
        ValueToStr[Value.V9] = "9";
        ValueToStr[Value.Reverse] = "reverse";
        ValueToStr[Value.Skip] = "skip";
        ValueToStr[Value.Wild] = "wild";
        ValueToStr[Value.DrawTwo] = "draw 2";
        ValueToStr[Value.WildDrawFour] = "wild draw 4";
    }

    public static Value[] GetValues()
        => Values;
    
    public static string Convert(Value value)
        => ValueToStr[value];
}