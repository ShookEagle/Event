using api.plugin;
using CounterStrikeSharp.API.Core;

namespace plugin.listeners;

public class MapStartListener
{
    private readonly IEvent eventBase;
    public MapStartListener(IEvent eventBase)
    {
        this.eventBase = eventBase;
        eventBase.getBase().RegisterListener<Listeners.OnMapStart>(OnMapStart);
    }

    public void OnMapStart(string mapname)
    {
        eventBase.getModesServices().execSettings();
    }
}