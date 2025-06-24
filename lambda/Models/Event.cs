using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSignup.Models
{
    [Table("events", Schema = "public")]
    public class Event
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

        public List<Participant> Participants { get; set; } = new();
    }
}