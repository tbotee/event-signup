using EventSignup.Data;
using EventSignup.Services;
using FluentValidation;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace EventSignup.Types
{
    public class CreateParticipantValidator : AbstractValidator<CreateParticipantInput>
    {
        public CreateParticipantValidator(IParticipantService _participanService)
        {
            RuleFor(x => x.EventId)
                .NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress();
            RuleFor(x => new
            {
                x.Email,
                x.EventId
            }).MustAsync(async (_, value, context, cancellationToken) =>
            {
                var p = await _participanService.GetParticipantByEventIdandEmail(value.EventId, value.Email, cancellationToken);
                if (p != null)
                {
                    context.MessageFormatter.AppendArgument("Email", value.Email);
                    context.MessageFormatter.AppendArgument("EventId", value.EventId);
                    return false;
                }
                return true;
            });
        }
    }
}
