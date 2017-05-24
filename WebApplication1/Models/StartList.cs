using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class StartList
    {
        public int StartListId { get; set; }
        public int StartNumber { get; set; }

        //public virtual HorseOrder Horse { get; set; }

        public virtual Horse HorseInformation { get; set; }

        public bool IsTeam { get; set; }

        public virtual Team VaultingTeam { get; set; }

        public virtual List<VaulterOrder> Vaulters { get; set; }
        //public virtual Vaulter Participant { get; set; }
        
        //public virtual Horse VaultingHorse { get; set; }

        //public int TestNumber { get; set; }

    }
}