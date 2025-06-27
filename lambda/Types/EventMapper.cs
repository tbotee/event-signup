using EventSignup.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace EventSignup.Types;

[Mapper]
public static partial class EventMapper
{
	public static partial Event Map(this EventEntity @event);

	public static partial IQueryable<Event> Map(this IQueryable<EventEntity> events);

    public static partial EventEntity Map(this CreateEventInput @event);
}
