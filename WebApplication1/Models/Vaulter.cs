using System.ComponentModel.DataAnnotations.Schema;

namespace VoltigeCore.Models
{
    [Table("vaulters")]
    public class Vaulter
    {
        public int VaulterId { get; set; }
        public string Name { get; set; }
        public int? VaultingClubId { get; set; }
        [ForeignKey("VaultingClubId")]
        public virtual Club? VaultingClub { get; set; }
        public string? Armband { get; set; }
        public int? VaultingClassId { get; set; }
        [ForeignKey("VaultingClassId")]
        public virtual CompetitionClass? VaultingClass { get; set; }
        public int VaulterTdbId { get; set; }
    }
}
