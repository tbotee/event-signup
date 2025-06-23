namespace EventSignup.GqlTypes;

using EventSignup.Models;
using EventSignup.Services;
using HotChocolate.Authorization;

[ExtendObjectType(typeof(Query))]
public class ParticipantTypeQuery
{
    //[Authorize]
    public async Task<IEnumerable<Participant>> ListParticipants(int eventId, [Service] IDatabaseService databaseService)
    {
        return await databaseService.GetEventParticipantsAsync(eventId);
    }
}