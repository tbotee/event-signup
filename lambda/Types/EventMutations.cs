using AppAny.HotChocolate.FluentValidation;
using EventSignup.Services;
using GreenDonut.Data;
using HotChocolate.Authorization;

namespace EventSignup.Types;

[ExtendObjectType(typeof(Mutation))]
public class EventMutations
{

	//[Authorize]
	public async Task<Event> CreateEventAsync(
		[UseFluentValidation] CreateEventInput input,
		CancellationToken cancellationToken,
		IEventService eventService,
		IEventByIdDataLoader eventById)
	{
		var createdEventId = await eventService.CreateEventAsync(input, cancellationToken);
		return await eventById
			.LoadRequiredAsync(createdEventId, cancellationToken);
	}

	[Error(typeof(NotFoundException))]
	public async Task<Event> UpdateEventAsync(
		[UseFluentValidation] UpdateEventInput input,
		CancellationToken cancellationToken,
		IEventService eventService,
		IEventByIdDataLoader eventById)
	{
		var eventId = await eventService.UpdateEventAsync(input, cancellationToken);
		return await eventById
			.LoadRequiredAsync(eventId, cancellationToken);
	}

	[Error(typeof(NotFoundException))]
	public async Task<int> DeleteEventAsync(
		int eventId,
		CancellationToken cancellationToken,
		IEventService eventService)
		=> await eventService.DeleteEventAsync(eventId, cancellationToken);
}
