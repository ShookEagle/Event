using CounterStrikeSharp.API.Core;
using plugin.services;

namespace api.plugin.services;
public interface IStatusService
{
    void AddPlayer(CCSPlayerController? player);
    void Clear();
    StatusService getBase();
    bool IsStarted();
    void IncrementTimes();
    IEnumerable<string> GetStatus();
    string FormatTime(TimeSpan time);
    void PrintStatus(CCSPlayerController? player);
    void StartCollection();
    void StopCollection();
    int GetPeak();
    void SetPeak(int peak);
}