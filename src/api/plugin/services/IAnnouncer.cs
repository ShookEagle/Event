using CounterStrikeSharp.API.Core;

namespace api.plugin.services;

public interface IAnnouncer
{
    void AnnounceChanges(AnnoncementType type, CCSPlayerController? admin, string action, string target);
    void AnnounceToECS(CCSPlayerController? admin, string action);
}

public enum AnnoncementType
{
    MapChange = 1, //yellow
    ModeChange = 2, //purple
    SettingEnable = 3, //lightgreen
    SettingDisable = 4 //lightred
}
