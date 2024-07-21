using CounterStrikeSharp.API.Core;
using plugin.models;

namespace api.plugin.services;
public interface ISettingsService
{
    List<Setting> GetSettings();
    void ToggleSetting(Setting setting, CCSPlayerController executor, bool viaCommand = false);
    void ForceSetting(string stem, string state);
    void ClearHashset();
    void SetDefaults();
}
