using plugin.models;

namespace api.plugin.services;

public interface IMapGroupService
{
    MapGroup FetchCurrentGroup();
    void SetMapGroup(string groupId);
    void ChangeMap(Map map);
}
