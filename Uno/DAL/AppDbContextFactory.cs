using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL;

// Migration:
// dotnet ef migrations add --project DAL --startup-project Uno InitialCreate


public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private static string connectionString = "DataSource=<%temppath>Uno\\app.db".Replace("<%temppath>", Path.GetTempPath());
    public AppDbContext CreateDbContext(string[] args)
    {
        
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}