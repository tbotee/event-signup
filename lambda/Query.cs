using EventSignup.Lambda.Models;
using HotChocolate.Authorization;

public class Query
{
    public IEnumerable<Event> ListEvents() => new[]
    {
        new Event { Id = "1", Name = "Lego Robotics", Date = "2025-07-01", MaxAttendees = 10 },
        new Event { Id = "2", Name = "Lego Architecture", Date = "2025-07-15", MaxAttendees = 10 }
    };

    [Authorize]
    public IEnumerable<Event> ListParticipants() => new[]
    {
        new Event { Id = "1", Name = "Lego Robotics", Date = "2025-07-01", MaxAttendees = 10 },
        new Event { Id = "2", Name = "Lego Architecture", Date = "2025-07-15", MaxAttendees = 10 }
    };

    [Authorize]
    public Event? GetEventById(string id) =>
        ListEvents().FirstOrDefault(e => e.Id == id);
}