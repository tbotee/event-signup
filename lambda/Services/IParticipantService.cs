using EventSignup.Types;
using GreenDonut.Data;

namespace EventSignup.Services
{
	public interface IParticipantService
	{
		Task<Page<Participant>> GetParticipantsAsync(PagingArguments pagingArguments, QueryContext<Participant>? queryContext = null, CancellationToken cancellationToken = default);

		Task<int> CreateParticipantAsync(CreateParticipantInput input, CancellationToken cancellationToken);

        Task<int> DeleteParticipantAsync(int eventId, CancellationToken cancellationToken);
    }
}