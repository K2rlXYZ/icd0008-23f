namespace Domain.Database;

public class DbGame : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string State { get; set; } = default!;

    public ICollection<DbPlayer>? Players { get; set; }
}