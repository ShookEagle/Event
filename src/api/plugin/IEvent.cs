using plugin;
using CounterStrikeSharp.API.Core;
using api.plugin.services;

namespace api.plugin;
public interface IEvent : IPluginConfig<EventConfig>
{
    BasePlugin getBase();
    IStatusService getStatusService();
    IAnnouncer getAnnouncer();
}