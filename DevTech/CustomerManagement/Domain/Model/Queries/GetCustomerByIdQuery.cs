using DevTech.CustomerManagement.Domain.Model.ValueObjects;

namespace DevTech.CustomerManagement.Domain.Model.Queries;

public record GetCustomerByIdQuery(CustomerId CustomerId)
{
    public GetCustomerByIdQuery(Guid customerId) : this(new CustomerId(customerId))
    {
    }
}