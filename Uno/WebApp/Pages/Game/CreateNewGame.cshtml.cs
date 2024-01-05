using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain.Database;
using UnoEngine;

namespace WebApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private readonly IGameRepository _gameRepository;

        public GameEngine Engine = default!;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
            _gameRepository = new GameRepositoryEF();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty, 
         Range(0, 10)]
        public int PlayerCount { get; set; }
        
        [BindProperty, 
         Range(0,3)]
        public int PlayerTypes { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPost()
        {
            Engine = new GameEngine(_gameRepository);
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
          
            Engine.SetPlayerCount(PlayerCount);

            Console.WriteLine(PlayerTypes);
            switch (PlayerTypes)
            {
              case 0: 
                  Engine.SetAllButOnePlayerAI();
                  break;
              case 1: 
                  Engine.SetAllButOnePlayerHuman();
                  break;
              case 2:
                  Engine.SetAllPlayersHuman();
                  break;
              case 3:
                  Engine.SetAllPlayersAI();
                  break;
              default:
                  return Page();
            }
            
            Engine.SaveGame(Engine.State.Id);
            return RedirectToPage("./ConfigureNicknames", routeValues: new
            { 
                GameId=Engine.State.Id
            });
        }
    }
}
