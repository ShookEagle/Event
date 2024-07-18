using api.plugin;
using plugin.extensions;
using CounterStrikeSharp.API.Core;
using plugin.services.ECMenuServices;
using shared.Menu;
using shared.Menu.enums;

namespace plugin.services.ECMenu;
public class ECMenu(IEvent baseEvent) : IECMenu
{
    public Menu Menu { get; } = new();

    public void BuildECMenu(CCSPlayerController controller)
    {
        var mainMenu = new MenuBase(new MenuValue("EC Menu")
        {
            Prefix = "<font class=\"fontSize-L\">",
            Suffix = "<font class=\"fontSize-sm\">"
        });

        mainMenu.Cursor =
        [
            new MenuValue("►") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            new MenuValue("◄") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Modes")]));
        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Maps")]));
        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Settings")]));
        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Tools")]));

        mainMenu.AddItem(new MenuItem(MenuItemType.Text, new MenuValue("Welcome to the new menu!")
        {
            Prefix = "<font color=\"#FF0000\">",
            Suffix = "<font color=\"#FFFFFF\">"
        }));

        Menu.SetMenu(controller, mainMenu, (buttons, menu, selectedItem) => 
        {
            if (buttons != MenuButtons.Select) return;

            switch (menu.Option)
            {
                case 0:
                    controller.PrintLocalizedChat(baseEvent.getBase().Localizer, "test");
                    break;
                case 1:
                    controller.PrintLocalizedChat(baseEvent.getBase().Localizer, "test");
                    break;
                case 2:
                    controller.PrintLocalizedChat(baseEvent.getBase().Localizer, "test");
                    break;
                case 3:
                    controller.PrintLocalizedChat(baseEvent.getBase().Localizer, "test");
                    break;
            }
        });
    }
}

public class ButtonValue(string value, ButtonType button) : MenuValue(value)
{
    public ButtonType Button { get; set; } = button;
}
