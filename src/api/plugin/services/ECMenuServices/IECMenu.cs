using CounterStrikeSharp.API.Core;

namespace plugin.services.ECMenuServices;
public interface IECMenu
{
    void BuildECMenu(CCSPlayerController controller);
}

public enum ButtonType
{
    Modes = 1,
    Maps = 2,
    Settings = 3,
    Tools = 4,
}
