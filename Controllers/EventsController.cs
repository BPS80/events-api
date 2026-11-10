using EventsApi.DTOs;
using EventsApi.Models;
using EventsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventsApi.Controllers;

[ApiController]
[Route("events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public ActionResult<List<EventResponse>> GetAll()
    {
        var events = _eventService.GetAll()
            .Select(ToResponse)
            .ToList();

        return Ok(events);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<EventResponse> GetById(Guid id)
    {
        var existingEvent = _eventService.GetById(id);

        if (existingEvent is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(existingEvent));
    }

    [HttpPost]
    public ActionResult<EventResponse> Create(EventRequest request)
    {
        var createdEvent = _eventService.Create(ToModel(request));

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdEvent.Id },
            ToResponse(createdEvent));
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, EventRequest request)
    {
        var updated = _eventService.Update(id, ToModel(request));

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _eventService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static Event ToModel(EventRequest request)
    {
        return new Event
        {
            Title = request.Title!,
            Description = request.Description,
            StartAt = request.StartAt!.Value,
            EndAt = request.EndAt!.Value
        };
    }

    private static EventResponse ToResponse(Event source)
    {
        return new EventResponse
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            StartAt = source.StartAt,
            EndAt = source.EndAt
        };
    }
}
