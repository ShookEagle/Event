using api.plugin.services;
using api.plugin;
using api.plugin.services.menus;
using CounterStrikeSharp.API.Core;
using shared.Menu;
using Microsoft.Extensions.Logging;
using plugin.models;
using System.Drawing;
using shared.Menu.enums;
using plugin.modelsl;
using CounterStrikeSharp.API;

namespace plugin.services.menus;

public class SettingsMenu : ISettingsMenu
{
    private readonly IEvent baseEvent;
    private readonly IAnnouncer announcer;
    private readonly ISettingsService settingsService;
    private Menu parentMenu;
    private int activePage = 1;
    private int pageSize = 5;
    private int pageCount;

    private readonly Dictionary<CCSPlayerController, MenuBase> _dynamicMenu = new();

    public SettingsMenu(IEvent baseEvent)
    {
        this.baseEvent = baseEvent;
        announcer = baseEvent.getAnnouncer();
        parentMenu = baseEvent.getECMenu().getBaseMenu();
        settingsService = baseEvent.getSettingsService();
        pageCount = (int)Math.Ceiling((double)settingsService.GetSettings().Count / pageSize);

        Menu.OnDrawMenu += (_, menuEvent) =>
        {
            var controller = menuEvent.Controller;

            if (!_dynamicMenu.TryGetValue(controller, out var dynamicMenu))
                return;

            if (menuEvent.Menu != dynamicMenu)
                return;

            dynamicMenu.Items[0] = GetItemValue(activePage, 0);
            dynamicMenu.Items[1] = GetItemValue(activePage, 1);
            dynamicMenu.Items[2] = GetItemValue(activePage, 2);
            dynamicMenu.Items[3] = GetItemValue(activePage, 3);
            dynamicMenu.Items[4] = GetItemValue(activePage, 4);
            dynamicMenu.Items[5].Values = GetPageSelectorValues();
        };
    }
    public void BuildSettingsMenu(CCSPlayerController controller)
    {
        var settingsMenu = new MenuBase(new MenuValue("Settings"));

        settingsMenu.Cursor =
        [
            new MenuValue("►") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            new MenuValue("◄") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        settingsMenu.Selector =
        [
        new MenuValue("[ ") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue(" ]") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        settingsMenu.AddItem(GetItemValue(activePage, 0));
        settingsMenu.AddItem(GetItemValue(activePage, 1));
        settingsMenu.AddItem(GetItemValue(activePage, 2));
        settingsMenu.AddItem(GetItemValue(activePage, 3));
        settingsMenu.AddItem(GetItemValue(activePage, 4));

        settingsMenu.AddItem(new MenuItem(MenuItemType.Choice, new MenuValue("Page: ")
        { Prefix = "<font color=\"#3399FF\">", Suffix = "<font color=\"#FFFFFF\">" }, GetPageSelectorValues()));

        _dynamicMenu[controller] = settingsMenu;

        parentMenu.AddMenu(controller, settingsMenu, (buttons, menu, selectedItem) =>
        {
            switch (buttons)
            {
                case MenuButtons.Left:
                    if (selectedItem!.Type == MenuItemType.Choice)
                    {
                        activePage = (activePage - 1 > 0) ? activePage - 1 : 1;
                    }
                    break;
                case MenuButtons.Right:
                    if (selectedItem!.Type == MenuItemType.Choice)
                    {
                        activePage = (activePage + 1 < pageCount) ? activePage + 1 : pageCount;
                    }
                    break;
                case MenuButtons.Exit:
                    activePage = 1;
                    break;
                case MenuButtons.Back:
                    activePage = 1;
                    break;
                case MenuButtons.Select:
                    if (selectedItem!.Type != MenuItemType.Button) break;

                    int ind = ((activePage - 1) * pageSize) + menu.Option;
                    if (ind >= settingsService.GetSettings().Count) break;

                    settingsService.ToggleSetting(settingsService.GetSettings()[ind], controller);
                    settingsService.ClearHashset();
                    break;
                default: break;
            }
        });
    }

    public MenuItem GetItemValue(int page, int slot)
    {
        if ((((page - 1) * pageSize) + slot) >= settingsService.GetSettings().Count)
        {
            return new MenuItem(MenuItemType.Button, [new MenuValue("None")
            { Prefix = "<font color=\"#555555\">", Suffix = "<font color=\"#FFFFFF\">" }]);
        }

        var color = (settingsService.GetSettings()[((page - 1) * pageSize) + slot].IsActive) ? "008000" : "FF0000";
        var symbol = (settingsService.GetSettings()[((page - 1) * pageSize) + slot].IsActive) ? "✔" : "✘";
        return new MenuItem(MenuItemType.Button, [new MenuValue($"{settingsService.GetSettings()[((page - 1) * pageSize) + slot].Name} - " +
            $"<font color=\"#{color}\">{symbol}") { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" }]);
    }

    public List<MenuValue> GetPageSelectorValues()
    {
        List<MenuValue> valueList = new();
        for (int i = 1; i <= pageCount; i++)
        {
            var numColor = (i == activePage) ? "3399FF" : "6688BB";
            valueList.Add(new($"{i}") { Prefix = $"<font color=\"#{numColor}\">", Suffix = "<font color=\"#FFFFFF\">" });
        }
        return valueList;
    }
}
