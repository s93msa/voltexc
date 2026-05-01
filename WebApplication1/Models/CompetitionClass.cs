using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltigeCore.Models
{
    public class CompetitionClass
    {
        public int CompetitionClassId { get; set; }
        public string ClassNr { get; set; }
        public string ClassName { get; set; }
        public int ClassTdbId { get; set; }
        public int? ScoreSheetId { get; set; }
        [ForeignKey("ScoreSheetId")]
        public virtual ScoreSheets? ScoreSheet { get; set; }

        public List<Step> GetCompetitionSteps(ContestType currentContestType)
        {
            return ScoreSheet.GetMoments();
        }
    }
}
