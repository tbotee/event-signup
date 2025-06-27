using EventSignup.Types;
using GreenDonut.Data;

namespace EventSignup.Services
{
	public interface IEventService
	{
		Task<int> CreateEventAsync(CreateEventInput input, CancellationToken cancellationToken);
		Task<int> DeleteEventAsync(int eventId, CancellationToken cancellationToken);
		Task<Page<Event>> GetEventsAsync(PagingArguments pagingArguments, QueryContext<Event>? queryContext = null, CancellationToken cancellationToken = default);
		Task<int> UpdateEventAsync(UpdateEventInput input, CancellationToken cancellationToken);
	}
}