using BlogsProject.Domain.Interfaces;

namespace BlogsProject.Application.Messaging;

using System.Collections.Concurrent;

public class LocalMessageBus
{
    private readonly ConcurrentDictionary<
        Type,
        List<Func<IDomainEvent, Task>>
    > _handlers = new();

    public void Subscribe<TEvent>(Func<TEvent, Task> handler)
        where TEvent : IDomainEvent
    {
        var type = typeof(TEvent);

        if (!_handlers.ContainsKey(type))
            _handlers[type] = new List<Func<IDomainEvent, Task>>();

        _handlers[type].Add(async (e) =>
        {
            await handler((TEvent)e);
        });
    }

    public async Task PublishAsync(IDomainEvent domainEvent)
    {
        var type = domainEvent.GetType();

        if (_handlers.TryGetValue(type, out var handlers))
        {
            foreach (var handler in handlers)
            {
                await handler(domainEvent);
            }
        }
    }
}