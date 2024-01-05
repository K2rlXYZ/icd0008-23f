using Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<DbGame> Games { get; set; } = default!;
    public DbSet<DbPlayer> Players { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}