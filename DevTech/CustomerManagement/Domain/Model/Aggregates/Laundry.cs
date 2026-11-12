using System;
using DevTech.CustomerManagement.Domain.Model.Commands;
using DevTech.CustomerManagement.Domain.Model.Events;
using DevTech.CustomerManagement.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Aggregates.Internal;
using DevTech.Shared.Domain.Model.Errors;

namespace DevTech.CustomerManagement.Domain.Model.Aggregates;

public partial class Laundry : AggregateRoot
{
    private Laundry() { }

    public Laundry(CreateLaundryCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Laundry name is required.");

        if (string.IsNullOrWhiteSpace(command.Address))
            throw new ArgumentException("Address is required.");

        ArgumentNullException.ThrowIfNull(command.PhoneNumber);

        Id = new LaundryId(Guid.NewGuid());
        Name = command.Name.Trim();
        Address = command.Address.Trim();
        PhoneNumber = command.PhoneNumber;

        AddDomainEvent(new LaundryCreated(Id.Value));
    }

    public LaundryId Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;

    public PhoneNumber PhoneNumber { get; private set; } = default!;

    public Guid Version { get; private set; } = Guid.NewGuid();

    public void RefreshVersion() => Version = Guid.NewGuid();

    public Error Update(
        string name,
        string address,
        PhoneNumber phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation(
                "CustomerManagement.InvalidLaundryName",
                "Laundry name is required.");

        if (string.IsNullOrWhiteSpace(address))
            return Error.Validation(
                "CustomerManagement.InvalidAddress",
                "Address is required.");

        if (phoneNumber.Value is null)
            return Error.Validation(
                "CustomerManagement.InvalidPhoneNumber",
                "Phone number is required.");

        Name = name.Trim();
        Address = address.Trim();
        PhoneNumber = phoneNumber;

        AddDomainEvent(new LaundryUpdated(Id.Value));

        return Error.None;
    }
}