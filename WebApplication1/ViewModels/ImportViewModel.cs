using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Business.Logic.Import;
using WebApplication1.Migrations;

namespace WebApplication1.ViewModels
{
    public class ImportViewModel
    {
        public int NewLungers { get; set; }
        public int UppdatedLungers { get; set; }

        public int NewHorses { get; set; }
        public int UpdatedHorses { get; set; }

        public int NewClubs { get; set; }
        public int UpdatedClubs { get; set; }
        public int NewClasses { get; set; }
        public int UpdatedClasses { get; set; }

        public int NewTeams { get; set; }
        public int UpdatedTeams { get; set; }

        public int NewTeamMembers { get; set; }
        public int UpdatedTeamMembers { get; set; }

        public int NewVaulters { get; set; }
        public int UpdatedVaulters { get; set; }


        public Dictionary<int,Changed> ChangedStartListTeamList { get; set; }
        public Dictionary<int, UpdateService.NewHordeorders> ChangedStartListIndividualList { get; set; }



    }
}