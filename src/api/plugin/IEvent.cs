using plugin;
using CounterStrikeSharp.API.Core;
using api.plugin.services;
using api.plugin.services.menus;

namespace api.plugin;
public interface IEvent : IPluginConfig<EventConfig>
{
    BasePlugin getBase();
    IStatusService getStatusService();
    IAnnouncer getAnnouncer();
    IECMenu getECMenu();
    IModesMenu getModesServices();
    IResetMenu getResetMenu();
    ISettingsMenu getSettingsMenu();
    ISettingsService getSettingsService();
    IMapGroupService getMapGroupService();
    IMapsMenu getMapsMenu();
}