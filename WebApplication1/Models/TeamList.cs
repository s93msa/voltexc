using System.ComponentModel.DataAnnotations.Schema;

namespace VoltigeCore.Models
{
    [Table("TeamLists")]
    public class TeamList
    {
        public int TeamListId { get; set; }
        public int StartNumber { get; set; }
        public int ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public virtual Vaulter Participant { get; set; }
        public int? TeamId { get; set; }
    }
}
