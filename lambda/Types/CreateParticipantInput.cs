namespace EventSignup.Types
{
    public class CreateParticipantInput
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required int EventId { get; set; }
    }
}
