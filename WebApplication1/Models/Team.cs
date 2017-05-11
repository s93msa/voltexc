using System.Collections.Generic;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public Club VaultingClub { get; set; }

        public SortedList<int, Vaulter> Vaulters { get; set; }

        public CompetitionClass VaultingClass { get; set; }
    }
}