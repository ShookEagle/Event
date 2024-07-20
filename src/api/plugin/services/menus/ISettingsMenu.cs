using CounterStrikeSharp.API.Core;
using plugin.models;

namespace api.plugin.services.menus;

public interface ISettingsMenu
{
    void BuildSettingsMenu(CCSPlayerController controller);
}
