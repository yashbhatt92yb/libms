namespace ABCLibrary.Domain.Entities;

public sealed class SystemLog : BaseEntity
{
    public required string Action { get; set; }
    public required string Description { get; set; }
    public string? Username { get; set; }
    public string? MetadataJson { get; set; }
}
