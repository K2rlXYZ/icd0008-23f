using System.Text.Json;
using Domain;

namespace DAL;

public class GameRepositoryFileSystem : IGameRepository
{
    private static readonly string FilePrefix = AppDomain.CurrentDomain.BaseDirectory+"Saves\\";

    public void SaveGame(Guid? id, GameState game)
    {
        if (!Directory.Exists(FilePrefix))
        {
            Directory.CreateDirectory(FilePrefix);
        }
        var fileName = id.ToString();
        File.WriteAllText(FilePrefix+fileName + ".json", JsonSerializer.Serialize(game));
    }

    public GameState? LoadGameState(Guid id)
    {
        return JsonSerializer.Deserialize<GameState>(File.ReadAllText(FilePrefix + id + ".json") ??
                                                     throw new ApplicationException(
                                                         "Couldn't load state with id: " + id));
    }

    public List<(Guid ID, DateTime LastEditedAt)> GetAllSaves()
    {
        if(Directory.Exists(FilePrefix)){
            var saveNames = new List<(Guid ID, DateTime LastEditedAt)>();
            foreach (var saveFileName in Directory.EnumerateFiles(FilePrefix))
            {
                saveNames.Add((
                    Guid.Parse(Path.GetFileNameWithoutExtension(saveFileName)),
                    Directory.GetLastWriteTime(saveFileName)));
            }

            return saveNames.OrderBy(g => g.LastEditedAt).ToList();
            
        }

        return new List<(Guid ID, DateTime LastEditedAt)>();
    }
}