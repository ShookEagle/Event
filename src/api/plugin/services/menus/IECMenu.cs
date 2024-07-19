using CounterStrikeSharp.API.Core;
using shared.Menu;
using plugin.enums;

namespace api.plugin.services.menus;
public interface IECMenu
{
    void BuildECMenu(CCSPlayerController controller, GoToSubMenu toSubMenu = GoToSubMenu.None);
    Menu getBaseMenu();
}
