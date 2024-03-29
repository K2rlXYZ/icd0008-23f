﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Database;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<DbGame> DbGame { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Games != null)
            {
                DbGame = await _context.Games.Include(g => g.Players).OrderByDescending(g => g.UpdatedAt).ToListAsync();
            }
        }
    }
}
