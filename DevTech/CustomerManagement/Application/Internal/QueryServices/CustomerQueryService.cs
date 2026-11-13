using DevTech.CustomerManagement.Application.QueryServices;
using DevTech.CustomerManagement.Domain.Model.Aggregates;
using DevTech.CustomerManagement.Domain.Model.Queries;
using DevTech.CustomerManagement.Domain.Repositories;

namespace DevTech.CustomerManagement.Application.Internal.QueryServices;

public class CustomerQueryService(
    ICustomerRepository customers) : ICustomerQueryService
{
    public Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery query)
    {
        return customers.ListAsNoTrackingAsync();
    }

    public Task<Customer?> Handle(GetCustomerByIdQuery query)
    {
        return customers.FindByIdAsNoTrackingAsync(query.CustomerId);
    }
}