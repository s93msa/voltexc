using System.Collections.Generic;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public Club VaultingClub { get; set; }

        public virtual List<TeamList> VaultersList { get; set; }

        public virtual CompetitionClass VaultingClass { get; set; }
    }
}