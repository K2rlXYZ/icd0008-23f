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
    public class IndexModel : PageModel
    {
        private readonly WebApp01.Data.ApplicationDbContext _context;

        public IndexModel(WebApp01.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Person> Person { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.People != null)
            {
                Person = await _context.People.ToListAsync();
            }
        }
    }
}
