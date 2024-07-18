using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using maul.enums;
using plugin.utils;
using Microsoft.Extensions.Localization;

namespace plugin.extensions;

public static class PlayerExtensions
{
    public static bool IsReal(this CCSPlayerController? player)
    {
        return (player != null && !player.IsBot && !player.IsHLTV &&
            player.IsValid && player.Connected == PlayerConnectedState.PlayerConnected);
    }

    public static bool IsEventCoordinator(this CCSPlayerController? controller)
    {
        return controller?.GetRank() >= MaulPermission.Manager;
    }

    public static void PrintLocalizedChat(this CCSPlayerController? controller, IStringLocalizer localizer, string local,
        params object[] args)
    {
        if (controller == null || !controller.IsReal()) return;
        string message = localizer[local, args];
        message = message.Replace("%prefix%", localizer["prefix"]);
        message = StringUtils.ReplaceChatColors(message);
        controller.PrintToChat(message);
    }

    public static void PrintLocalizedConsole(this CCSPlayerController? controller, IStringLocalizer localizer,
        string local, params object[] args)
    {
        if (controller == null || !controller.IsReal()) return;
        string message = localizer[local, args];
        message = message.Replace("%prefix%", localizer["prefix"]);
        message = StringUtils.ReplaceChatColors(message);
        controller.PrintToConsole(message);
    }

    public static MaulPermission GetRank(this CCSPlayerController player)
    {
        if (!player.IsReal())
            return MaulPermission.None;
        if (AdminManager.PlayerInGroup(player, "#ego/root") || AdminManager.PlayerHasPermissions(player, "@css/root"))
            return MaulPermission.Root;
        if (AdminManager.PlayerInGroup(player, "#ego/executive"))
            return MaulPermission.Executive;
        if (AdminManager.PlayerInGroup(player, "#ego/directory"))
            return MaulPermission.Director;
        if (AdminManager.PlayerInGroup(player, "#ego/commgr"))
            return MaulPermission.CommunityManager;
        if (AdminManager.PlayerInGroup(player, "#ego/srmanager"))
            return MaulPermission.SeniorManager;
        if (AdminManager.PlayerInGroup(player, "#ego/manager"))
            return MaulPermission.Manager;
        if (AdminManager.PlayerInGroup(player, "#ego/advisor"))
            return MaulPermission.Advisor;
        if (AdminManager.PlayerInGroup(player, "#ego/ego"))
            return MaulPermission.EGO;
        if (AdminManager.PlayerInGroup(player, "#ego/eg"))
            return MaulPermission.EG;
        if (AdminManager.PlayerInGroup(player, "#ego/e"))
            return MaulPermission.E;
        return MaulPermission.None;
    }


}
