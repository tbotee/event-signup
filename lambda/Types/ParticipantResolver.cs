using EventSignup.Services;
using GreenDonut.Data;
using HotChocolate.Authorization;
using HotChocolate.Types.Pagination;

namespace EventSignup.Types;

//[Authorize]
[ExtendObjectType(typeof(Query))]
public static partial class ParticipantResolver
{
    public static async Task<Participant?> GetParticipantAsync(
        int participantId,
        QueryContext<Participant> queryContext,
        IParticipantByIdDataLoader participantById,
        CancellationToken cancellationToken)
        => await participantById
            .With(queryContext)
            .LoadAsync(participantId, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public static async Task<Connection<Participant>> GetParticipantsAsync(
        PagingArguments pagingArguments,
        QueryContext<Participant> queryContext,
        CancellationToken cancellationToken,
        IParticipantService participantService)
        => await participantService
            .GetParticipantsAsync(pagingArguments, queryContext, cancellationToken)
            .ToConnectionAsync();
}
