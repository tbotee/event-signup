using EventSignup.Data;
using EventSignup.Types;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;

namespace EventSignup.Services
{
	public class EventService(EventSignupContext _dbContext) : IEventService
	{
		public async Task<Page<Event>> GetEventsAsync(
			PagingArguments pagingArguments,
			QueryContext<Event>? queryContext = default,
			CancellationToken cancellationToken = default)
			=> await _dbContext.Events
				.Map()
				.With(queryContext, sd => sd.AddAscending(n => n.Id))
				.ToPageAsync(pagingArguments, cancellationToken);

		public async Task<int> CreateEventAsync(
			CreateEventInput input,
			CancellationToken cancellationToken)
		{
			var @event = input.Map();
			_dbContext.Events.Add(@event);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return @event.Id;
		}

		public async Task<int> UpdateEventAsync(
			UpdateEventInput input,
			CancellationToken cancellationToken)
		{
			var @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == input.Id, cancellationToken);
			if (@event == null)
			{
				throw new NotFoundException(nameof(Event), input.Id);
			}

			if (input.Name.HasValue)
			{
				@event.Name = input.Name.Value;
			}
			if (input.Date.HasValue)
			{
				@event.Date = input.Date.Value.Value;
			}
			if (input.MaxAttendees.HasValue)
			{
				@event.MaxAttendees = input.MaxAttendees.Value.Value;
			}

			_dbContext.Events.Update(@event);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return @event.Id;
		}

		public async Task<int> DeleteEventAsync(
			int eventId,
			CancellationToken cancellationToken)
		{
			var @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
			if (@event == null)
			{
				throw new NotFoundException(nameof(Event), eventId);
			}

			_dbContext.Events.Remove(@event);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return eventId;
		}
	}
}
