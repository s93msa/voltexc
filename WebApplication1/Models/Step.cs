using System.ComponentModel.DataAnnotations.Schema;

namespace VoltigeCore.Models
{
    public class Step
    {
        public int StepId { get; set; }
        [Column("TypeOfStep_StepTypeId")]
        public int? TypeOfStepStepTypeId { get; set; }
        [ForeignKey("TypeOfStepStepTypeId")]
        public virtual StepType TypeOfStep { get; set; }
        [Column("ScoreSheets_ScoreSheetsId")]
        public int? ScoreSheetsScoreSheetsId { get; set; }
        public int TestNumber { get; set; }
        public string Name { get; set; }
        public string ExcelWorksheetNameJudgesTableA { get; set; }
        public string ExcelWorksheetNameJudgesTableB { get; set; }
        public string ExcelWorksheetNameJudgesTableC { get; set; }
        public string ExcelWorksheetNameJudgesTableD { get; set; }
        public string ResultMomentText { get; set; }
        public string OverrideExcelfileName { get; set; }
    }
}
