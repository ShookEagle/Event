using CounterStrikeSharp.API.Core;
using plugin.enums;
using shared.Menu;

namespace api.plugin.services.menus;

public interface IModesMenu
{
    void BuildModesMenu(CCSPlayerController controller);
}