using api.plugin.services;
using api.plugin;
using api.plugin.services.menus;
using CounterStrikeSharp.API.Core;
using shared.Menu;
using plugin.modelsl;
using plugin.models;
using shared.Menu.enums;
using Serilog.Core;
using Microsoft.Extensions.Logging;
using CounterStrikeSharp.API;

namespace plugin.services.menus;
public class MapMenu : IMapsMenu
{
    private readonly IEvent baseEvent;
    private readonly IAnnouncer announcer;
    private MapGroup currentMapGroup;
    private int activePage = 1;
    private int pageSize = 5;
    private Menu parentMenu;
    private int pageCount;

    private readonly Dictionary<CCSPlayerController, MenuBase> _dynamicMenu = new();

    public MapMenu(IEvent baseEvent)
    {
        this.baseEvent = baseEvent;
        announcer = baseEvent.getAnnouncer();
        parentMenu = baseEvent.getECMenu().getBaseMenu();
        currentMapGroup = baseEvent.getMapGroupService().FetchCurrentGroup();
        pageCount = (int)Math.Ceiling((double)currentMapGroup.Maps.Count / pageSize);

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

    public void BuildMapsMenu(CCSPlayerController controller)
    {
        currentMapGroup = baseEvent.getMapGroupService().FetchCurrentGroup();
        pageCount = (int)Math.Ceiling((double)currentMapGroup.Maps.Count / pageSize);

        var mapsMenu = new MenuBase(new MenuValue("Settings"));

        mapsMenu.Cursor =
        [
            new MenuValue("►") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            new MenuValue("◄") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        mapsMenu.Selector =
        [
        new MenuValue("[ ") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue(" ]") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        mapsMenu.AddItem(GetItemValue(activePage, 0));
        mapsMenu.AddItem(GetItemValue(activePage, 1));
        mapsMenu.AddItem(GetItemValue(activePage, 2));
        mapsMenu.AddItem(GetItemValue(activePage, 3));
        mapsMenu.AddItem(GetItemValue(activePage, 4));

        mapsMenu.AddItem(new MenuItem(MenuItemType.Choice, new MenuValue("Page: ")
        { Prefix = "<font color=\"#3399FF\">", Suffix = "<font color=\"#FFFFFF\">" }, GetPageSelectorValues()));

        _dynamicMenu[controller] = mapsMenu;

        parentMenu.AddMenu(controller, mapsMenu, (buttons, menu, selectedItem) =>
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
                    if (ind >= currentMapGroup.Maps.Count) break;
                    var map = currentMapGroup.Maps[ind];

                    announcer.AnnounceChanges(AnnoncementType.MapChange, controller, "Changed the map to", map.Name);
                    baseEvent.getMapGroupService().ChangeMap(map);
                    break;
                default: break;
            }
        });
    }

    public MenuItem GetItemValue(int page, int slot)
    {
        if ((((page - 1) * pageSize) + slot) >= currentMapGroup.Maps.Count)
        {
            return new MenuItem(MenuItemType.Button, [new MenuValue("None")
            { Prefix = "<font color=\"#555555\">", Suffix = "<font color=\"#FFFFFF\">" }]);
        }

        return new MenuItem(MenuItemType.Button, [new MenuValue(currentMapGroup.Maps[((page - 1) * pageSize) + slot].Name)
        { Prefix = "<font color=\"#FFFFFF\">", Suffix = "<font color=\"#FFFFFF\">" }]);
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
