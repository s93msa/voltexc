using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TeamList
    {
        public int TeamListId { get; set; }
        public int StartNumber { get; set; }
        public virtual Vaulter Participant { get; set; }
    }
}