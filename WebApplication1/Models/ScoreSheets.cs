using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int? ScoreSheetBaseId { get; set; }
        [ForeignKey("ScoreSheetBaseId")]
        public virtual ScoreSheets BaseScoreScheet { get; set; }

        public string HeaderPostfix { get; set; }


        public string GetExcelfile()
        {
            if(!string.IsNullOrEmpty(Excelfile))
            {
                return Excelfile;
            }
            return BaseScoreScheet.GetExcelfile();
        }

        public List<Step> GetMoments()
        {
            if (TestMomentList != null && TestMomentList.Count > 0)
            {
                return TestMomentList;
            }
            return BaseScoreScheet.GetMoments();
        }


    }
}