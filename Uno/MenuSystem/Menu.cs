using static System.String;

namespace MenuSystem;

public class Menu
{
    private string? Title { get; set; }
    private Dictionary<string, MenuItem> MenuItems { get; set; } = new();

    private List<string> Returns { get; set; } = new()
    {
        "x",
        "askForColor",
        "won"
    };

    private readonly string? _thisMenuLevel;

    private const string MenuSeparator = "----------------";
    private readonly HashSet<string> _reservedPromptKeys = new() {"x", "b"};

    public Menu(string? title, List<MenuItem> menuItems, List<string> menuLevels, string? thisMenuLevel = null)
    {
        Title = title;
        _thisMenuLevel = thisMenuLevel;
        Returns.AddRange(menuLevels);
        foreach (var menuItem in menuItems)
        {
            AddMenuItem(menuItem);
        }
    }

    public void AddMenuItem(MenuItem menuItem)
    {
        menuItem.PromptText = menuItem.PromptText?.ToLower();
        if (menuItem.PromptText != null && _reservedPromptKeys.Contains(menuItem.PromptText))
        {
            throw new ApplicationException($"Illegal menu prompt key: '{menuItem.PromptText}'");
        }

        if (menuItem.PromptText != null && MenuItems.ContainsKey(menuItem.PromptText))
        {
            throw new ApplicationException($"{menuItem.PromptText} Is already a registered menu prompt key");
        }

        if (menuItem.PromptText != null) MenuItems[menuItem.PromptText] = menuItem;
    }
    
    private void Draw()
    {
        if (!IsNullOrWhiteSpace(Title))
        {
            Console.WriteLine(Title);
            Console.WriteLine(MenuSeparator);
        }
        foreach (var menuItem in MenuItems)
        {
            Console.Write(menuItem.Key);
            Console.Write(") ");
            Console.WriteLine(menuItem.Value.MenuLabel?.Invoke());
            Console.ForegroundColor = ConsoleColor.White;
        }

        if (_reservedPromptKeys.Contains("b"))
        {
            Console.WriteLine("b) Back");
        }

        if (_reservedPromptKeys.Contains("x"))
        {
            Console.WriteLine("x) Exit");
        }
        Console.WriteLine(MenuSeparator);
        Console.Write("Your Choice: ");
    }

    public string? RunOnce()
    {
        Draw();
        var userChoice = Console.ReadLine()?.Trim().ToLower();
        Console.Clear();
        if (userChoice != null && MenuItems.ContainsKey(userChoice))
        {
            if (MenuItems[userChoice].MethodToRun != null)
            {
                var result = MenuItems[userChoice].MethodToRun!.Invoke();
                
                if (result != null && 
                    Returns.Contains(result) &&
                    _thisMenuLevel != result) return result;
            }
        }
        else if (userChoice != null && !_reservedPromptKeys.Contains(userChoice))
        {
            Console.WriteLine($"Undefined option: {userChoice}");
        }

        return userChoice;
    }

    public string? Run()
    {
        var userChoice = "";
        do
        {
            userChoice = RunOnce();
            if (userChoice != null && 
                Returns.Contains(userChoice) &&
                _thisMenuLevel != userChoice) return userChoice;
        } while ((_thisMenuLevel != null && 
                  userChoice == _thisMenuLevel) 
                 || 
                 (userChoice != null && 
                  !_reservedPromptKeys.Contains(userChoice)));
        return userChoice;
    }

    public void RemoveBackAndExitKeys()
    {
        _reservedPromptKeys.Clear();
    }
    
    public void RemoveBackKey()
    {
        _reservedPromptKeys.Remove("b");
    }
}