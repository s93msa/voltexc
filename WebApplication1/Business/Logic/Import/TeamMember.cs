using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Import
{
    public class TeamMember
    {
        public string TeamName { get; set; }

        public int StartNumber { get; set; }
        public int VaulterTdbId { get; set; }
         public string VaulterName  {get; set; }
    }
}