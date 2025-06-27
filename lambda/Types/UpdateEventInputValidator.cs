
using FluentValidation;

namespace EventSignup.Types;

public class UpdateEventInputValidator : AbstractValidator<UpdateEventInput>
{
	public UpdateEventInputValidator()
	{
		RuleFor(x => x.Name.Value)
			.NotEmpty().WithName(nameof(UpdateEventInput.Name))
			.When(x => x.Name.HasValue);
		RuleFor(x => x.Date.Value)
			.NotNull().WithName(nameof(UpdateEventInput.Date))
			.When(x => x.Date.HasValue);
		RuleFor(x => x.MaxAttendees.Value)
			.NotNull().WithName(nameof(UpdateEventInput.MaxAttendees))
			.When(x => x.MaxAttendees.HasValue);
	}
}