using EventSignup.Models;
using EventSignup.Services;
using HotChocolate.Authorization;

namespace EventSignup
{
    public class Mutation
    {
        private readonly IDatabaseService _databaseService;

        public Mutation(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [Authorize]
        public async Task<SignupResult> SignupForEvent(SignupInput input)
        {
            try
            {
                // Check if event exists
                var eventItem = await _databaseService.GetEventByIdAsync(input.EventId);
                if (eventItem == null)
                {
                    return new SignupResult { Success = false, Message = "Event not found" };
                }

                // Check if event is full
                var participantCount = await _databaseService.GetEventParticipantCountAsync(input.EventId);
                if (participantCount >= eventItem.MaxAttendees)
                {
                    return new SignupResult { Success = false, Message = "Event is full" };
                }

                // Check if user is already signed up
                var existingParticipation = await _databaseService.GetParticipantAsync(input.EventId, input.Email);
                if (existingParticipation != null)
                {
                    return new SignupResult { Success = false, Message = "Already signed up for this event" };
                }

                // Create new participant
                var participant = new Participant
                {
                    EventId = input.EventId,
                    Name = input.Name,
                    Email = input.Email
                };

                var createdParticipant = await _databaseService.CreateParticipantAsync(participant);

                return new SignupResult
                {
                    Success = true,
                    Message = "Successfully signed up for event",
                    Participant = createdParticipant
                };
            }
            catch (Exception ex)
            {
                return new SignupResult { Success = false, Message = $"Error signing up: {ex.Message}" };
            }
        }
    }



    //     [Authorize]
    //     public async Task<SignupResult> CancelSignup(int eventId, string email)
    //     {
    //         try
    //         {
    //             var deleted = await _databaseService.DeleteParticipantAsync(eventId, email);
    //             if (deleted)
    //             {
    //                 return new SignupResult { Success = true, Message = "Successfully cancelled signup" };
    //             }
    //             else
    //             {
    //                 return new SignupResult { Success = false, Message = "No signup found to cancel" };
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             return new SignupResult { Success = false, Message = $"Error cancelling signup: {ex.Message}" };
    //         }
    //     }

    //     [Authorize]
    //     public async Task<SignupResult> UpdateSignup(UpdateSignupInput input)
    //     {
    //         try
    //         {
    //             var existingParticipant = await _databaseService.GetParticipantAsync(input.EventId, input.Email);
    //             if (existingParticipant == null)
    //             {
    //                 return new SignupResult { Success = false, Message = "No signup found to update" };
    //             }

    //             // Update fields
    //             existingParticipant.Name = input.Name ?? existingParticipant.Name;
    //             existingParticipant.Email = input.Email ?? existingParticipant.Email;

    //             var updated = await _databaseService.UpdateParticipantAsync(existingParticipant);
    //             if (updated)
    //             {
    //                 return new SignupResult 
    //                 { 
    //                     Success = true, 
    //                     Message = "Successfully updated signup",
    //                     Participant = existingParticipant
    //                 };
    //             }
    //             else
    //             {
    //                 return new SignupResult { Success = false, Message = "Failed to update signup" };
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             return new SignupResult { Success = false, Message = $"Error updating signup: {ex.Message}" };
    //         }
    //     }
    // }


    public class SignupInput
    {
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class UpdateSignupInput
    {
        public int EventId { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class SignupResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Participant? Participant { get; set; }
    }
} 