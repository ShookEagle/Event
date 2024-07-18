using api.plugin.services;
using plugin.extensions;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using api.plugin;

namespace plugin.listeners;

public class ConnectionListener(IEvent eventBase)
{
    IStatusService service = eventBase.getStatusService();

    
    [GameEventHandler]
    public HookResult OnPlayerConnect(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (service.IsStarted()) return HookResult.Continue;

        service.AddPlayer(@event.Userid);

        int pCount = Utilities.GetPlayers().Count(p => p.IsReal());
        if (service.GetPeak() < pCount)
            service.SetPeak(pCount);

        return HookResult.Continue;
    }
}