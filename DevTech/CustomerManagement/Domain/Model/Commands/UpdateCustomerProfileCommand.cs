using DevTech.CustomerManagement.Domain.Model.ValueObjects;

namespace DevTech.CustomerManagement.Domain.Model.Commands;

public record UpdateCustomerProfileCommand(
    CustomerId CustomerId,
    PersonName Name,
    PhoneNumber PhoneNumber,
    StreetAddress Address)
{
    public UpdateCustomerProfileCommand(Guid customerId, PersonName name, PhoneNumber phoneNumber, StreetAddress address)
        : this(new CustomerId(customerId), name, phoneNumber, address)
    {
    }
}