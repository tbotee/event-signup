namespace EventSignup.Types
{
	public class CreateEventInput
	{
		public required string Name { get; set; }

		public DateTime Date { get; set; }

		public int MaxAttendees { get; set; }
	}
}