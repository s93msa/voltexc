using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace WebApplication1.Classes
{
    public class VaultingContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<CompetitionClass> CompetitionClasses { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<Horse> Horses { get; set; }

        public DbSet<JudgeTable> JudgeTables  { get; set; }
        public DbSet<Lunger> Lungers { get; set; }

        public DbSet<StartListClass> StartListClasses { get; set; }
        public DbSet<StartListClassStep> StartListClassSteps { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Vaulter> Vaulters { get; set; }
    }
}