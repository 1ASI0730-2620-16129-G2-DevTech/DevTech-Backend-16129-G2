using DevTech.CustomerManagement.Domain.Model.Aggregates;
using DevTech.CustomerManagement.Domain.Model.Commands;
using DevTech.Shared.Application.Model;

namespace DevTech.CustomerManagement.Application.CommandServices;

public interface ICustomerCommandService
{
    Task<Result<Customer>> Handle(CreateCustomerCommand command);
    
    Task<Result<Customer>> Handle(UpdateCustomerProfileCommand command);
}