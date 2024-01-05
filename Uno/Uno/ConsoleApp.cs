using DAL;
using Domain;
using MenuSystem;
using UnoEngine;

IGameRepository gameRepository = new GameRepositoryEF();
//var gameRepository = new GameRepositoryFileSystem();
var game = new GameEngine(gameRepository);

var menuLevels = new List<string> { "mainMenu" };

var colorMenu = new Menu("Pick a color for the top card: ", new List<MenuItem>()
{
    new MenuItem()
    {
        PromptText = "l",
        MenuLabel = () => "Blue",
        MethodToRun = () => "Blue"
    },
    new MenuItem()
    {
        PromptText = "r",
        MenuLabel = () => "Red",
        MethodToRun = () => "Red"
    },
    new MenuItem()
    {
        PromptText = "g",
        MenuLabel = () => "Green",
        MethodToRun = () => "Green"
    },
    new MenuItem()
    {
        PromptText = "y",
        MenuLabel = () => "Yellow",
        MethodToRun = () => "Yellow"
    }
}, menuLevels);
colorMenu.RemoveBackAndExitKeys();

string? SetPlayerCount()
{
    Console.Clear();
    Console.Write("Player count: ");
    var countStr = Console.ReadLine()?.Trim();
    try
    {
        if (countStr != null)
        {
            var count = int.Parse(countStr);
            game.SetPlayerCount(count);
        }
    }
    catch (FormatException)
    {
        Console.WriteLine($"Input ({countStr}) was not an integer number");
        return "b";
    }
    return "b";
}

string? SetNickNames()
{
    Console.Clear();
    var menuItems = new List<MenuItem>();
    int i = 1;
    foreach (var player in game.State.Players)
    {
        menuItems.Add(new MenuItem()
        {
            PromptText = i.ToString(),
            MenuLabel = () => $"Change nickname of {player.Nickname} ({player.Type})",
            MethodToRun = () =>
            {
                Console.Write($"Set nickname of {player.Nickname} ({player.Type}) to: ");
                var newNickname = Console.ReadLine()?.Trim();
                player.Nickname = newNickname;
                return null;
            }
        });
        i++;
    }

    var nicknamesMenu = new Menu("Nicknames: ", menuItems, menuLevels);
    return nicknamesMenu.RunOnce();
}

string? GameLoop()
{
    var userChoice = "";
    do
    {
        // Also checks wether or not the player has to draw
        var drew = game.DrawUntilPlayableCard();
        var player = game.State.Players[game.State.SelectedPlayerIndex];
        if (player.Type == EPlayerType.Human)
        {
            Console.WriteLine("Last card: " + game.State.PlayDeck.Last());
            Console.ForegroundColor = ConsoleColor.White;
            var cardsMenuItems = new List<MenuItem>();
            for (int cardIndex = 0; cardIndex < player.CardsHand.Count; cardIndex++)
            {
                var thisPlayerIndex = game.State.SelectedPlayerIndex;
                var thisCardIndex = cardIndex;
                cardsMenuItems.Add(new MenuItem()
                {
                    PromptText = (cardIndex + 1).ToString(),
                    MenuLabel = player.CardsHand[cardIndex].ToString,
                    MethodToRun = () => game.HumanPlayCard(thisPlayerIndex, thisCardIndex)
                });
            }

            if (drew)
            {
                cardsMenuItems.Add(new MenuItem()
                {
                    PromptText = "k",
                    MenuLabel = () => "Skip turn",
                    MethodToRun = () =>
                    {
                        Console.WriteLine(game.State.Players[game.State.SelectedPlayerIndex].Nickname +
                                          " skipped their turn");
                        game.IncreasePlayerIndex();
                        return null;
                    }
                });
            }

            var gameLoopMenu = new Menu(player.Nickname + ", play a card from your deck", cardsMenuItems, menuLevels);
            // Replaces back key with return to main menu key
            gameLoopMenu.RemoveBackKey();
            gameLoopMenu.AddMenuItem(new MenuItem()
            {
                PromptText = "b",
                MenuLabel = () => "Back to main menu",
                MethodToRun = () => menuLevels[0]
            });
            gameLoopMenu.AddMenuItem(new MenuItem()
            {
                PromptText = "s",
                MenuLabel = () => "Save game and exit to menu",
                MethodToRun = () =>
                {
                    gameRepository.SaveGame(game.State.Id, game.State);
                    return menuLevels[0];
                }
            });
            
            userChoice = gameLoopMenu.RunOnce();

            if (userChoice == "askForColor")
            {
                Console.Clear();
                string? color;
                do
                {
                    color = colorMenu.RunOnce();
                } while (!new List<string> { "l", "r", "g", "y" }.Contains(color!));

                game.SetTopCardColor(color);
                Console.Clear();
                Console.WriteLine(game.State.Players[game.GetLastPlayerIndex(1)].Nickname + " PLAYED: " +
                                  game.State.PlayDeck.Last());
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (userChoice is null or "won")
            {
                return menuLevels[0];
            }
        }
        else if (player.Type == EPlayerType.AI)
        {
            userChoice = game.AIPlayRandomCard();
            if (userChoice == "askForColor")
            {
                Console.WriteLine(game.State.Players[game.GetLastPlayerIndex(2)].Nickname + " PLAYED: " +
                                  game.State.PlayDeck.Last());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    } while (userChoice != "x" && !menuLevels.Contains(userChoice!));
    Console.Clear();
    return userChoice;
}

var playerTypesMenu = new Menu("Set player types", new List<MenuItem>
{
    new()
    {
        PromptText = "h",
        MenuLabel = () => "All human",
        MethodToRun = () =>
        {
            game.SetAllPlayersHuman(); 
            return "b";
        }
    },
    new()
    {
        PromptText = "a",
        MenuLabel = () => "All AI",
        MethodToRun = () =>
        {
            game.SetAllPlayersAI(); 
            return "b";
        }
    },
    new()
    {
        PromptText = "1",
        MenuLabel = () => "1 human",
        MethodToRun = () =>
        {
            game.SetAllButOnePlayerAI(); 
            return "b";
        }
    },
    new()
    {
        PromptText = "2",
        MenuLabel = () => "1 AI",
        MethodToRun = () =>
        {
            game.SetAllButOnePlayerHuman(); 
            return "b";
        }
    }
}, menuLevels);

var startNewGameMenu = new Menu("New game", new List<MenuItem>
{
    new()
    {
        PromptText = "c",
        MenuLabel = () => "Player count: " + game.GetPlayerCount(),
        MethodToRun = SetPlayerCount
    },
    new()
    {
        PromptText = "t",
        MenuLabel = () => "Player types: " + game.GetPlayerTypes(),
        MethodToRun = playerTypesMenu.RunOnce
    },
    new()
    {
        PromptText = "n",
        MenuLabel = () => "Nicknames: " + game.GetPlayerNicknames(),
        MethodToRun = SetNickNames
    },
    new()
    {
        PromptText = "s",
        MenuLabel = () => "Start the game",
        MethodToRun = GameLoop
    }
}, menuLevels);

var mainMenu = new Menu("Main menu", new List<MenuItem>
{
    new()
    {
        PromptText = "n",
        MenuLabel = () => "Start a new game",
        MethodToRun = () =>
        {
            game = new GameEngine(gameRepository);
            return startNewGameMenu.Run();
        }
    },
    new()
    {
        PromptText = "l",
        MenuLabel = () => "Load a game",
        MethodToRun = () =>
        {
            var menuItems = new List<MenuItem>();
            int i = 1;
            List<(Guid ID, DateTime LastEditedAt)> saves;
            try
            {
                saves = gameRepository.GetAllSaves();
            }
            catch (Exception)
            {
                Console.WriteLine("No saves");
                return "";
            }

            foreach (var save in saves)
            {
                menuItems.Add(new MenuItem()
                {
                    PromptText = i.ToString(),
                    MenuLabel = () => save.ID + ", " + save.LastEditedAt,
                    MethodToRun = () =>
                    {
                        game.LoadGame(save.ID);
                        return GameLoop();
                    }
                    
                });
                i++;
            }
            var loadGameMenu = new Menu("Load menu", menuItems, menuLevels);
            return loadGameMenu.RunOnce();
        }
    }
}, menuLevels, menuLevels[0]);
mainMenu.Run();