using AppAny.HotChocolate.FluentValidation;
using EventSignup.Services;

namespace EventSignup.Types
{

    [ExtendObjectType(typeof(Mutation))]
    public class ParticipantMutations
    {
        [Error(typeof(NotFoundException))]
        public async Task<Participant> CreateParticipantAsync(
            [UseFluentValidation] CreateParticipantInput input,
            CancellationToken cancellationToken,
            IParticipantService participantService,
            IParticipantByIdDataLoader participantById)
        {
            var createdParticipantId = await participantService.CreateParticipantAsync(input, cancellationToken);
            return await participantById
                .LoadRequiredAsync(createdParticipantId, cancellationToken);
        }

        [Error(typeof(NotFoundException))]
        public async Task<int> DeleteParticipantAsync(
            int participantId,
            CancellationToken cancellationToken,
            IParticipantService participantService)
        => await participantService.DeleteParticipantAsync(participantId, cancellationToken);

    }
}
