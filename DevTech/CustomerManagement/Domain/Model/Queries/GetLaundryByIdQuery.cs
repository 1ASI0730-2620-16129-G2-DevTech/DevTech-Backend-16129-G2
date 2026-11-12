using DevTech.CustomerManagement.Domain.Model.ValueObjects;

namespace DevTech.CustomerManagement.Domain.Model.Queries;

public record GetLaundryByIdQuery(LaundryId LaundryId)
{
    public GetLaundryByIdQuery(Guid laundryId) : this(new LaundryId(laundryId))
    {
    }
}