using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Excel
{
    public abstract class ExcelBaseService
    {
        private ExcelPreCompetitionData _competitionData;

        public ExcelBaseService(ExcelPreCompetitionData competitionInformation)
        {
            _competitionData = competitionInformation;
        }

        protected void ShowOnlyWorksheet(IXLWorksheet worksheet)
        {
            IXLWorksheets workbookWorksheets = _competitionData.Workbook?.Worksheets;
            if (workbookWorksheets == null)
                return;

            foreach (var currrentWorksheet in workbookWorksheets)
            {
                if (currrentWorksheet == worksheet)
                {
                    currrentWorksheet.Visibility = XLWorksheetVisibility.Visible;
                    currrentWorksheet.TabActive = true;
                }
                else
                    currrentWorksheet.Hide(); //.Visibility = XLWorksheetVisibility.Hidden;
            }
        }

        protected void SetFirstInformationGroup(IXLWorksheet worksheet, int startRow)
        {
            SetValueInWorksheet(worksheet, startRow, "c", _competitionData.GetStepDate());
            SetValueInWorksheet(worksheet, startRow + 1, "c", _competitionData.EventLocation);
            SetValueInWorksheet(worksheet, startRow + 2, "c", _competitionData.VaulterName??_competitionData.Team1?.Name);
            SetValueInWorksheet(worksheet, startRow + 3, "c", _competitionData.VaultingClubName);
            SetValueInWorksheet(worksheet, startRow + 4, "c", _competitionData.Country);
            SetValueInWorksheet(worksheet, startRow + 5, "c", _competitionData.HorseName);
            SetValueInWorksheet(worksheet, startRow + 6, "c", _competitionData.LungerName);
        }

        //private void SetInformationGroup2(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        //{
        //    string tableName = GetJudgeTableName(judgeTable);
        //    SetInformationGroup2(worksheet, tableName, startRow);
        //}
        protected void SetJudgeName(IXLWorksheet worksheet, int row, JudgeTable judgeTable)
        {
            SetValueInWorksheet(worksheet, row, "c", GetJudgeName(judgeTable));
        }

        protected void SetInformationGroup2(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow, string startNumber)
        {
            string tableName = GetJudgeTableName(judgeTable);
            SetValueInWorksheet(worksheet, startRow, "l", startNumber);
            SetValueInWorksheet(worksheet, startRow + 1, "l", tableName);
            SetValueInWorksheet(worksheet, startRow + 2, "l", _competitionData.VaulterClass.ClassNr + " (" + _competitionData.VaulterClass.ClassName + ")");
            SetValueInWorksheet(worksheet, startRow + 3, "l", _competitionData.MomentName);
            SetValueInWorksheet(worksheet, startRow + 4, "l", _competitionData.ArmNumber);
        }

        

        protected void SaveExcelFile(string outputFileName)
        {
            if (outputFileName == null)
                return;

            string fileoutputname = @"C:\Temp\Test_Voligemallar\output\" + outputFileName + ".xlsx";
            _competitionData.Workbook.SaveAs(fileoutputname);
        }

        protected string GetOutputFilename(JudgeTable judgeTabel)
        {
            return _competitionData.ListClassStep.Date.ToShortDateString() + @"\" + judgeTabel.JudgeTableName + @"\" + _competitionData.ListClassStep.Name + @"\" + _competitionData.GetName() + ".xlsx";
        }

        private IXLCell GetNamedCell(IXLWorksheet worksheet, string namedCell)
        {
            IXLNamedRange xlNamedRange = worksheet.NamedRange(namedCell);
            if (xlNamedRange == null)
                return (IXLCell)null;
            IXLRange xlRange = xlNamedRange.Ranges.FirstOrDefault<IXLRange>();
            if (xlRange == null)
                return (IXLCell)null;
            return xlRange.FirstCell();
        }

        private void SetValueInWorksheet(IXLWorksheet worksheet, int row, string column, string value)
        {
            worksheet.Cell(row, column).Value = value;
        }

        private void SetValueInWorksheet(IXLWorksheet worksheet, string cellName, string value)
        {
            var linkTocell = worksheet.Workbook.NamedRange(cellName);
            //var g = GetNamedCell(worksheet, cellName);
            worksheet.Cell(linkTocell.RefersTo.Split('!')[1]).Value = value;
            //          worksheet.Workbook.Range(cellName).Cells();
            //= value;
        }
        private string GetJudgeName(JudgeTable judgeTable)
        {
            return judgeTable?.JudgeName;
        }

        private string GetJudgeTableName(JudgeTable judgeTable)
        {
            return judgeTable?.JudgeTableName.ToString();
        }
    }
}