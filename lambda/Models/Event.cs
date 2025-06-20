namespace EventSignup.Lambda.Models
{
    public class Event
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Date { get; set; }
        public required int MaxAttendees { get; set; }
    }
}