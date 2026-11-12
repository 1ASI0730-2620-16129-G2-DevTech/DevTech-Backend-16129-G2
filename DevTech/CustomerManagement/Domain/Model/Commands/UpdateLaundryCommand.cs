using DevTech.CustomerManagement.Domain.Model.ValueObjects;

namespace DevTech.CustomerManagement.Domain.Model.Commands;

public record UpdateLaundryCommand(
    LaundryId LaundryId, 
    string Name, 
    StreetAddress Address, 
    PhoneNumber PhoneNumber)
{
    public UpdateLaundryCommand(Guid laundryId, string name, StreetAddress address, PhoneNumber phoneNumber)
        : this(new LaundryId(laundryId), name, address, phoneNumber)
    {
    }   
}