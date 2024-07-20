using CounterStrikeSharp.API.Core;
using plugin.models;

namespace api.plugin.services;
public interface ISettingsService
{
    List<Setting> GetSettings();
    void ToggleSetting(Setting setting, CCSPlayerController? executor = null, bool viaCommand = false);

    void ClearHashset();
}
