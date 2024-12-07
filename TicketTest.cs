using EMS.Models;
using EMS.Services;
using Moq;

namespace EMS.Tests;

public class TicketTest
{
    private readonly Mock<IEventService> _mockEventService;
    private readonly TicketService _ticketService;

    public TicketTest()
    {
        _mockEventService = new Mock<IEventService>();
        _ticketService = new TicketService(_mockEventService.Object);
    }

    [Fact]
    public void PurchaseTickets_ShouldReturnPurchasedTickets_WhenValidRequest()
    {
        var userId = 1;
        var eventId = 101;
        var ticketsRequested = 3;

        var mockEvent = new Event(
            Id: eventId,
            Title: "Concert",
            Date: DateTime.Now.AddDays(1),
            AvailableTickets: 10
        );

        _mockEventService.Setup(es => es.GetEventById(eventId)).Returns(mockEvent);
        _mockEventService.Setup(es => es.AreTicketsAvailable(eventId, ticketsRequested)).Returns(true);

        var purchasedTickets = _ticketService.PurchaseTickets(userId, eventId, ticketsRequested);

        Assert.NotNull(purchasedTickets);
        Assert.Equal(ticketsRequested, purchasedTickets.Count);
        Assert.All(purchasedTickets, ticket => Assert.Equal(userId, ticket.UserId));
        Assert.All(purchasedTickets, ticket => Assert.Equal(eventId, ticket.EventId));
    }

    [Fact]
    public void PurchaseTickets_ShouldThrowInvalidOperationException_WhenEventIsInThePast()
    {
        var userId = 1;
        var eventId = 101;
        var ticketsRequested = 3;

        var mockEvent = new Event(
            Id: eventId,
            Title: "Concert",
            Date: DateTime.Now.AddDays(-1),
            AvailableTickets: 10
        );

        _mockEventService.Setup(es => es.GetEventById(eventId)).Returns(mockEvent);

        var exception = Assert.Throws<InvalidOperationException>(() => 
            _ticketService.PurchaseTickets(userId, eventId, ticketsRequested));
        
        Assert.Equal("Cannot purchase tickets for past events.", exception.Message);
    }

    [Fact]
    public void PurchaseTickets_ShouldThrowInvalidOperationException_WhenNotEnoughTicketsAvailable()
    {
        var userId = 1;
        var eventId = 101;
        var ticketsRequested = 5;

        var mockEvent = new Event(
            Id: eventId,
            Title: "Concert",
            Date: DateTime.Now.AddDays(1),
            AvailableTickets: 3
        );

        _mockEventService.Setup(es => es.GetEventById(eventId)).Returns(mockEvent);
        _mockEventService.Setup(es => es.AreTicketsAvailable(eventId, ticketsRequested)).Returns(false);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            _ticketService.PurchaseTickets(userId, eventId, ticketsRequested));

        Assert.Equal("Not enough tickets available.", exception.Message);
    }

    [Fact]
    public void GetTicketById_ShouldReturnTicket_WhenTicketExists()
    {
        var ticketId = 1;
        var ticket = new Ticket(
            Id: ticketId,
            EventId: 101,
            UserId: 1,
            PurchaseDate: DateTime.Now
        );

        _ticketService._tickets.Add(ticket);

        var fetchedTicket = _ticketService.GetTicketById(ticketId);

        Assert.NotNull(fetchedTicket);
        Assert.Equal(ticketId, fetchedTicket.Id);
    }

    [Fact]
    public void GetTicketById_ShouldThrowKeyNotFoundException_WhenTicketDoesNotExist()
    {
        var ticketId = 999;

        var exception = Assert.Throws<KeyNotFoundException>(() =>
            _ticketService.GetTicketById(ticketId));

        Assert.Equal("Ticket not found.", exception.Message);
    }
}