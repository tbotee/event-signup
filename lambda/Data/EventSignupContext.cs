using Microsoft.EntityFrameworkCore;
using EventSignup.Models;

namespace EventSignup.Data
{
    public class EventSignupContext : DbContext
    {
        public EventSignupContext(DbContextOptions<EventSignupContext> options) : base(options)
        {
        }

        public DbSet<Event> Events => Set<Event>();
        public DbSet<Participant> Participants => Set<Participant>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<Participant>()
                .HasIndex(p => new { p.EventId, p.Email })
                .IsUnique()
                .HasDatabaseName("idx_participants_event_email_unique");
                
            modelBuilder.Entity<Participant>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 