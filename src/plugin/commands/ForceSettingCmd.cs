using api.plugin;
using api.plugin.services;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using plugin.extensions;

namespace plugin.commands;
public class ForceSettingCmd(IEvent eventBase) : Command(eventBase)
{
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (executor.IsReal()) return; //This Command is only for cfgs to use

        if (info.ArgCount != 3)
        {
            eventBase.getBase().Logger.LogError("Bad Arguments in css_forcesetting | Usage: css_forcesetting [stem] <on/off>");
        }

        eventBase.getSettingsService().ForceSetting(info.ArgByIndex(1), info.ArgByIndex(2));
    }
}
