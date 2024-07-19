using api.plugin;
using api.plugin.services.menus;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using plugin.enums;
using plugin.extensions;
using plugin.modelsl;
using System.ComponentModel.Design;
using System.Text.Json.Nodes;

namespace plugin.commands;

public class ModesMenuCmd(IEvent baseEvent) : Command(baseEvent)
{
    IECMenu ECMenuService = baseEvent.getECMenu();
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (executor == null || !executor.IsReal()) return;

        ECMenuService.BuildECMenu(executor, GoToSubMenu.Modes);
    }
}