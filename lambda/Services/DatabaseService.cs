using EventSignup.Data;
using EventSignup.Models;
using Microsoft.EntityFrameworkCore;

namespace EventSignup.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly EventSignupContext _context;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(EventSignupContext context, ILogger<DatabaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            try
            {
                return await _context.Events
                    .OrderBy(e => e.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get events");
                throw;
            }
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            try
            {
                return await _context.Events
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get event with id {EventId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Participant>> GetEventParticipantsAsync(int eventId)
        {
            try
            {
                return await _context.Participants
                    .Where(p => p.EventId == eventId)
                    .OrderBy(p => p.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get participants for event {EventId}", eventId);
                throw;
            }
        }

        public async Task<Participant?> GetParticipantAsync(int eventId, string email)
        {
            try
            {
                return await _context.Participants
                    .FirstOrDefaultAsync(p => p.EventId == eventId && p.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get participant for event {EventId} with email {Email}", eventId, email);
                throw;
            }
        }

        public async Task<Participant> CreateParticipantAsync(Participant participant)
        {
            try
            {
                _context.Participants.Add(participant);
                await _context.SaveChangesAsync();
                return participant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create participant for event {EventId} with email {Email}", participant.EventId, participant.Email);
                throw;
            }
        }

        public async Task<bool> DeleteParticipantAsync(int eventId, string email)
        {
            try
            {
                var participant = await _context.Participants
                    .FirstOrDefaultAsync(p => p.EventId == eventId && p.Email == email);

                if (participant == null)
                    return false;

                _context.Participants.Remove(participant);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete participant for event {EventId} with email {Email}", eventId, email);
                throw;
            }
        }

        public async Task<int> GetEventParticipantCountAsync(int eventId)
        {
            try
            {
                return await _context.Participants
                    .CountAsync(p => p.EventId == eventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get participant count for event {EventId}", eventId);
                throw;
            }
        }

        public async Task<Participant> UpdateParticipantAsync(Participant participant)
        {
            try
            {
                _context.Participants.Update(participant);
                await _context.SaveChangesAsync();
                return participant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update participant with id {ParticipantId}", participant.Id);
                throw;
            }
        }
    }
} 