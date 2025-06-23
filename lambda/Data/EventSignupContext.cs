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
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events", schema: "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Date).HasColumnName("date").IsRequired();
                entity.Property(e => e.MaxAttendees).HasColumnName("max_attendees").HasDefaultValue(0);

                entity.HasIndex(e => e.Date).HasDatabaseName("idx_events_date");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("participants", schema: "public");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.EventId).HasColumnName("event_id").IsRequired();
                entity.Property(p => p.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
                entity.Property(p => p.Email).HasColumnName("email").HasMaxLength(255).IsRequired();

                entity.HasOne(p => p.Event)
                    .WithMany(e => e.Participants)
                    .HasForeignKey(p => p.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => p.EventId).HasDatabaseName("idx_participants_event_id");
                entity.HasIndex(p => new { p.EventId, p.Email })
                    .IsUnique()
                    .HasDatabaseName("idx_participants_event_email_unique");
            });
        }
    }
} 