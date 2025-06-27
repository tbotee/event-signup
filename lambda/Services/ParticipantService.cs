using EventSignup.Data;
using EventSignup.Types;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventSignup.Services
{
	public class ParticipantService(EventSignupContext _dbContext) : IParticipantService
	{
        public async Task<int> CreateParticipantAsync(
			CreateParticipantInput input,
			CancellationToken cancellationToken)
        {
            var participant = input.Map();
            _dbContext.Participants.Add(participant);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return participant.Id;
        }

        public async Task<int> DeleteParticipantAsync(int particvipantId, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants
                .FirstOrDefaultAsync(p => p.Id == particvipantId, cancellationToken);
            if (participant == null)
            {
                throw new NotFoundException(nameof(Event), particvipantId);
            }

            _dbContext.Participants.Remove(participant);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return particvipantId;
        }

        public async Task<Page<Participant>> GetParticipantsAsync(
			PagingArguments pagingArguments,
			QueryContext<Participant>? queryContext = default,
			CancellationToken cancellationToken = default)
			=> await _dbContext.Participants
				.Map()
				.With(queryContext, sd => sd.AddAscending(n => n.Id))
				.ToPageAsync(pagingArguments, cancellationToken);
	}
}
