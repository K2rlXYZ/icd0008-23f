namespace Domain;

public class GameState
{
    public Guid Id { get; set; }
    public List<UnoCard> DrawDeck { get; set; } = new();
    public List<UnoCard> PlayDeck { get; set; } = new();
    public List<Player> Players { get; set; } = new();
    public int SelectedPlayerIndex { get; set; }
    public int Direction = 1;
}