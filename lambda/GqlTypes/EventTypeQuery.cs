using EventSignup.Models;
using EventSignup.Services;

namespace EventSignup.GqlTypes;

[ExtendObjectType(typeof(Query))]
public class EventTypeQuery
{
    public async Task<IEnumerable<Event>> ListEvents([Service] IDatabaseService databaseService)
    {
        return await databaseService.GetEventsAsync();
    }
}