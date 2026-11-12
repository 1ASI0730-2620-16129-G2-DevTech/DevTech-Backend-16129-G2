using DevTech.CustomerManagement.Domain.Model.ValueObjects;

namespace DevTech.CustomerManagement.Domain.Model.Commands;

public record CreateCustomerCommand(
    CustomerId CustomerId, 
    PersonName Name, 
    PhoneNumber PhoneNumber, 
    DocumentType DocumentType, 
    StreetAddress Address);