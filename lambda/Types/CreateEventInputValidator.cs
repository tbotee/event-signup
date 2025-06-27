
using FluentValidation;

namespace EventSignup.Types;

public class CreateEventInputValidator : AbstractValidator<CreateEventInput>
{
	public CreateEventInputValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();
		RuleFor(x => x.Date)
			.NotNull();
		RuleFor(x => x.MaxAttendees)
			.NotNull();
	}
}