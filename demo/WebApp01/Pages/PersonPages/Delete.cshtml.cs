using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp01.Data;

namespace WebApp01._Pages_PersonPages
{
    public class DeleteModel : PageModel
    {
        private readonly WebApp01.Data.ApplicationDbContext _context;

        public DeleteModel(WebApp01.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Person Person { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People.FirstOrDefaultAsync(m => m.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            else 
            {
                Person = person;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }
            var person = await _context.People.FindAsync(id);

            if (person != null)
            {
                Person = person;
                _context.People.Remove(Person);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
