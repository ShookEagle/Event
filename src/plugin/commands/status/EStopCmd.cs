using api.plugin;
using api.plugin.services;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using plugin.extensions;

namespace plugin.commands.status;
public class EStopCmd(IEvent eventBase) : Command(eventBase)
{
    IStatusService service = eventBase.getStatusService();
    IAnnouncer announcer = eventBase.getAnnouncer();
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (!executor.IsReal() || !executor.IsEventCoordinator()) return;

        if (!service.IsStarted())
        {
            info.ReplyLocalized(eventBase.getBase().Localizer, "command_status_err_not_started");
            return;
        }
        service.StopCollection();
        service.Clear();
        announcer.AnnounceToECS(executor, "command_status_event_stopped");
    }
}
