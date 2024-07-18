using plugin.services;
using plugin.commands;
using plugin.commands.status;
using plugin.listeners;
using api.plugin;
using api.plugin.services;
using CounterStrikeSharp.API.Core;

namespace plugin;

public class Event : BasePlugin, IPluginConfig<EventConfig>, IEvent
{
    private readonly Dictionary<string, Command> commands = new();
    public override string ModuleName => "Event";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "ShookEagle";
    public override string ModuleDescription => "eGO Event Server Plugin";

    private IStatusService? status;
    private IAnnouncer? announcer;

    public EventConfig Config { get; set; } = new();
    public void OnConfigParsed(EventConfig config)
    {
        Config = config;
    }

    public BasePlugin getBase()
    {
        return this;
    }

    public IStatusService getStatusService() { return status!; }
    public IAnnouncer getAnnouncer() { return announcer!; }

    public override void Load(bool hotReload)
    {
        _ = new ConnectionListener(this);

        status = new StatusService(this);
        announcer = new AnonymousAnnouncer(this);

        LoadCommands();
    }

    private void LoadCommands()
    {
        commands.Add("css_estart", new EStartCmd(this));
        commands.Add("css_startevent", new EStartCmd(this));
        commands.Add("css_eventstart", new EStartCmd(this));

        commands.Add("css_estatus", new EStatusCmd(this));
        commands.Add("css_ecstatus", new EStatusCmd(this));

        commands.Add("css_estop", new EStopCmd(this));
        commands.Add("css_stopevent", new EStopCmd(this));
        commands.Add("css_endevent", new EStopCmd(this));
        commands.Add("css_eventstop", new EStopCmd(this));

        foreach (var command in commands)
            AddCommand(command.Key, command.Value.Description ?? "No Description Provided", command.Value.OnCommand);
    }
}
