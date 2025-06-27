using Microsoft.EntityFrameworkCore;
using EventSignup.Data.Entities;

namespace EventSignup.Data
{
    public class EventSignupContext : DbContext
    {
        public EventSignupContext(DbContextOptions<EventSignupContext> options) : base(options)
        {
        }

        public DbSet<EventEntity> Events => Set<EventEntity>();
        public DbSet<ParticipantEntity> Participants => Set<ParticipantEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<ParticipantEntity>()
                .HasIndex(p => new { p.EventId, p.Email })
                .IsUnique()
                .HasDatabaseName("idx_participants_event_email_unique");
                
            modelBuilder.Entity<ParticipantEntity>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 