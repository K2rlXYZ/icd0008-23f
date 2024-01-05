using System.ComponentModel.DataAnnotations;

namespace Domain.Database;

public class DbPlayer : BaseEntity
{
    [MaxLength(128)] public string? Nickname { get; set; }
    
    public EPlayerType PlayerType { get; set; }
    public bool Drew { get; set; }
    public Guid GameId { get; set; }
    public DbGame? Game { get; set; }
}