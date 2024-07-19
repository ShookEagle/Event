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
using System.Net.Sockets;

namespace plugin.services.menus;

public class ModesMenu(IEvent baseEvent) : IModesMenu
{
    public string activeMode = "None";
    IAnnouncer announcer = baseEvent.getAnnouncer();

    public void BuildModesMenu(CCSPlayerController controller)
    {
        var Modes = GetModesFromJson();
        var parentMenu = baseEvent.getECMenu().getBaseMenu();

        if (!Modes.Any())
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

        foreach (var mode in Modes)
        {
            var color = (mode.Name == activeMode) ? "008000" : "FFFFFF";
            modesMenu.AddItem(new MenuItem(MenuItemType.Button, [new MenuValue(mode.Name)
            { Prefix = $"<font color=\"#{color}\">", Suffix = "<font color=\"#FFFFFF\">" }]));
        }

        parentMenu.AddMenu(controller, modesMenu, (buttons, menu, item) =>
        {
            if (buttons != MenuButtons.Select) return;

            var selectedMode = Modes[menu.Option];
            if (selectedMode == null)
            {
                controller.PrintLocalizedChat(baseEvent.getBase().Localizer, "error_menu_mode_not_passed");
                return;
            }

            ExecuteMode(controller, selectedMode);
        });
    }

    public void ExecuteMode(CCSPlayerController executor, Mode mode)
    {
        //Server.ExecuteCommand($"exec \"utils/unload_plugins.cfg\"");
        //Set Mapgroup Internally Here
        Server.ExecuteCommand($"hostname \"=(eGO)= | EVENTS | {mode.Name.ToUpper()} | EdgeGamers.com\"");
        Server.ExecuteCommand($"sv_tags event, events, ego, edgegamers, {String.Join(", ", mode.Tags?.ToArray()!)}");
        announcer.AnnounceChanges(AnnoncementType.ModeChange, executor, "Changed the mode to", mode.Name);
        activeMode = mode.Name;
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