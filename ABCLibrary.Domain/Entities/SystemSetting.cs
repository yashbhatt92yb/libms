namespace ABCLibrary.Domain.Entities;

public sealed class SystemSetting : BaseEntity
{
    public required string Key { get; set; }
    public required string Value { get; set; }
}
