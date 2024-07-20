using api.plugin.services;
using api.plugin;
using api.plugin.services.menus;
using CounterStrikeSharp.API.Core;
using shared.Menu;
using shared.Menu.enums;
using plugin.extensions;

namespace plugin.services.menus;

public class ResetMenu : IResetMenu
{
    private readonly IEvent baseEvent;
    private readonly IAnnouncer announcer;
    private Menu parentMenu;

    public ResetMenu(IEvent baseEvent)
    {
        this.baseEvent = baseEvent;
        announcer = baseEvent.getAnnouncer();
        parentMenu = baseEvent.getECMenu().getBaseMenu();
    }

    public void BuildResetMenu(CCSPlayerController controller)
    {
        var resetMenu = new MenuBase(new MenuValue("Reset Server?"));

        resetMenu.Cursor =
        [
            new MenuValue("►") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            new MenuValue("◄") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        resetMenu.Selector =
        [
        new MenuValue("[ ") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue(" ]") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        resetMenu.AddItem(new MenuItem(MenuItemType.Text, new MenuValue("Are you sure you would like to reset the server to it's default config and map?")
        { Prefix = "<font class=\"fontSize-sm\"><font color=\"#FF0000\">", Suffix = "<font color=\"#FFFFFF\"><font class=\"fontSize-m\">" }));

        resetMenu.AddItem(new MenuItem(MenuItemType.Spacer));

        resetMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue("Confirm") 
        { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" }]));

        parentMenu.AddMenu(controller, resetMenu, (buttons, menu, selectedItem) =>
        {
            if (buttons != MenuButtons.Select) return;

            controller.PrintLocalizedChat(baseEvent.getBase().Localizer, "test");
        });
    }
}
