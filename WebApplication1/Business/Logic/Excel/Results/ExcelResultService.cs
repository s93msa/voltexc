using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Business.Logic.Excel;

namespace WebApplication1.Business.Logic.Excel.Results
{
    public class ExcelResultService
    {
        private ExcelBaseService _excelBaseService;

        public ExcelResultService(XLWorkbook workbook)
        {
            _excelBaseService = new ExcelBaseService(workbook);
        }

        public void Save()
        {
            _excelBaseService.SaveExcelFile();
        }

        public void SetCompetitionClasses(CompetitionClass[] competetionClasses)
        {
            const string worksheetName = "Klasser"; 
            List<Row<string>> rows = new List<Row<string>>();
            foreach (var competitionClass in competetionClasses)
            {
                string[] row = CompetetionClassAsStringArray(competitionClass);
                rows.Add(new Row<string>(row));
            }

            _excelBaseService.SetValuesInWorkSheet<string>(worksheetName, 2, rows.ToArray());
        }

        private string[] CompetetionClassAsStringArray(CompetitionClass competitionClass)
        {
            string[] classArray = new string[15];
            classArray[0] = competitionClass.ClassNumber;
            classArray[1] = competitionClass.ClassName;
            classArray[2] = competitionClass.NumberOfJudges;
            classArray[3] = competitionClass.Moment1;
            classArray[4] = competitionClass.Moment2;
            classArray[5] = competitionClass.Moment3;
            classArray[6] = competitionClass.Moment4;
            classArray[7] = competitionClass.Moment1Header;
            classArray[8] = competitionClass.Moment2Header;
            classArray[9] = competitionClass.Moment3Header;
            classArray[10] = competitionClass.Moment4Header;
            classArray[11] = competitionClass.JudgesMoment1;
            classArray[12] = competitionClass.JudgesMoment2;
            classArray[13] = competitionClass.JudgesMoment3;
            classArray[14] = competitionClass.JudgesMoment4;

            return classArray;
        }
    }
}