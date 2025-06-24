using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSignup.Models;

[Table("participants", Schema = "public")]
public class Participant
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("event_id")]
    public int EventId { get; set; }
    
    [Required]
    [MaxLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [ForeignKey("EventId")]
    public Event Event { get; set; } = null!;
}