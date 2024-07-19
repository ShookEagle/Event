using CounterStrikeSharp.API.Core;
using plugin.enums;

namespace api.plugin.services.menus;

public interface IModesMenu
{
    void BuildModesMenu(CCSPlayerController controller);
}