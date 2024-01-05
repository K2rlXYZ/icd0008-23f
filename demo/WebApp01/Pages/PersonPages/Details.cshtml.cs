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
    public class DetailsModel : PageModel
    {
        private readonly WebApp01.Data.ApplicationDbContext _context;

        public DetailsModel(WebApp01.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
