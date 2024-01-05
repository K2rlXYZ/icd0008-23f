using System.Text.Json;
using Domain;
using Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GameRepositoryEF : IGameRepository
{
    private static string connectionString = "DataSource=<%temppath>Uno\\app.db".Replace("<%temppath>", Path.GetTempPath());
    private static readonly DbContextOptions<AppDbContext> ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(connectionString)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .Options;


    private readonly AppDbContext _ctx = new(ContextOptions);

    /*public GameRepositoryEF()
    {
        _ctx.Database.Migrate();
    }*/

    public void SaveGame(Guid? id, GameState gameState)
    {
        var game = _ctx.Games.FirstOrDefault(g => g.Id == gameState.Id);
        if (game == null)
        {
            game = new DbGame
            {
                Id = gameState.Id,
                State = JsonSerializer.Serialize(game),
                Players = gameState.Players.Select(p => new DbPlayer()
                {
                    Id = p.Id,
                    Nickname = p.Nickname,
                    PlayerType = p.Type
                }).ToList(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            game.State = JsonSerializer.Serialize(gameState);
            _ctx.Games.Add(game);
        }
        else
        {
            game.UpdatedAt = DateTime.Now;
            game.State = JsonSerializer.Serialize(gameState);
        }

        var changeCount = _ctx.SaveChanges();
        //Console.WriteLine("SaveChanges: " + changeCount);
    }

    public GameState LoadGameState(Guid id)
    {
        var game = _ctx.Games.First(g => g.Id == id);
        return JsonSerializer.Deserialize<GameState>(game.State ??
                                                     throw new InvalidOperationException(
                                                         "No game in database with id " + id)) ??
               throw new InvalidOperationException("Couldn't deserialize state (" + game.State + ") of game with id " +
                                                   id);
    }

    public List<(Guid ID, DateTime LastEditedAt)> GetAllSaves()
    {
        return _ctx.Games
            .OrderByDescending(g => g.UpdatedAt)
            .ToList()
            .Select(g => (g.Id, g.UpdatedAt))
            .ToList();
    }
}