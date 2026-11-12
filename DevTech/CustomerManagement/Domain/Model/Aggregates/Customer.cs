using System;
using DevTech.CustomerManagement.Domain.Model.Commands;
using DevTech.CustomerManagement.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Aggregates.Internal;

namespace DevTech.CustomerManagement.Domain.Model.Aggregates;

public partial class Customer : AggregateRoot
{
    public Customer(CreateCustomerCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CustomerId.Value == Guid.Empty)
        {
            throw new ArgumentException(
                "CustomerId must be non-empty.",
                nameof(command.CustomerId));
        }

        ValidateProfile(
            command.Name,
            command.PhoneNumber,
            command.Address);

        if (string.IsNullOrWhiteSpace(command.Document.Value))
            throw new ArgumentException(
                "Document is required.", nameof(command.Document));

        Document = command.Document;
        
        Id = command.CustomerId;
        Name = command.Name;
        PhoneNumber = command.PhoneNumber;
        Document = command.Document;
        Address = command.Address;
    }
    
    public CustomerId Id { get; private set; }
    public PersonName Name { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public DocumentNumber Document { get; private set; } = default!;
    public StreetAddress Address { get; private set; }

    public void UpdateProfile(
        PersonName name,
        PhoneNumber phoneNumber,
        StreetAddress address)
    {
        ValidateProfile(name, phoneNumber, address);

        Name = name;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    private static void ValidateProfile(
        PersonName name,
        PhoneNumber phoneNumber,
        StreetAddress address)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(phoneNumber);
        ArgumentNullException.ThrowIfNull(address);
    }
}