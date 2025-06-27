using EventSignup.Services;
using GreenDonut.Data;

namespace EventSignup.Types;

public class Participant
{
	public int Id { get; set; }

	public int EventId { get; set; }

	public required string Name { get; set; }

	public required string Email { get; set; }

	public async Task<Event> GetEventAsync(
		[Parent(requires: nameof(EventId))] Participant parent,
		QueryContext<Event> queryContext,
		CancellationToken cancellationToken,
		IEventByIdDataLoader eventById)
		=> await eventById
			.With(queryContext)
			.LoadRequiredAsync(parent.EventId, cancellationToken);
}
