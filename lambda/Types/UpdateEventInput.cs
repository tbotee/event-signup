
namespace EventSignup.Types
{
	public class UpdateEventInput
	{
		public int Id { get; set; }

		public Optional<string?> Name { get; set; }

		public Optional<DateTime?> Date { get; set; }

		public Optional<int?> MaxAttendees { get; set; }
	}
}