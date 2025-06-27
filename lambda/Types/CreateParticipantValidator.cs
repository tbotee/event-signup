using FluentValidation;

namespace EventSignup.Types
{
    public class CreateParticipantValidator : AbstractValidator<CreateParticipantInput>
    {
        public CreateParticipantValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(x => x.Email)
                .NotNull();
        }
    }
}
