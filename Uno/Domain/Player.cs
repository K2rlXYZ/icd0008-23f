namespace Domain;

public class Player
{
    public bool Drew { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Nickname { get; set; }
    public EPlayerType Type { get; set; }

    public List<UnoCard> CardsHand { get; set; } = new();
}