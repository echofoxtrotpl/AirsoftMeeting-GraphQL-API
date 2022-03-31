using Microsoft.EntityFrameworkCore;

namespace AirsoftMeetingGraphQL.Entities
{
    public class AirsoftDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<PlayersJoinedEvent> PlayersJoinedEvents { get; set; }

        public AirsoftDbContext(DbContextOptions options) :base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity
                    .HasOne<Player>(e => e.Creator)
                    .WithMany(h => h.HostedEvents)
                    .HasForeignKey(e => e.CreatorId);
                entity
                    .HasOne<Location>(e => e.Location)
                    .WithMany(i => i.Events)
                    .HasForeignKey(i => i.LocationId);
                entity
                    .HasOne<Image>(e => e.Image)
                    .WithOne(i => i.Event)
                    .HasForeignKey<Event>(i => i.ImageId);
            });
            modelBuilder.Entity<Player>(entity =>
            {
                entity
                    .HasMany<Image>(p => p.Images)
                    .WithOne(i => i.Creator)
                    .HasForeignKey(i=>i.CreatorId);
                entity
                    .HasOne<Team>(p => p.Team)
                    .WithMany(t => t.TeamCrew)
                    .HasForeignKey(p => p.TeamId);
            });
            modelBuilder.Entity<PlayersJoinedEvent>(entity =>
            {
                entity
                    .HasKey(ep => new {ep.EventId, ep.PlayerId});
                entity
                    .HasOne<Player>(pe => pe.Player)
                    .WithMany(p => p.JoinedEvents);
                entity
                    .HasOne<Event>(pe => pe.Event)
                    .WithMany(e => e.PlayersJoinedEvent);
            });
        }
    }
}