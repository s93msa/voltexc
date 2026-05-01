using System.ComponentModel.DataAnnotations.Schema;

namespace VoltigeCore.Models
{
    public class VaulterOrder
    {
        public int VaulterOrderID { get; set; }
        public int StartOrder { get; set; }
        public int? VaulterId { get; set; }
        [ForeignKey("VaulterId")]
        public virtual Vaulter Participant { get; set; }
        public int Testnumber { get; set; }
        public bool IsActive { get; set; }
        public int? HorseOrderId { get; set; }
    }
}
