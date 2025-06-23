using EventSignup.Data;
using EventSignup.Models;
using EventSignup.Services;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EventSignup
{
    public class Query(ILogger<Query> logger)
    {
        public async Task<IEnumerable<Event>> ListEvents([Service] IDatabaseService databaseService)
        {
            try
            {
                logger.LogInformation("Getting all events from database");
                
                return await databaseService.GetEventsAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to list events");
                return new List<Event>
                {
                    new Event { Id = 1, Name = $"Error: {ex.Message}", Date = DateTime.Now, MaxAttendees = 100 }
                };
            }
        }

        // [Authorize]
        // public async Task<IEnumerable<Participant>> ListParticipants(int eventId)
        // {
        //     return await databaseService.GetEventParticipantsAsync(eventId);
        // }
    }
}