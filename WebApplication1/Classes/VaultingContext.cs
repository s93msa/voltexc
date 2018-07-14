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
        public VaultingContext()
        {
            Configuration.LazyLoadingEnabled = true;
        }


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
        public DbSet<TeamList> TeamMembers { get; set; }
        public DbSet<Vaulter> Vaulters { get; set; }


        public System.Data.Entity.DbSet<WebApplication1.Models.HorseOrder> HorseOrders { get; set; }

        public DbSet<ContestType> ContestTypes { get; set; }

        public DbSet<StepType> StepTypes { get; set; }

        public DbSet<VaulterOrder> VaulterOrders { get; set; }

    }
}