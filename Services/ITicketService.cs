using EMS.Models;

namespace EMS.Services;

public interface ITicketService
{
    /// <summary>
    /// Purchases the requested number of tickets. This method ensures thread safety when tickets are purchased.
    /// </summary>
    /// <param name="userId">The ID of the user purchasing the tickets.</param>
    /// <param name="eventId">The ID of the event for which tickets are being purchased.</param>
    /// <param name="ticketsRequested">The number of tickets to purchase.</param>
    /// <returns>The ID of the newly purchased ticket.</returns>
    List<Ticket> PurchaseTickets(int userId, int eventId, int ticketsRequested);

    /// <summary>
    /// Gets the number of tickets currently available.
    /// </summary>
    /// <returns>The number of tickets available.</returns>
    Ticket GetTicketById(int ticketId);
}
