using EventSignup.Data;
using EventSignup.Types;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;

namespace EventSignup.Services;

public static class EventDataLoader
{
	[DataLoader]
	public static async Task<Dictionary<int, Event>> GetEventByIdAsync(
			IReadOnlyList<int> ids,
			QueryContext<Event> queryContext,
			CancellationToken cancellationToken,
			EventSignupContext dbContext)
		=> await dbContext.Events
				.Where(e => ids.Contains(e.Id))
				.Map()
				.With(queryContext.Include(e => e.Id))
				.ToDictionaryAsync(e => e.Id, cancellationToken);
}
