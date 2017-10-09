using System;

namespace WebApplication1.Models
{
    public class Step
    {
        public int StepId { get; set; }
        public virtual ContestType TypeOfContest { get; set; }
        public virtual StepType TypeOfStep { get; set; }

        public int TestNumber { get; set; }
        public string Name { get; set; }
        //public DateTime Date { get; set; }

        public string ExcelWorksheetNameJudgesTableA { get; set; }
        public string ExcelWorksheetNameJudgesTableB { get; set; }
        public string ExcelWorksheetNameJudgesTableC { get; set; }
        public string ExcelWorksheetNameJudgesTableD { get; set; }

        public string ResultMomentText { get; set; }

        public string OverrideExcelfileName { get; set; }



    }


}