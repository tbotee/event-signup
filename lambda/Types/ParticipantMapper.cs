using EventSignup.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace EventSignup.Types;

[Mapper]
public static partial class ParticipantMapper
{
	public static partial Participant Map(this ParticipantEntity participant);

	public static partial IQueryable<Participant> Map(this IQueryable<ParticipantEntity> participants);

    public static partial ParticipantEntity Map(this CreateParticipantInput participant);
}
