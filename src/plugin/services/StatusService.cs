using api.plugin;
using api.plugin.services;
using plugin.extensions;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;

namespace plugin.services;
public class StatusService(IEvent plugin) : IStatusService
{
    public int peakPlayers;
    public bool eventStarted;
    public TimeSpan Duration;
    public readonly Dictionary<ulong, TimeSpan> allJoiners = new();
    public readonly Dictionary<ulong, string> allNames = new();

    public void AddPlayer(CCSPlayerController? player)
    {
        if (player == null || !player.IsReal()) return;
        allJoiners.TryAdd(player.SteamID, TimeSpan.Zero);
        allNames.TryAdd(player.SteamID, player.PlayerName);
    }

    public void Clear()
    {
        allJoiners.Clear();
        allNames.Clear();
    }
    public StatusService getBase()
    {
        return this;
    }

    public bool IsStarted()
    {
        return eventStarted;
    }

    public void IncrementTimes()
    {
        var onlinePlayers = Utilities.GetPlayers().Where(p => p.IsReal());
        foreach (var player in onlinePlayers)
        {
            if (player.Connected == PlayerConnectedState.PlayerConnected)
                if (allJoiners.ContainsKey(player.SteamID))
                    allJoiners[player.SteamID] = allJoiners[player.SteamID].Add(TimeSpan.FromSeconds(1));
                else AddPlayer(player);
        }
        Duration = Duration.Add(TimeSpan.FromSeconds(1));
    }

    public IEnumerable<string> GetStatus()
    {
        List<string> statusMessages = new();
        var rank = 1;

        foreach (var player in allJoiners.OrderByDescending(k => k.Value))
        {
            statusMessages.Add($"{rank}. {player.Key} {allNames[player.Key]} {FormatTime(allJoiners[player.Key])}");
            rank++;
        }
        return statusMessages;
    }

    public string FormatTime(TimeSpan time)
    {
        if (time.TotalHours >= 1)
            return $"{time.TotalHours:F2} hours";
        if (time.TotalMinutes >= 1)
            return $"{time.TotalMinutes:F2} minutes";
        return $"{time.TotalSeconds:F0} seconds";
    }

    public void PrintStatus(CCSPlayerController? player)
    {
        var statusMessages = GetStatus();
        player.PrintLocalizedConsole(plugin.getBase().Localizer, "log_status_header", FormatTime(Duration), peakPlayers);
        foreach (var message in statusMessages)
            player.PrintLocalizedConsole(plugin.getBase().Localizer, "log_status_body", message);
        player.PrintLocalizedConsole(plugin.getBase().Localizer, "log_status_footer");
    }

    public void StartCollection()
    {
        plugin.getBase().AddTimer(1f, IncrementTimes, TimerFlags.REPEAT);
        eventStarted = true;
    }

    public void StopCollection()
    {
        plugin.getBase().Timers.Clear();
        eventStarted = false;
        Duration = TimeSpan.Zero;
        peakPlayers = 0;
    }

    public int GetPeak()
    {
        return peakPlayers;
    }

    public void SetPeak(int peak)
    {
        peakPlayers = peak;
    }
}
