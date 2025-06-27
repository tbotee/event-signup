using EventSignup.Data;
using EventSignup.Types;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;

namespace EventSignup.Services;

public static class ParticipantDataLoader
{
	[DataLoader]
	public static async Task<Dictionary<int, Participant>> GetParticipantByIdAsync(
			IReadOnlyList<int> ids,
			QueryContext<Participant> queryContext,
			CancellationToken cancellationToken,
            EventSignupContext dbContext)
		=> await dbContext.Participants
				.Where(p => ids.Contains(p.Id))
				.Map()
				.With(queryContext.Include(p => p.Id))
				.ToDictionaryAsync(p => p.Id, cancellationToken);

	[DataLoader]
	public static async Task<Dictionary<int, Page<Participant>>> GetParticipantsByEventIdAsync(
			IReadOnlyList<int> ids,
			PagingArguments pagingArguments,
			QueryContext<Participant> queryContext,
			CancellationToken cancellationToken,
            EventSignupContext dbContext)
		=> await dbContext.Participants
				.Where(p => ids.Contains(p.EventId))
				.Map()
				.With(queryContext, sd => sd.AddAscending(p => p.Id))
				.ToBatchPageAsync(p => p.EventId, pagingArguments, cancellationToken);
}
