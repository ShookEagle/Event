using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using api.plugin.services;
using plugin.extensions;
using api.plugin;

namespace plugin.commands.status;
public class EStatusCmd(IEvent eventBase) : Command(eventBase)
{
    IStatusService service = eventBase.getStatusService();
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info) 
    {
        if (!executor.IsReal() || !executor.IsEventCoordinator()) return;

        if (!service.IsStarted())
        {
            info.ReplyLocalized(eventBase.getBase().Localizer, "command_status_err_not_started");
            return;
        }

        service.PrintStatus(executor);
        info.ReplyLocalized(eventBase.getBase().Localizer, "command_status_printed");
    }
}

