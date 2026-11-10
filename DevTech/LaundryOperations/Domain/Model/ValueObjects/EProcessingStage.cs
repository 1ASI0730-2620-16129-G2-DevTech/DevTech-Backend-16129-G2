namespace DevTech.LaundryOperations.Domain.Model.ValueObjects;

/// <summary>
///     Represents the processing stage of a job.
/// </summary>
public enum EProcessingStage
{
    RECEPTION,
    CLASSIFICATION,
    WASHING,
    DRYING_IRONING,
    PACKAGING,
    READY
}
