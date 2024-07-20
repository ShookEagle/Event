using api.plugin.services;
using api.plugin;
using plugin.extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using maul.enums;

namespace plugin.services;

public class AnonymousAnnouncer(IEvent plugin) : IAnnouncer
{
    public void AnnounceChanges(AnnoncementType type, CCSPlayerController? admin, string action, string target)
    {
        string msg = string.Empty;
        switch (type)
        {
            case AnnoncementType.MapChange:
                msg = "ec_action_map"; break;
            case AnnoncementType.ModeChange:
                msg = "ec_action_mode"; break;
            case AnnoncementType.SettingDisable:
                msg = "ec_action_disable"; break;
            case AnnoncementType.SettingEnable:
                msg = "ec_action_enable"; break;
            default: msg = "error_announcement_unkown"; break;
        }

        foreach (var player in Utilities.GetPlayers())
        {
            var displayAdmin = (player.GetRank() >= MaulPermission.Manager) ? $"{admin?.PlayerName} " : String.Empty;
            player.PrintLocalizedChat(plugin.getBase().Localizer, msg, displayAdmin, action, target);
        }
    }

    public void AnnounceToECS(CCSPlayerController? admin, string msg)
    {
        if (admin == null) return;
        foreach (var player in Utilities.GetPlayers().Where(p => p.GetRank() >= MaulPermission.Manager))
        {
            player.PrintLocalizedChat(plugin.getBase().Localizer, msg, admin.PlayerName);
        }
    }
}