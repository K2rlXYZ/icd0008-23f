namespace Domain;
public class UnoCard
{
    public EUnoCards.UnoCardsValues Value { get; set; }
    public EUnoCards.UnoCardsColors Color { get; set; }

    public override string ToString()
    {
        var color = CardColorToString();
        var value = CardValueToString();
        if (Value is EUnoCards.UnoCardsValues.ValuePlus4 or EUnoCards.UnoCardsValues.ValuePickColor) return value;
        return  $"{color} {value}";
    }

    private string CardColorToString()
    {
        switch (Color)
        {
            case EUnoCards.UnoCardsColors.Yellow:
                Console.ForegroundColor = ConsoleColor.Yellow;
                return "Yellow";
            case EUnoCards.UnoCardsColors.Green:
                Console.ForegroundColor = ConsoleColor.Green;
                return "Green";
            case EUnoCards.UnoCardsColors.Red:
                Console.ForegroundColor = ConsoleColor.Red;
                return "Red";
            case EUnoCards.UnoCardsColors.Blue:
                Console.ForegroundColor = ConsoleColor.Blue;
                return "Blue";
            default:
                throw new ApplicationException("No such card color" + Color);
        }
    }

    private string CardValueToString()
    {
        switch (Value)
        {
            case EUnoCards.UnoCardsValues.Value0:
                return "0";
            case EUnoCards.UnoCardsValues.Value1:
                return "1";
            case EUnoCards.UnoCardsValues.Value2:
                return "2";
            case EUnoCards.UnoCardsValues.Value3:
                return "3";
            case EUnoCards.UnoCardsValues.Value4:
                return "4";
            case EUnoCards.UnoCardsValues.Value5:
                return "5";
            case EUnoCards.UnoCardsValues.Value6:
                return "6";
            case EUnoCards.UnoCardsValues.Value7:
                return "7";
            case EUnoCards.UnoCardsValues.Value8:
                return "8";
            case EUnoCards.UnoCardsValues.Value9:
                return "9";
            case EUnoCards.UnoCardsValues.ValueBlock:
                return "Skip";
            case EUnoCards.UnoCardsValues.ValuePlus2:
                return "+2";
            case EUnoCards.UnoCardsValues.ValueReverse:
                return "Reverse";
            case EUnoCards.UnoCardsValues.ValuePlus4:
                return "+4";
            case EUnoCards.UnoCardsValues.ValuePickColor:
                return "Color Card";
            default:
                throw new ApplicationException("No such card value" + Value);
        }
    }
}