using DevTech.CustomerManagement.Domain.Model.Aggregates;
using DevTech.CustomerManagement.Domain.Model.Queries;

namespace DevTech.CustomerManagement.Application.QueryServices;

public interface ICustomerQueryService
{
    Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery query);
    
    Task<Customer?> Handle(GetCustomerByIdQuery query);
}