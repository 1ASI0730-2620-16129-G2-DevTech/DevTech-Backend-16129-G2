using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Queries;

public record GetAllLaundryResourcesByLaundryIdQuery(LaundryId LaundryId)
{
    public GetAllLaundryResourcesByLaundryIdQuery(Guid laundryId) : this(new LaundryId(laundryId))
    {
        
    }
}
