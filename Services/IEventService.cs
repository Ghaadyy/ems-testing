namespace EMS.Services;

using EMS.Models;

public interface IEventService
{
    /// <summary>
    /// Creates a new event.
    /// </summary>
    /// <param name="event">The event to be created.</param>
    /// <returns>The ID of the newly created event.</returns>
    int CreateEvent(Event @event);

    /// <summary>
    /// Retrieves an event by its ID.
    /// </summary>
    /// <param name="eventId">The ID of the event to retrieve.</param>
    /// <returns>The event object, or null if not found.</returns>
    Event GetEventById(int eventId);

    /// <summary>
    /// Checks if tickets are available for a specific event.
    /// </summary>
    /// <param name="eventId">The ID of the event to check.</param>
    /// <returns>True if tickets are available, otherwise false.</returns>
    bool AreTicketsAvailable(int eventId, int ticketsRequested);

    /// <summary>
    /// Updates the number of available tickets for an event.
    /// </summary>
    /// <param name="eventId">The ID of the event to update.</param>
    /// <param name="ticketsRequested">The number of tickets to reduce.</param>
    /// <returns>True if the update was successful, otherwise false.</returns>
    bool UpdateAvailableTickets(int eventId, int ticketsRequested);
}
