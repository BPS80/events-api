using EventsApi.Models;

namespace EventsApi.Services;

public interface IEventService
{
    IReadOnlyList<Event> GetAll();

    Event? GetById(Guid id);

    Event Create(Event newEvent);

    bool Update(Guid id, Event updatedEvent);

    bool Delete(Guid id);
}
