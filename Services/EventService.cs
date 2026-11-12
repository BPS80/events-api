using EventsApi.Models;

namespace EventsApi.Services;

public class EventService : IEventService
{
    private readonly List<Event> _events = new();
    private readonly object _lock = new();

    public IReadOnlyList<Event> GetAll()
    {
        lock (_lock)
        {
            return _events.Select(Copy).ToList();
        }
    }

    public Event? GetById(Guid id)
    {
        lock (_lock)
        {
            var existingEvent = _events.Find(e => e.Id == id);

            return existingEvent is null ? null : Copy(existingEvent);
        }
    }

    public Event Create(Event newEvent)
    {
        lock (_lock)
        {
            var createdEvent = Copy(newEvent);
            createdEvent.Id = Guid.NewGuid();

            _events.Add(createdEvent);

            return Copy(createdEvent);
        }
    }

    public bool Update(Guid id, Event updatedEvent)
    {
        lock (_lock)
        {
            var existingEvent = _events.Find(e => e.Id == id);

            if (existingEvent is null)
            {
                return false;
            }

            existingEvent.Title = updatedEvent.Title;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.StartAt = updatedEvent.StartAt;
            existingEvent.EndAt = updatedEvent.EndAt;

            return true;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            return _events.RemoveAll(e => e.Id == id) > 0;
        }
    }

    private static Event Copy(Event source)
    {
        return new Event
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            StartAt = source.StartAt,
            EndAt = source.EndAt
        };
    }
}
