using api.plugin;
using api.plugin.services;
using Microsoft.Extensions.Logging;
using plugin.models;
using System.Text.Json.Nodes;

namespace plugin.services;
public class MapGroupService : IMapGroupService
{
    private IEvent baseEvent;
    private IAnnouncer announcer;
    List<MapGroup> mapGroupsList = new();

    public MapGroupService(IEvent baseEvent)
    {
        this.baseEvent = baseEvent;
        announcer = baseEvent.getAnnouncer();
        mapGroupsList = GetMapGroupsFromJson();
    }
    public List<MapGroup> GetMapGroupsFromJson()
    {
        if (baseEvent.Config.MapGroups == null)
        {
            baseEvent.getBase().Logger.LogError("Unable to Fetch map groups from Json");
            return new List<MapGroup>();
        }
        var mapGroups = new List<MapGroup>();

        var mapGroupsJson = baseEvent.Config.MapGroups["Map Groups"]?.AsObject();
        if (mapGroupsJson != null)
        {
            foreach (var mapGroup in mapGroupsJson)
            {
                var mapGroupValue = mapGroup.Value?.AsObject();
                if (mapGroupValue != null)
                {
                    var mapGroupObj = new MapGroup
                    {
                        Name = mapGroup.Key,
                    };

                    foreach (var map in mapGroupValue)
                    {
                        var mapValue = map.Value?.AsObject();
                        if (mapValue != null)
                        {
                            var mapObj = new Map
                            {
                                Name = mapValue["name"]?.GetValue<string>() ?? string.Empty,
                                WorkshopId = long.TryParse(mapValue["id"]?.GetValue<string>(), out long workshopId) ? workshopId : -1
                            };
                            mapGroupObj.Maps.Add(mapObj);
                        }
                    }

                    mapGroups.Add(mapGroupObj);
                }
            }
        }
        return mapGroups;
    }
}
