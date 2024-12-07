namespace EMS;

using EMS.Models;
using EMS.Services;

public class EventTests
{
    private readonly EventService _eventService;

    public EventTests() => _eventService = new EventService();

    [Fact]
    public void CreateEvent_ShouldCreateEventSuccessfully()
    {
        var newEvent = new Event(0, "Tech Conference 2024", DateTime.Now.AddDays(30), 100);

        int eventId = _eventService.CreateEvent(newEvent);

        Assert.True(eventId > 0, "Event ID should be greater than 0.");
        var createdEvent = _eventService.GetEventById(eventId);
        Assert.Equal("Tech Conference 2024", createdEvent.Title);
        Assert.Equal(100, createdEvent.AvailableTickets);
    }

    [Fact]
    public void CreateEvent_WithPastDate_ShouldThrowException()
    {
        var pastEvent = new Event(0, "Old Event", DateTime.Now.AddDays(-1), 50);

        Assert.Throws<InvalidOperationException>(() => _eventService.CreateEvent(pastEvent));
    }

    [Fact]
    public void GetEventById_ShouldReturnEvent_WhenEventExists()
    {
        var newEvent = new Event(0, "Tech Conference 2024", DateTime.Now.AddDays(30), 100);
        int eventId = _eventService.CreateEvent(newEvent);

        var fetchedEvent = _eventService.GetEventById(eventId);

        Assert.NotNull(fetchedEvent);
        Assert.Equal("Tech Conference 2024", fetchedEvent.Title);
    }

    [Fact]
    public void GetEventById_ShouldThrowException_WhenEventDoesNotExist()
    {
        Assert.Throws<KeyNotFoundException>(() => _eventService.GetEventById(999));
    }

    [Fact]
    public void AreTicketsAvailable_ShouldReturnTrue_WhenTicketsAreAvailable()
    {
        var newEvent = new Event(0, "Tech Conference 2024", DateTime.Now.AddDays(30), 100);
        int eventId = _eventService.CreateEvent(newEvent);

        bool result = _eventService.AreTicketsAvailable(eventId, 10);

        Assert.True(result, "Tickets should be available.");
    }

    [Fact]
    public void AreTicketsAvailable_ShouldReturnFalse_WhenTicketsRequestedExceedAvailable()
    {
        var newEvent = new Event(0, "Tech Conference 2024", DateTime.Now.AddDays(30), 100);
        int eventId = _eventService.CreateEvent(newEvent);

        bool result = _eventService.AreTicketsAvailable(eventId, 200);

        Assert.False(result, "Tickets should not be available when requested amount exceeds availability.");
    }

    [Fact]
    public void UpdateAvailableTickets_ShouldReduceAvailableTicketsSuccessfully()
    {
        var newEvent = new Event(0, "Tech Conference 2024", DateTime.Now.AddDays(30), 100);
        int eventId = _eventService.CreateEvent(newEvent);

        bool updateResult = _eventService.UpdateAvailableTickets(eventId, 10);
        var updatedEvent = _eventService.GetEventById(eventId);

        Assert.True(updateResult, "Tickets should be reduced successfully.");
        Assert.Equal(90, updatedEvent.AvailableTickets);
    }

    [Fact]
    public void UpdateAvailableTickets_ShouldThrowException_WhenNotEnoughTicketsAvailable()
    {
        var newEvent = new Event(0, "Tech Conference 2024", DateTime.Now.AddDays(30), 100);
        int eventId = _eventService.CreateEvent(newEvent);

        Assert.Throws<InvalidOperationException>(() => _eventService.UpdateAvailableTickets(eventId, 200));
    }

    [Fact]
    public void UpdateAvailableTickets_ShouldThrowException_WhenEventDoesNotExist()
    {
        Assert.Throws<KeyNotFoundException>(() => _eventService.UpdateAvailableTickets(999, 10));
    }

    [Fact]
    public void AreTicketsAvailable_ShouldThrowException_WhenRequestedTicketsAreInvalid()
    {
        var newEvent = new Event(0, "Tech Conference 2024", DateTime.Now.AddDays(30), 100);
        int eventId = _eventService.CreateEvent(newEvent);

        Assert.Throws<ArgumentException>(() => _eventService.AreTicketsAvailable(eventId, -5));
    }
}
