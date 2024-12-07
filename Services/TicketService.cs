using EMS.Models;

namespace EMS.Services;

public class TicketService(IEventService eventService) : ITicketService
{
    internal readonly List<Ticket> _tickets = [];
    private readonly IEventService _eventService = eventService;
    private readonly object _lock = new();

    public List<Ticket> PurchaseTickets(int userId, int eventId, int ticketsRequested)
    {
        lock (_lock)
        {
            var @event = _eventService.GetEventById(eventId);

            if (@event.Date < DateTime.Now)
                throw new InvalidOperationException("Cannot purchase tickets for past events.");

            if (!_eventService.AreTicketsAvailable(eventId, ticketsRequested))
                throw new InvalidOperationException("Not enough tickets available.");

            _eventService.UpdateAvailableTickets(eventId, @event.AvailableTickets - ticketsRequested);

            var newTickets = new List<Ticket>();
            for (int i = 0; i < ticketsRequested; i++)
            {
                var newTicket = new Ticket(
                    Id: _tickets.Count + 1,
                    EventId: eventId,
                    UserId: userId,
                    PurchaseDate: DateTime.Now
                );

                _tickets.Add(newTicket);
                newTickets.Add(newTicket);
            }

            return newTickets;
        }
    }

    public Ticket GetTicketById(int ticketId)
    {
        return _tickets.FirstOrDefault(t => t.Id == ticketId)
            ?? throw new KeyNotFoundException("Ticket not found.");
    }
}
