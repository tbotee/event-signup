using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSignup.Data.Entities
{
    [Table("events", Schema = "public")]
    public class EventEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Column("date")]
        public DateTime Date { get; set; }
        
        [Column("max_attendees")]
        public int MaxAttendees { get; set; } = 0;

        public List<ParticipantEntity> Participants { get; set; } = new();
    }
}