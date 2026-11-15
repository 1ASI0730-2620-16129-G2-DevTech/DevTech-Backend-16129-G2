using DevTech.Shared.Domain.Model.Events;

namespace DevTech.OrderManagement.Domain.Model.Events;

public sealed record OrderCreated(Guid OrderId) : IEvent;
