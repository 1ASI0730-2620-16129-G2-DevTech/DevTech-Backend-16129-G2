using DevTech.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace DevTech.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}