using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Queries;

public record GetLaundryOrderByIdQuery(LaundryOrderId LaundryOrderId)
{
    public GetLaundryOrderByIdQuery(Guid laundryOrderId) : this(new LaundryOrderId(laundryOrderId))
    {
    }
}