using EventSignup.Services;
using GreenDonut.Data;
using HotChocolate.Authorization;
using HotChocolate.Types.Pagination;

namespace EventSignup.Types;

public class Event
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public DateTime Date { get; set; }

	public int MaxAttendees { get; set; } = 0;

	public List<Participant> Participants { get; set; } = new();

	//[Authorize]
	[UsePaging]
	[UseFiltering]
	[UseSorting]
	public async Task<Connection<Participant>> GetParticipantsAsync(
		[Parent(requires: nameof(Id))] Event parent,
		PagingArguments pagingArguments,
		QueryContext<Participant> queryContext,
		CancellationToken cancellationToken,
		IParticipantsByEventIdDataLoader participantsByEventId)
		=> await participantsByEventId
			.With(pagingArguments, queryContext)
			.LoadAsync(parent.Id, cancellationToken)
			.ToConnectionAsync();
}
