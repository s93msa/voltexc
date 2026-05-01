using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltigeCore.Models
{
    public class HorseOrder
    {
        public int HorseOrderId { get; set; }
        public int StartNumber { get; set; }
        public int? HorseId { get; set; }
        [ForeignKey("HorseId")]
        public virtual Horse HorseInformation { get; set; }
        public bool IsTeam { get; set; }
        public int? VaultingTeamId { get; set; }
        [ForeignKey("VaultingTeamId")]
        public virtual Team VaultingTeam { get; set; }
        public int TeamTestnumber { get; set; }
        public bool IsActive { get; set; }
        public virtual List<VaulterOrder> Vaulters { get; set; }
        public int? StartListClassStepId { get; set; }

        public List<VaulterOrder> GetActiveVaulters()
        {
            return Vaulters.FindAll(x => x.IsActive);
        }
    }
}
