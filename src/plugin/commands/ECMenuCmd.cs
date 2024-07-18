using api.plugin;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using plugin.extensions;
using plugin.services.ECMenuServices;

namespace plugin.commands;

public class ECMenuCmd(IEvent eventBase) : Command(eventBase)
{
    IECMenu ECMenuService = eventBase.getECMenu();
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (executor == null || !executor.IsReal()) return;

        ECMenuService.BuildECMenu(executor);
    }
}
