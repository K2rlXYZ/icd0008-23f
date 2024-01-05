using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp01.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<PersonPet> PersonPets { get; set; }
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
}