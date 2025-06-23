namespace EventSignup.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int MaxAttendees { get; set; } = 0;

        public List<Participant> Participants { get; set; } = new();
    }
}