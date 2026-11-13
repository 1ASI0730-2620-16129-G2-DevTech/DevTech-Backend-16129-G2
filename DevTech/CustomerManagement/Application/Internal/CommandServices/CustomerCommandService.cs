using DevTech.CustomerManagement.Application.CommandServices;
using DevTech.CustomerManagement.Domain.Model.Aggregates;
using DevTech.CustomerManagement.Domain.Model.Commands;
using DevTech.CustomerManagement.Domain.Model.Errors;
using DevTech.CustomerManagement.Domain.Model.ValueObjects;
using DevTech.CustomerManagement.Domain.Repositories;
using DevTech.Shared.Application.Model;
using DevTech.Shared.Domain.Model.Errors;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.CustomerManagement.Application.Internal.CommandServices;

public class CustomerCommandService(
    ICustomerRepository customers,
    IUnitOfWork unitOfWork) : ICustomerCommandService
{
    public async Task<Result<Customer>> Handle(CreateCustomerCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CustomerId.Value == Guid.Empty)
            return Result<Customer>.Failure(
                CustomerManagementErrors.InvalidCustomerId);

        var error = ValidateProfile(
            command.Name,
            command.PhoneNumber,
            command.Address);

        if (error != Error.None)
            return Result<Customer>.Failure(error);

        if (string.IsNullOrWhiteSpace(command.Document.Value))
            return Result<Customer>.Failure(
                Error.Validation(
                    "CustomerManagement.InvalidDocumentNumber",
                    "Document number is required."));

        if (!Enum.IsDefined(command.Document.Type))
            return Result<Customer>.Failure(
                CustomerManagementErrors.InvalidDocumentType);

        var existingCustomer = await customers.FindByIdAsync(
            command.CustomerId);

        if (existingCustomer is not null)
            return Result<Customer>.Failure(
                CustomerManagementErrors.DuplicateCustomer);

        var customerWithDocument = await customers.FindByDocumentAsync(
            command.Document,
            CancellationToken.None);

        if (customerWithDocument is not null)
            return Result<Customer>.Failure(
                CustomerManagementErrors.DuplicateCustomer);

        var customer = new Customer(command);

        await customers.AddAsync(customer);
        await unitOfWork.CompleteAsync();

        return Result<Customer>.Success(customer);
    }

    public async Task<Result<Customer>> Handle(
        UpdateCustomerProfileCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CustomerId.Value == Guid.Empty)
            return Result<Customer>.Failure(
                CustomerManagementErrors.InvalidCustomerId);

        var error = ValidateProfile(
            command.Name,
            command.PhoneNumber,
            command.Address);

        if (error != Error.None)
            return Result<Customer>.Failure(error);

        var customer = await customers.FindByIdAsync(command.CustomerId);

        if (customer is null)
            return Result<Customer>.Failure(
                CustomerManagementErrors.CustomerNotFound);

        customer.UpdateProfile(
            command.Name,
            command.PhoneNumber,
            command.Address);

        // FindByIdAsync obtiene la entidad con seguimiento,
        // igual que en LaundryOrderCommandService.
        await unitOfWork.CompleteAsync();

        return Result<Customer>.Success(customer);
    }

    private static Error ValidateProfile(
        PersonName name,
        PhoneNumber phoneNumber,
        StreetAddress address)
    {
        if (name is null || string.IsNullOrWhiteSpace(name.FirstName))
            return CustomerManagementErrors.InvalidCustomerName;

        if (string.IsNullOrWhiteSpace(phoneNumber.Value))
            return CustomerManagementErrors.InvalidPhoneNumber;

        if (address is null || string.IsNullOrWhiteSpace(address.Street))
            return CustomerManagementErrors.InvalidAddress;

        return Error.None;
    }
}