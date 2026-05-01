using Microsoft.EntityFrameworkCore;
using VoltigeCore.Models;

namespace VoltigeCore.Classes
{
    public class VaultingContext : DbContext
    {
        public VaultingContext() { }

        public VaultingContext(DbContextOptions<VaultingContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(AppConfig.ConnectionString);
            }
        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<CompetitionClass> CompetitionClasses { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<Horse> Horses { get; set; }
        public DbSet<JudgeTable> JudgeTables { get; set; }
        public DbSet<Lunger> Lungers { get; set; }
        public DbSet<StartListClass> StartListClasses { get; set; }
        public DbSet<StartListClassStep> StartListClassSteps { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamList> TeamMembers { get; set; }
        public DbSet<Vaulter> Vaulters { get; set; }
        public DbSet<HorseOrder> HorseOrders { get; set; }
        public DbSet<ContestType> ContestTypes { get; set; }
        public DbSet<StepType> StepTypes { get; set; }
        public DbSet<VaulterOrder> VaulterOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>().HasIndex(c => c.ClubTdbId);
            modelBuilder.Entity<CompetitionClass>().HasIndex(c => c.ClassTdbId);
            modelBuilder.Entity<Horse>().HasIndex(h => h.HorseTdbId);
            modelBuilder.Entity<Lunger>().HasIndex(l => l.LungerTdbId);
            modelBuilder.Entity<Vaulter>().HasIndex(v => v.VaulterTdbId);
        }
    }
}
