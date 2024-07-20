namespace plugin.models;

public class MapGroup
{
    public string Name { get; set; } = String.Empty;
    public List<Map> Maps { get; set; } = new List<Map>();
}
