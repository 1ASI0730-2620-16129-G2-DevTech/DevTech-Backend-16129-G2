using DevTech.CustomerManagement.Domain.Model.ValueObjects;

namespace DevTech.CustomerManagement.Domain.Model.Commands;

public record CreateLaundryCommand(string Name, string Address, PhoneNumber PhoneNumber);