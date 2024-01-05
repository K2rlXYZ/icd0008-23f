namespace MenuSystem;

public class Menu
{
    
    // TODO: class property for EMenuLevel
    public string? Title { get; set; } = default!;
    public Dictionary<string, MenuItem> MenuItems { get; set; } = new();

    private const string MenuSeparator = "--------------------";
    private static readonly HashSet<string> ReservedShortcuts = new() {"x", "b", "r"};

    public Menu(string? title, List<MenuItem> menuItems, EMenuLevel level = EMenuLevel.Other)
    {
        Title = title;
        foreach (var menuItem in menuItems)
        {
            if (ReservedShortcuts.Contains(menuItem.Shortcut.ToLower()))
            {
                throw new ApplicationException($"Menu shortcut '{menuItem.Shortcut}' in not allowed list!");
            }
            if (MenuItems.ContainsKey(menuItem.Shortcut.ToLower()))
            {
                throw new ApplicationException($"Menu shortcut '{menuItem.Shortcut}' in already registered!");
            }
            
            MenuItems[menuItem.Shortcut.ToLower()] = menuItem;
        }
    }
    
    private void Draw()
    {
        if (!string.IsNullOrWhiteSpace(Title))
        {
            Console.WriteLine(Title);
            Console.WriteLine(MenuSeparator);
        }
        foreach (var menuItem in MenuItems)
        {
            Console.Write(menuItem.Key);
            Console.Write(") ");
            Console.WriteLine(menuItem.Value.MenuLabel);
        }
        
        Console.WriteLine("b) Back");
        Console.WriteLine("r) Return to Main");
        Console.WriteLine("x) eXit");
        
        Console.WriteLine(MenuSeparator);
        Console.Write("Your choice: ");
    }

    public string Run(EMenuLevel menuLevel = EMenuLevel.Other)
    {
        string userChoice;
        
        do
        {
            Draw();
            userChoice = Console.ReadLine()?.Trim();
            Console.Clear();
            if (MenuItems.ContainsKey(userChoice?.ToLower()))
            {
                string? result;
                if (MenuItems[userChoice.ToLower()].SubMenuToRun != null)
                {
                    if (menuLevel == EMenuLevel.First)
                    {
                        result = MenuItems[userChoice.ToLower()].SubMenuToRun!(EMenuLevel.Second);
                    }
                    else
                    {
                        result =MenuItems[userChoice.ToLower()].SubMenuToRun!(EMenuLevel.Other);
                    }
                    
                    // TODO: handle b, x, r
                }
                result = MenuItems[userChoice.ToLower()].MethodToRun?.Invoke();
                if (result?.ToLower() == "x")
                {
                    userChoice = "x";
                }
            }
            else if (!ReservedShortcuts.Contains(userChoice.ToLower()))
            {
                Console.WriteLine($"Undefined shortcut '{userChoice.ToLower()}'");
            }
            
            Console.WriteLine();
            
        } while (!ReservedShortcuts.Contains(userChoice));

        return userChoice;
    }
}