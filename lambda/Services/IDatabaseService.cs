using EventSignup.Models;

namespace EventSignup.Services
{
    public interface IDatabaseService
    {
        Task<IEnumerable<Event>> GetEventsAsync();
        Task<Participant> CreateParticipantAsync(Participant participant);
        Task<Event?> GetEventByIdAsync(int id);
        Task<int> GetEventParticipantCountAsync(int eventId);
        Task<IEnumerable<Participant>> GetEventParticipantsAsync(int eventId);
        Task<Participant?> GetParticipantAsync(int eventId, string email);
        Task<bool> DeleteAllParticipantsByEmailAsync(string email);
        Task<Event> CreateEventAsync(Event eventItem);
        Task<Event> UpdateEventAsync(Event eventItem);
        Task<bool> DeleteEventAsync(int eventId);
    }
} 