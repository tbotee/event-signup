using EventSignup.Models;

namespace EventSignup.Services
{
    public interface IDatabaseService
    {
        Task<IEnumerable<Event>> GetEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<IEnumerable<Participant>> GetEventParticipantsAsync(int eventId);
        Task<Participant?> GetParticipantAsync(int eventId, string email);
        Task<Participant> CreateParticipantAsync(Participant participant);
        Task<bool> DeleteParticipantAsync(int eventId, string email);
        Task<int> GetEventParticipantCountAsync(int eventId);
        Task<Participant> UpdateParticipantAsync(Participant participant);
    }
} 