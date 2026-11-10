using DevTech.LaundryOperations.Domain.Model.ValueObjects;
namespace DevTech.LaundryOperations.Domain.Model.Entities;

public class WashingCycle
{
    private WashingCycle() { }
    public WashingCycle(string name, int durationMinutes, string compatibleType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(compatibleType);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(durationMinutes);
        Id = new(Guid.NewGuid());
        Name = name.Trim();
        DurationMinutes = durationMinutes;
        CompatibleType = compatibleType.Trim();
    }
    public WashingCycleId Id { get; private set; }
    public string Name { get; private set; } = "";
    public int DurationMinutes { get; private set; }
    public string CompatibleType { get; private set; } = "";
}

