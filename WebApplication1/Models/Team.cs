using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }

        public int? VaultingClubId { get; set; }

        [ForeignKey("VaultingClubId")]
        public virtual Club VaultingClub { get; set; }

        public virtual List<TeamList> TeamList { get; set; }

        public int? VaultingClassId { get; set; }

        [ForeignKey("VaultingClassId")]
        public virtual CompetitionClass VaultingClass { get; set; }
    }
}