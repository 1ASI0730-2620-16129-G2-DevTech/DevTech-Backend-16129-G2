namespace DevTech.LaundryOperations.Domain.Model.Queries;

public record GetAllLaundryResourcesByLaundryIdQuery(LaundryId LaundryId)
{
    public GetAllLaundryResourcesByLaundryIdQuery(int laundryId) : this(new LaundryId(laundryId))
    {
        
    }
}