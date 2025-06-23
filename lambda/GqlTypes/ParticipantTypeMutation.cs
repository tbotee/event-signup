using EventSignup.Models;
using EventSignup.Services;

namespace EventSignup.GqlTypes;

[ExtendObjectType(typeof(Mutation))]
public class ParticipantTypeMutation
{
    public async Task<SignupResult> SignupForEvent(SignupInput input, [Service] IDatabaseService databaseService)
    {
        try
        {
            var validationResult = await ValidateSignupRequest(input, databaseService);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            return await CreateParticipant(input, databaseService);
        }
        catch (Exception ex)
        {
            return new SignupResult { Success = false, Message = $"Error signing up: {ex.Message}" };
        }
    }

    public async Task<SignupResult> DeleteAllParticipantsByEmail(DeleteAllParticipantsByEmailInput input, [Service] IDatabaseService databaseService)
    {
        var result = await databaseService.DeleteAllParticipantsByEmailAsync(input.Email);
        return new SignupResult { Success = result, Message = "Participants deleted" };
    }


    private async Task<SignupResult> CreateParticipant(SignupInput input, IDatabaseService databaseService)
    {
        var participant = new Participant
        {
            EventId = input.EventId,
            Name = input.Name,
            Email = input.Email
        };

        var createdParticipant = await databaseService.CreateParticipantAsync(participant);

        return new SignupResult
        {
            Success = true,
            Message = "Successfully signed up for event",
            Participant = createdParticipant
        };
    }

    private async Task<SignupResult> ValidateSignupRequest(SignupInput input, IDatabaseService databaseService)
    {
        var eventItem = await databaseService.GetEventByIdAsync(input.EventId);
        if (eventItem == null)
        {
            return new SignupResult { Success = false, Message = "Event not found" };
        }

        var participantCount = await databaseService.GetEventParticipantCountAsync(input.EventId);
        if (participantCount >= eventItem.MaxAttendees)
        {
            return new SignupResult { Success = false, Message = "Event is full" };
        }


        var existingParticipation = await databaseService.GetParticipantAsync(input.EventId, input.Email);
        if (existingParticipation != null)
        {
            return new SignupResult { Success = false, Message = "Already signed up for this event" };
        }

        return new SignupResult { Success = true, Message = "Validation passed" };
    }

    public class SignupInput
    {
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class DeleteAllParticipantsByEmailInput
    {
        public string Email { get; set; } = string.Empty;
    }

    public class SignupResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Participant? Participant { get; set; }
    }
}
