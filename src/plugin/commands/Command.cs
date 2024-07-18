using api.plugin;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace plugin.commands;

public abstract class Command(IEvent eventBase)
{
    protected readonly IEvent eventBase = eventBase;
    public string? Description => null;
    public abstract void OnCommand(CCSPlayerController? executor, CommandInfo info);
}
