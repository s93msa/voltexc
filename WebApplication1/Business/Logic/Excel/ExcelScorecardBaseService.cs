using System;
using ClosedXML.Excel;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Excel
{
    public abstract class ExcelScorecardBaseService
    {
        public bool StartOrderInfileName { get; set; } = false;

        private readonly ExcelPreCompetitionData _competitionData;
        protected ExcelBaseService _excelBaseService;

        protected ExcelScorecardBaseService(ExcelPreCompetitionData competitionInformation)
        {
            _competitionData = competitionInformation;
            _excelBaseService = new ExcelBaseService(_competitionData.Workbook);
        }

        protected void SetHorsePoints(IXLWorksheet worksheet)
        {
            var horseSheetNames = new string[] { "Häst, individuell", "Häst, lag", "Pas-de-Deux Häst" };
            if (IsSheetNameInArray(worksheet, horseSheetNames))
                SetAJudgeResult(worksheet);
            else
                SetLattClassHorseResult(worksheet);
        }

        protected void SetAJudgeResult(IXLWorksheet worksheet)
        {
            var result = _excelBaseService.GetNamedCell(worksheet, "result");
            result.Value = ContestService.HorsePointTraHastTavling();
        }

        protected void SetLattClassHorseResult(IXLWorksheet worksheet)
        {
            var result = _excelBaseService.GetNamedCell(worksheet, "Hästpoäng");
            if (result != null)
                result.Value = ContestService.HorsePointTraHastTavling();
        }

        protected static bool IsSheetNameInArray(IXLWorksheet worksheet, string[] names)
        {
            return System.Linq.Enumerable.Contains(names, worksheet.Name.Trim());
        }

        protected static string[] ConcatArrays(string[] a, string[] b)
        {
            var result = new string[a.Length + b.Length];
            a.CopyTo(result, 0);
            b.CopyTo(result, a.Length);
            return result;
        }

        protected void SetHeaderPostfix(IXLWorksheet worksheet)
        {
            var header = _excelBaseService.GetNamedCell(worksheet, "header");
            if (header != null)
            {
                var headerPostfix = _competitionData.VaultingClass.ScoreSheet.HeaderPostfix;
                if (header.Value.IsBlank || (!string.IsNullOrEmpty(headerPostfix) && !header.Value.ToString().EndsWith(headerPostfix)))
                    header.Value = header.Value + " " + headerPostfix;
            }
        }

        protected void SetFirstInformationGroup(IXLWorksheet worksheet, int startRow)
        {
            var firstcell = _excelBaseService.GetNamedCell(worksheet, "datum");
            firstcell.Value = _competitionData.GetStepDate();
            firstcell.CellBelow(1).Value = _competitionData.EventLocation;
            firstcell.CellBelow(2).Value = _competitionData.GetName();
            firstcell.CellBelow(3).Value = _competitionData.VaultingClubName;
            firstcell.CellBelow(4).Value = _competitionData.Country;
            firstcell.CellBelow(5).Value = RemoveNumberFromEnd(_competitionData.HorseName);
            firstcell.CellBelow(6).Value = _competitionData.LungerName;
        }

        protected void SetJudgeName(IXLWorksheet worksheet, int row, JudgeTable judgeTable)
        {
            _excelBaseService.SetValueInWorksheet(worksheet, "domare", judgeTable?.JudgeName);
        }

        protected void SetInformationGroup2(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow, string startNumber)
        {
            var backgroundColors = new XLColor[]
            {
                XLColor.White, XLColor.White, XLColor.Blue, XLColor.Green, XLColor.Red, XLColor.Yellow,
                XLColor.White, XLColor.White, XLColor.White, XLColor.White, XLColor.White, XLColor.Red, XLColor.Yellow
            };

            string tableName = judgeTable?.JudgeTableName.ToString();
            var secondcell = _excelBaseService.GetNamedCell(worksheet, "bord");
            secondcell.CellAbove(1).Value = startNumber;
            secondcell.Value = tableName;
            secondcell.CellBelow(1).SetValue(_competitionData.VaultingClass.ClassNr);
            secondcell.CellBelow(2).Value = _competitionData.MomentName;
            var armnrCell = _excelBaseService.SetValueInWorksheet(worksheet, "armnr", _competitionData.ArmNumber?.Trim());
            int classNr;
            if (armnrCell != null && int.TryParse(_competitionData.VaultingClass.ClassNr, out classNr) && classNr <= backgroundColors.Length)
                armnrCell.CellBelow(1).Style.Fill.BackgroundColor = backgroundColors[classNr - 1];
        }

        protected void SaveExcelFile(string outputFileName)
        {
            outputFileName = outputFileName.Replace("/", "");
            string fileoutputname = AppConfig.OutputPath + outputFileName;
            _excelBaseService.SaveExcelFile(fileoutputname);
        }

        protected string GetOutputFilename(JudgeTable judgeTabel, string fileNamePrefix = "")
        {
            string pathPrefix = "";
            if (judgeTabel == null) return null;
            if (fileNamePrefix.Length > 0)
                pathPrefix = "utskrift_";

            var fileName = _competitionData.GetName().Replace("–", "").Replace(".xlsx", "");
            fileName = fileName.Trim() + '_' + judgeTabel.JudgeTableName +
                       "_klass" + _competitionData.VaultingClass.ClassNr + '_' + _competitionData.MomentName + "_" +
                       _competitionData.Horse1.HorseName.Trim() + '_' +
                       _competitionData.ListClassStep.Date.DayOfWeek.ToString().Substring(0, 2);

            var path = pathPrefix + _competitionData.ListClassStep.Date.ToShortDateString() +
                       @"\" + judgeTabel.JudgeTableName + @"\" +
                       _competitionData.ListClassStep.Name.Trim().Replace("–", "") + @"\";

            return path + fileNamePrefix + fileName + ".xlsx";
        }

        private string RemoveNumberFromEnd(string horseName)
        {
            if (horseName == null) return null;
            var length = horseName.Length;
            if (length > 1)
            {
                var lastChar = horseName.Substring(horseName.Length - 1, 1);
                if (int.TryParse(lastChar, out _))
                    horseName = horseName.Substring(0, length - 1);
            }
            return horseName;
        }
    }
}
