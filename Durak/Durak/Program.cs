// See https://aka.ms/new-console-template for more information


using MenuSystem;

var subMenu1 = new Menu("Submenu 1", new List<MenuItem>()
{
    new MenuItem()
    {
        Shortcut = "i",
        MenuLabel = "Item 1",
    },
    new MenuItem()
    {
        Shortcut = "t",
        MenuLabel = "iTem 2",
    },
}, EMenuLevel.Second);

var mainMenu = new Menu("Main Menu", new List<MenuItem>()
{
    new MenuItem()
    {
        Shortcut = "s",
        MenuLabel = "Submenu 1",
        SubMenuToRun = subMenu1.Run,
    },
    new MenuItem()
    {
        Shortcut = "t",
        MenuLabel = "iTem 2",
    },
}, EMenuLevel.First);

var userChoice = mainMenu.Run(EMenuLevel.First);
