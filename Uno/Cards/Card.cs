namespace Uno.Cards;

public class Card
{
    private Color _color;
    private Value _value;

    public Card(Color color, Value value)
    {
        _value = value;
        _color = IsWildCard(value)? Color.MultiColor: color;
    }

    private bool IsWildCard(Value value)
        => value == Value.Wild || value == Value.WildDrawFour;
    
    public void SetColor(int colorId)
        => _color = ColorConverter.Convert(colorId);

    public bool IsMulticolor()
        => _color == Color.MultiColor;

    public bool Is(Value value)
        => _value == value;
    
    public bool DoTheyHaveTheSameColorOrValue(Card other)
        => other._color == _color || other._value == _value;

    public Card Clone()
        => new (_color, _value);
    
    public override string ToString()
    {
        string color = ColorConverter.Convert(_color);
        string value = ValueConverter.Convert(_value);
        return $"{color}/{value}";
    }
}