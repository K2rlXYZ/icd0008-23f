using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnoEngine;

namespace WebApp.Pages.Game;

public class ConfigureNicknames : PageModel
{
    
    private readonly DAL.AppDbContext _context;
    private readonly IGameRepository _gameRepository;
    public GameEngine Engine { get; set; } = default!;
    
    public ConfigureNicknames(AppDbContext context)
    {
        _context = context;
        _gameRepository = new GameRepositoryEF();
        Nicknames = new List<string>();
    }
    
    [BindProperty(SupportsGet = true)]
    public Guid GameId { get; set; }
    public void OnGet()
    {
        Engine = new GameEngine(_gameRepository);
        Engine.LoadGame(GameId);
        foreach (var player in Engine.State.Players)
        {
            if (player.Nickname != null) Nicknames.Add(player.Nickname);
        }
    }
    [BindProperty]
    public int PlayAsPlayerIndex { get; set; }

    [BindProperty]
    public List<string> Nicknames { get; set; }

    public async Task<IActionResult> OnPost()
    {
        Engine = new GameEngine(_gameRepository);
        Engine.LoadGame(GameId);

        for (int i = 0; i < Engine.State.Players.Count; i++)
        {
            Engine.State.Players[i].Nickname = Nicknames[i];
        }
            
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        Engine.SaveGame(Engine.State.Id);
        
        var playAsPlayerId = Engine.State.Players[PlayAsPlayerIndex].Id;
            
        return RedirectToPage("./Index", routeValues:new
        {
            GameId=Engine.State.Id,
            PlayerId=playAsPlayerId
        });
    }
}