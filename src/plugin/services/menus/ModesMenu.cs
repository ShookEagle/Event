using api.plugin;
using api.plugin.services;
using api.plugin.services.menus;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using plugin.extensions;
using plugin.modelsl;
using shared.Menu;
using shared.Menu.enums;

namespace plugin.services.menus;

//Since the plugin doesn't handle multiple pages we must build the logic ourselves for handling page switching.
//This makes some parts of this look very convoluted.
//There's almost definelty an easier way but this is what i could figure out so i'm keeping it.
//Pretty much just have to abuse the dynamic menu system to create a pretty page switching function.
public class ModesMenu : IModesMenu
{
    private readonly IEvent baseEvent;
    private readonly IAnnouncer announcer;
    private string activeMode = "None";
    private int activePage = 1;
    private int pageSize = 5;
    private List<Mode> Modes;
    private Menu parentMenu;
    private int pageCount;

    private readonly Dictionary<CCSPlayerController, MenuBase> _dynamicMenu = new();

    public ModesMenu(IEvent baseEvent)
    {
        this.baseEvent = baseEvent;
        announcer = baseEvent.getAnnouncer();
        Modes = GetModesFromJson();
        parentMenu = baseEvent.getECMenu().getBaseMenu();
        pageCount = (int)Math.Ceiling((double)Modes.Count / pageSize);

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

    public void BuildModesMenu(CCSPlayerController controller)
    {
        if (Modes.Count == 0)
        {
            controller.PrintLocalizedChat(baseEvent.getBase().Localizer, "error_json_deser_issue");
            return;
        }

        var modesMenu = new MenuBase(new MenuValue("Modes"));

        modesMenu.Cursor =
        [
            new MenuValue("►") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
            new MenuValue("◄") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        modesMenu.Selector =
        [
        new MenuValue("[ ") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" },
        new MenuValue(" ]") { Prefix = "<font color=\"#0033FF\">", Suffix = "<font color=\"#FFFFFF\">" }
        ];

        modesMenu.AddItem(GetItemValue(activePage, 0));
        modesMenu.AddItem(GetItemValue(activePage, 1));
        modesMenu.AddItem(GetItemValue(activePage, 2));
        modesMenu.AddItem(GetItemValue(activePage, 3));
        modesMenu.AddItem(GetItemValue(activePage, 4));

        modesMenu.AddItem(new MenuItem(MenuItemType.Choice, new MenuValue("Page: ") 
        { Prefix = "<font color=\"#3399FF\">", Suffix = "<font color=\"#FFFFFF\">" }, GetPageSelectorValues()));

        _dynamicMenu[controller] = modesMenu;

        parentMenu.AddMenu(controller, modesMenu, (buttons, menu, selectedItem) =>
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
                    if (ind >= Modes.Count) break;

                    ExecuteMode(controller, Modes[ind]);
                    Server.PrintToChatAll("mode");
                    break;
                default: break;
            }
        });
    }

    public void ExecuteMode(CCSPlayerController executor, Mode mode)
    {
        //Server.ExecuteCommand($"exec \"utils/unload_plugins.cfg\"");
        //Set Mapgroup Internally Here
        //Server.ExecuteCommand($"hostname \"=(eGO)= | EVENTS | {mode.Name.ToUpper()} | EdgeGamers.com\"");
        //Server.ExecuteCommand($"sv_tags event, events, ego, edgegamers, {String.Join(", ", mode.Tags?.ToArray()!)}");
        announcer.AnnounceChanges(AnnoncementType.ModeChange, executor, "Changed the mode to", mode.Name);
        activeMode = mode.Name;
    }

    public MenuItem GetItemValue(int page, int slot)
    {
        if ((((page - 1) * pageSize) + slot) >= Modes.Count)
        {
            return new MenuItem(MenuItemType.Button, [new MenuValue("None")
            { Prefix = "<font color=\"#555555\">", Suffix = "<font color=\"#FFFFFF\">" }]);
        }

        var color = (Modes[((page - 1) * pageSize) + slot].Name == activeMode) ? "008000" : "FFFFFF";
        return new MenuItem(MenuItemType.Button, [new MenuValue(Modes[((page - 1) * pageSize) + slot].Name)
        { Prefix = $"<font color=\"#{color}\">", Suffix = "<font color=\"#FFFFFF\">" }]);
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

    public List<Mode> GetModesFromJson()
    {
        List<Mode> modesList = new();

        if (baseEvent.Config.Modes != null)
        {
            foreach (var mode in baseEvent.Config.Modes)
            {
                var modeValue = mode.Value?.AsObject();
                if (modeValue != null)
                {
                    var modeObj = new Mode
                    {
                        Name = modeValue["name"]?.GetValue<string>() ?? string.Empty,
                        File = modeValue["file"]?.GetValue<string>() ?? string.Empty,
                        Tags = modeValue["tags"]?.AsArray().Select(t => t!.GetValue<string>()).ToList() ?? new List<string>(),
                        MapGroup = modeValue["mapgroup"]?.GetValue<string>() ?? "mg_active"
                    };
                    modesList.Add(modeObj);
                }
            }
        } else baseEvent.getBase().Logger.LogError("Unable to Fetch Modes from Json");
        return modesList.OrderBy(m => m.Name).ToList();
    }

}