using api.plugin;
using plugin.extensions;
using CounterStrikeSharp.API.Core;
using api.plugin.services.menus;
using shared.Menu;
using shared.Menu.enums;
using plugin.enums;
using CounterStrikeSharp.API;
using System.Drawing;

namespace plugin.services.menus;
public class ECMenu(IEvent baseEvent) : IECMenu
{
    public Menu Menu { get; } = new();

    public void BuildECMenu(CCSPlayerController controller, GoToSubMenu toSubMenu = GoToSubMenu.None)
    {
        var mainMenu = new MenuBase(new MenuValue("EC Menu")
        {
            Prefix = "<font class=\"fontSize-L\"><font color=\"#3399FF\">",
            Suffix = "<font color=\"#FFFFFF\"><font class=\"fontSize-m\">"
        });

        mainMenu.Cursor =
        [
            new MenuValue("►") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            new MenuValue("◄") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        mainMenu.Selector =
        [
        new MenuValue("[ ") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue(" ]") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Modes")]));
        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Maps")]));
        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Settings")]));
        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Tools")]));
        mainMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Reset") { Prefix = "<font color=\"#FF0000\">", Suffix = "<font color=\"#FFFFFF\">" }]));

        Menu.SetMenu(controller, mainMenu, (buttons, menu, selectedItem) =>
        {
            if (toSubMenu != GoToSubMenu.None)
            {
                switch (toSubMenu)
                {
                    case GoToSubMenu.Modes:
                        baseEvent.getModesServices().BuildModesMenu(controller);
                        break;
                    case GoToSubMenu.Maps:
                        break;
                    case GoToSubMenu.Settings:
                        break;
                    case GoToSubMenu.Tools:
                        break;
                }
            }

            if (buttons != MenuButtons.Select) return;

            switch (menu.Option)
            {
                case 0:
                    baseEvent.getModesServices().BuildModesMenu(controller);
                    break;
                case 1:
                    baseEvent.getMapsMenu().BuildMapsMenu(controller);
                    break;
                case 2:
                    baseEvent.getSettingsMenu().BuildSettingsMenu(controller);
                    break;
                case 3:
                    controller.PrintLocalizedChat(baseEvent.getBase().Localizer, "test");
                    break;
                case 4:
                    baseEvent.getResetMenu().BuildResetMenu(controller);
                    break;
            }
        });
    }

    public Menu getBaseMenu()
    {
        return Menu;
    }
}
