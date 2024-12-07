namespace EMS.Services;

using EMS.Models;

public class EventService : IEventService
{
    private readonly List<Event> _events = [];

    public int CreateEvent(Event @event)
    {
        if (string.IsNullOrWhiteSpace(@event.Title))
            throw new ArgumentException("Event title is required.");

        if (@event.Date < DateTime.Now)
            throw new InvalidOperationException("Event date must be in the future.");

        var newEvent = @event with { Id = _events.Count + 1 };
        _events.Add(newEvent);

        return newEvent.Id;
    }

    public Event GetEventById(int eventId)
    {
        return _events.FirstOrDefault(e => e.Id == eventId)
            ?? throw new KeyNotFoundException("Event not found.");
    }

    public bool AreTicketsAvailable(int eventId)
    {
        var @event = GetEventById(eventId);
        return @event.AvailableTickets > 0;
    }

    public bool AreTicketsAvailable(int eventId, int ticketsRequested)
    {
        var @event = GetEventById(eventId);

        if (ticketsRequested <= 0)
            throw new ArgumentException("The number of tickets requested must be positive.");

        return @event.AvailableTickets >= ticketsRequested;
    }

    public bool UpdateAvailableTickets(int eventId, int ticketsRequested)
    {
        var @event = GetEventById(eventId);

        if (ticketsRequested <= 0)
            throw new ArgumentException("The number of tickets requested must be positive.");

        if (!AreTicketsAvailable(eventId, ticketsRequested))
            throw new InvalidOperationException("Not enough tickets available.");

        var updatedEvent = @event with { AvailableTickets = @event.AvailableTickets - ticketsRequested };
        var index = _events.IndexOf(@event);

        _events[index] = updatedEvent;
        return true;
    }
}
