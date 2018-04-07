using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace WebApplication1.Business.Logic.Import
{
    public class ExcelImportMergedModel
    {
        public int ClassTdbId { get; set; }
        public int ClassNr { get; set; }
        public string ClassName { get; set; }
        public int LungerTdbId { get; set; }
        public string LungerName { get; set; }
        public int HorseTdbId { get; set; }
        public string HorseName { get; set; }
        public int ClubTdbId { get; set; }
        public string ClubName { get; set; }
        public int VaulterId1 { get; set; }
        public string VaulterName1 { get; set; }
        public int VaulterId2 { get; set; }
        public string VaulterName2 { get; set; }
        public int VaulterId3 { get; set; }
        public string VaulterName3 { get; set; }
        public int VaulterId4 { get; set; }
        public string VaulterName4 { get; set; }
        public int VaulterId5 { get; set; }
        public string VaulterName5 { get; set; }
        public int VaulterId6 { get; set; }
        public string VaulterName6 { get; set; }

        public bool IsTeam { get; set; }

        public string TeamName { get; set; }

    }
}