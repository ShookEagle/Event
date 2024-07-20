using api.plugin;
using api.plugin.services;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using plugin.models;

namespace plugin.services;
public class SettingsService : ISettingsService
{
    private IEvent baseEvent;
    private IAnnouncer announcer;
    public List<Setting> Settings;
    private readonly HashSet<Setting> recentEvents = new();

    public SettingsService(IEvent baseEvent)
    {
        this.baseEvent = baseEvent;
        announcer = baseEvent.getAnnouncer();
        Settings = GetSettingsFromJson();
    }

    public List<Setting> GetSettings()
    {
        return Settings;
    }
    public void ClearHashset() //to furture me: I know this how system with the hashset looks crazy. don't try and fix it. this was 3 hours of avoiding the jank solution and fianlly caving in :)
    {
        recentEvents.Clear();
    }

    public void ToggleSetting(Setting setting, CCSPlayerController? executor = null, bool viaCommand = false)
    {

        if (recentEvents.Contains(setting)) return;

        if (!viaCommand)
            recentEvents.Add(setting);


        var settingIndex = Settings.FindIndex(s => s.Name == setting.Name);
        if (settingIndex == -1) return;

        Settings[settingIndex].IsActive = !Settings[settingIndex].IsActive;
        var newState = Settings[settingIndex].IsActive;

        var suffix = (newState) ? "on" : "off";
        var type = (newState) ? AnnoncementType.SettingEnable : AnnoncementType.SettingDisable;
        var target = (newState) ? "Enabled" : "Disabled";

        if (executor != null)
            announcer.AnnounceChanges(type, executor, $"Set {setting.Name} to", target);
        //Server.ExecuteCommand($"exec settings/{setting.Stem}_{suffix}");
    }



    public List<Setting> GetSettingsFromJson()
    {
        List<Setting> settingsList = new();

        if (baseEvent.Config.Settings != null)
        {
            foreach (var setting in baseEvent.Config.Settings)
            {
                var settingValue = setting.Value?.AsObject();
                if (settingValue != null)
                {
                    var settingObj = new Setting
                    {
                        Name = settingValue["name"]?.GetValue<string>() ?? string.Empty,
                        Stem = settingValue["stem"]?.GetValue<string>() ?? string.Empty,
                        Default = bool.TryParse(settingValue["default"]?.GetValue<string>(), out bool defaultValue) && defaultValue
                    };
                    settingObj.IsActive = settingObj.Default;
                    settingsList.Add(settingObj);
                }
            }
        }
        else baseEvent.getBase().Logger.LogError("Unable to Fetch settings from Json");
        return settingsList.OrderBy(m => m.Name).ToList();
    }
}
