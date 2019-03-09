using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ScoreSheets
    {
        public int ScoreSheetsId { get; set; }

        public string NameOfType { get; set; }

        public string Excelfile { get; set; }

        public virtual List<Step> TestMomentList { get; set; }



    }
}