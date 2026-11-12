using DevTech.Shared.Domain.Model.Events;

namespace DevTech.CustomerManagement.Domain.Model.Events;

public record LaundryCreated(Guid LaundryId) : IEvent;