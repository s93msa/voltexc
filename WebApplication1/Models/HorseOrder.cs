using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Migrations;

namespace WebApplication1.Models
{
    public class HorseOrder
    {
        public int HorseOrderId { get; set; }
        public int StartNumber { get; set; }

        //public virtual HorseOrder Horse { get; set; }

        public virtual Horse HorseInformation { get; set; }

        public bool IsTeam { get; set; }

        public virtual Team VaultingTeam { get; set; }

        public virtual List<VaulterOrder> Vaulters { get; set; }
        //public virtual Vaulter Participant { get; set; }
        
        //public virtual Horse VaultingHorse { get; set; }

        //public int TestNumber { get; set; }

        public List<VaulterOrder> GetActiveVaulters()
        {
            return Vaulters.FindAll(x => x.IsActive);
        }

    

    }
}