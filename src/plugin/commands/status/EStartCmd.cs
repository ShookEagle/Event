using api.plugin;
using api.plugin.services;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using plugin.extensions;

namespace plugin.commands.status;
public class EStartCmd(IEvent eventBase) : Command(eventBase)
{
    IStatusService service = eventBase.getStatusService();
    IAnnouncer announcer = eventBase.getAnnouncer();
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (!executor.IsReal() || !executor.IsEventCoordinator()) return;

        if (service.IsStarted())
        {
            info.ReplyLocalized(eventBase.getBase().Localizer, "command_status_err_event_started");
            return;
        }

        var players = Utilities.GetPlayers().Where(p => p.IsReal());
        foreach (var player in players)
        {
            service.AddPlayer(player);
        }
        service.SetPeak(Utilities.GetPlayers().Count(p => p.IsReal()));
        service.StartCollection();
        announcer.AnnounceToECS(executor, "command_status_event_started");
    }
}
