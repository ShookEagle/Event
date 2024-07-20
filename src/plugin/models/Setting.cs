namespace plugin.models;
public class Setting
{
    public string Name { get; set; } = String.Empty;
    public string Stem { get; set; } = String.Empty;
    public bool Default { get; set; }
    public bool IsActive { get; set; }
}
