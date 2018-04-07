using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TeamList
    {
        public int TeamListId { get; set; }
        public int StartNumber { get; set; }
        public int ParticipantId { get; set; }

        [ForeignKey("ParticipantId")]
        public virtual Vaulter Participant { get; set; }

        public int? TeamId { get; set; }

        //public Team Team { get; set; }
    }
}