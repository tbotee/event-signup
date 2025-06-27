using EventSignup.Services;
using GreenDonut.Data;
using HotChocolate.Authorization;
using HotChocolate.Types.Pagination;

namespace EventSignup.Types;

//[Authorize]
[ExtendObjectType(typeof(Query))]
public static partial class EventResolver
{
	public static async Task<Event?> GetEventAsync(
		int eventId,
		QueryContext<Event> queryContext,
		IEventByIdDataLoader eventById,
		CancellationToken cancellationToken)
		=> await eventById
			.With(queryContext)
			.LoadAsync(eventId, cancellationToken); 

	[UsePaging]
	[UseFiltering]
	[UseSorting]
	public static async Task<Connection<Event>> GetEventsAsync(
		PagingArguments pagingArguments,
		QueryContext<Event> queryContext,
		CancellationToken cancellationToken,
		IEventService eventService)
		=> await eventService
			.GetEventsAsync(pagingArguments, queryContext, cancellationToken)
			.ToConnectionAsync();
}
