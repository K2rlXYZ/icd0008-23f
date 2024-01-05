using Domain.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Games
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public DbGame DbGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var dbgame = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);

            if (dbgame == null)
            {
                return NotFound();
            }
            else 
            {
                DbGame = dbgame;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }
            var dbgame = await _context.Games.FindAsync(id);

            if (dbgame != null)
            {
                DbGame = dbgame;
                _context.Games.Remove(DbGame);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}
