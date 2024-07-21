using api.plugin;
using api.plugin.services;
using CounterStrikeSharp.API;
using Microsoft.Extensions.Logging;
using plugin.models;
using System.Text.Json.Nodes;

namespace plugin.services;
public class MapGroupService : IMapGroupService
{
    private IEvent baseEvent;
    private IAnnouncer announcer;
    List<MapGroup> mapGroupsList = new();
    private MapGroup currentGroup;

    public MapGroupService(IEvent baseEvent)
    {
        this.baseEvent = baseEvent;
        announcer = baseEvent.getAnnouncer();
        mapGroupsList = GetMapGroupsFromJson();
        currentGroup = mapGroupsList.FirstOrDefault(g => g.Name == "mg_active")!;
    }

    public MapGroup FetchCurrentGroup()
    {
        return currentGroup;
    }

    public void SetMapGroup(string groupId)
    {
       currentGroup = mapGroupsList.FirstOrDefault(g => g.Name == groupId)!;
    }

    public void ChangeMap(Map map)
    {
        if (map.WorkshopId == -1)
            Server.RunOnTickAsync(Server.TickCount + 128, () => Server.ExecuteCommand($"changelevel {map.Name}"));
        Server.RunOnTickAsync(Server.TickCount + 128, () => Server.ExecuteCommand($"host_workshop_map {map.WorkshopId}"));
    }

    public List<MapGroup> GetMapGroupsFromJson()
    {
        var mapGroups = new List<MapGroup>();
        var mapGroupsJson = baseEvent.Config.MapGroups?.AsObject();

        if (mapGroupsJson == null) return mapGroups;

        foreach (var mapGroup in mapGroupsJson)
        {
            var mapGroupValue = mapGroup.Value?.AsObject();
            var mapsJson = mapGroupValue?["maps"]?.AsObject();

            if (mapGroupValue == null || mapsJson == null) continue;

            var mapGroupObj = new MapGroup { Name = mapGroup.Key };

            foreach (var map in mapsJson)
            {
                var mapValue = map.Value?.AsObject();
                if (mapValue == null) continue;

                var mapObj = new Map
                {
                    Name = mapValue["name"]?.GetValue<string>() ?? string.Empty,
                    WorkshopId = long.TryParse(mapValue["id"]?.GetValue<string>(), out long workshopId) ? workshopId : -1
                };
                mapGroupObj.Maps.Add(mapObj);
            }

            mapGroups.Add(mapGroupObj);
        }

        return mapGroups;
    }
}
