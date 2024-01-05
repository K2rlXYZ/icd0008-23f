using DAL;
using Domain.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Test : PageModel
{
    private AppDbContext _db;

    public Test(AppDbContext db)
    {
        _db = db;
    }

    public List<DbGame> Games { get; set; } = default!;

    public void OnGet()
    {
        _db.Games.Add(new DbGame()
        {
            CreatedAt = DateTime.Now,
            Id = Guid.Empty,
            Players = new List<DbPlayer>(),
            State = "",
            UpdatedAt = DateTime.Now
        });
        _db.SaveChanges();
        Games = _db.Games.ToList();
        
    }
}