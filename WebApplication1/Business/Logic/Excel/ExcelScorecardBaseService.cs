using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc.Routing.Constraints;
using ClosedXML.Excel;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Excel
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
            {
                SetAJudgeResult(worksheet);
            }
            else
            {
                //var lattClassWorksheetNames = new string[]
                //    {"Lätt grund", "Lätt lagkür 1", "Lätt lagkür 2", "Lätt ind grund", "Lätt ind kür"};
                //var mellanClassWorksheetNames = new string[]
                //    {"Skritt mellan grund", "Skritt jrlag grund", "Skritt srlag grund", "Skritt lagkür"};

                //var horseIsPartOfSheet = ConcatArrays(lattClassWorksheetNames, mellanClassWorksheetNames);
                //if (IsSheetNameInArray(worksheet, horseIsPartOfSheet))
                //{
                    SetLattClassHorseResult(worksheet);
                //}
            }
        }

        protected void SetAJudgeResult(IXLWorksheet worksheet)
        {
            var result = _excelBaseService.GetNamedCell(worksheet, "result");
            result.Value = ContestService.HorsePointTraHastTavling();
        }
        protected void SetLattClassHorseResult(IXLWorksheet worksheet)
        {
            var result = _excelBaseService.GetNamedCell(worksheet, "Hästpoäng");
            if(result != null)
            {
                result.Value = ContestService.HorsePointTraHastTavling();
            }
        }

        protected static bool IsSheetNameInArray(IXLWorksheet worksheet, string[] lattClassWorksheetName)
        {
            return lattClassWorksheetName.Contains(worksheet.Name.Trim());
        }

        protected static string[] ConcatArrays(string[] lattClassWorksheetNames, string[] mellanClassWorksheetNames)
        {
            var horseIsPartOfSheet = new string[lattClassWorksheetNames.Length + mellanClassWorksheetNames.Length];
            lattClassWorksheetNames.CopyTo(horseIsPartOfSheet, 0);
            mellanClassWorksheetNames.CopyTo(horseIsPartOfSheet, lattClassWorksheetNames.Length);
            return horseIsPartOfSheet;
        }

        protected void SetHeaderPostfix(IXLWorksheet worksheet)
        {
            var header = _excelBaseService.GetNamedCell(worksheet, "header");
            if(header != null)
            {
                //var headerPostfix = _competitionData.VaultingClass.HeaderPostfix;
                //header.Value = header.Value + " " + headerPostfix;
            }


        }
        protected void SetFirstInformationGroup(IXLWorksheet worksheet, int startRow)
        {
            var firstcell = _excelBaseService.GetNamedCell(worksheet, "datum");
            
            //var range = worksheet.Range("datum") ;
            //var firstcell = range.FirstCell();
            firstcell.Value = _competitionData.GetStepDate();
            firstcell.CellBelow(1).Value = _competitionData.EventLocation;
            firstcell.CellBelow(2).Value = _competitionData.GetName();
            firstcell.CellBelow(3).Value = _competitionData.VaultingClubName;
            firstcell.CellBelow(4).Value = _competitionData.Country;
            firstcell.CellBelow(5).Value = RemoveNumberFromEnd(_competitionData.HorseName);
            firstcell.CellBelow(6).Value = _competitionData.LungerName;
            //SetValueInWorksheet(worksheet, startRow, "c", _competitionData.GetStepDate());
            //SetValueInWorksheet(worksheet, startRow + 1, "c", _competitionData.EventLocation);
            //SetValueInWorksheet(worksheet, startRow + 2, "c", _competitionData.VaulterName??_competitionData.TeamName);
            //SetValueInWorksheet(worksheet, startRow + 3, "c", _competitionData.VaultingClubName);
            //SetValueInWorksheet(worksheet, startRow + 4, "c", _competitionData.Country);
            //SetValueInWorksheet(worksheet, startRow + 5, "c", _competitionData.HorseName);
            //SetValueInWorksheet(worksheet, startRow + 6, "c", _competitionData.LungerName);
        }

       

        //private void SetInformationGroup2(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        //{
        //    string tableName = GetJudgeTableName(judgeTable);
        //    SetInformationGroup2(worksheet, tableName, startRow);
        //}
        protected void SetJudgeName(IXLWorksheet worksheet, int row, JudgeTable judgeTable)
        {
            _excelBaseService.SetValueInWorksheet(worksheet,"domare", GetJudgeName(judgeTable));
            //SetValueInWorksheet(worksheet, row, "c", GetJudgeName(judgeTable));
        }

        protected void SetInformationGroup2(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow, string startNumber)
        {
            var backgroundColors = new XLColor[]
            {
                XLColor.White , XLColor.White , XLColor.Blue, XLColor.Green, XLColor.Red, XLColor.Yellow, XLColor.White, XLColor.White, XLColor.White, XLColor.White, XLColor.White, XLColor.Red, XLColor.Yellow

            };

            string tableName = GetJudgeTableName(judgeTable);

            var secondcell = _excelBaseService.GetNamedCell(worksheet, "bord");

            secondcell.CellAbove(1).Value = startNumber;
            secondcell.Value = tableName;
            secondcell.CellBelow(1).SetValue( _competitionData.VaultingClass.ClassNr);
            secondcell.CellBelow(2).Value = _competitionData.MomentName;
            var armnrCell = _excelBaseService.SetValueInWorksheet(worksheet, "armnr", _competitionData.ArmNumber?.Trim());
            int classNr;
            
            if (armnrCell != null && int.TryParse(_competitionData.VaultingClass.ClassNr, out classNr) && classNr <= backgroundColors.Length)
            {
                armnrCell.CellBelow(1).Style.Fill.BackgroundColor = backgroundColors[classNr-1];
            }

            //secondcell.CellBelow(3).Value = _competitionData.ArmNumber;
            //SetValueInWorksheet(worksheet, startRow, "l", startNumber);

            ////           SetValueInWorksheet(worksheet, startRow + 1, "l", tableName);
            //SetValueInWorksheet(worksheet, startRow + 2, "l", _competitionData.VaultingClass.ClassNr.ToString());
            //SetValueInWorksheet(worksheet, startRow + 3, "l", _competitionData.MomentName);
            //SetValueInWorksheet(worksheet, startRow + 4, "l", _competitionData.ArmNumber);
        }



        protected void SaveExcelFile(string outputFileName)
        {
            string fileoutputname = @"C:\episerver\voltige\VoltigeClosedXML\output\" + outputFileName;
            _excelBaseService.SaveExcelFile(fileoutputname);
        }

        protected string GetOutputFilename(JudgeTable judgeTabel, string fileNamePrefix = "")
        {
            string pathPrefix = "";
            if (judgeTabel == null)
                return null;
            if (fileNamePrefix.Length > 0)
            {
                pathPrefix = "utskrift_";
            }

            var fileName = _competitionData.GetName().Replace("–", "").Replace(".xlsx", ""); //https://www.pdfen.com/merge/merge-files-to-pdf
            fileName = fileName.Trim() + '_' + judgeTabel.JudgeTableName +
                       "_klass" + _competitionData.VaultingClass.ClassNr + '_' + _competitionData.MomentName + "_" +
                       _competitionData.Horse1.HorseName.Trim() + '_' +
                       _competitionData.ListClassStep.Date.DayOfWeek.ToString().Substring(0,2);

            var path = pathPrefix + _competitionData.ListClassStep.Date.ToShortDateString() +
                       @"\" + judgeTabel.JudgeTableName + @"\" +
                       _competitionData.ListClassStep.Name.Trim().Replace("–", "") + @"\";

            return path + fileNamePrefix+ fileName + ".xlsx";
        }

       
        private string GetJudgeName(JudgeTable judgeTable)
        {
            return judgeTable?.JudgeName;
        }

        private string GetJudgeTableName(JudgeTable judgeTable)
        {
            return judgeTable?.JudgeTableName.ToString();
        }

        private string RemoveNumberFromEnd(string horseName)
        {
            if (horseName == null)
            {
                return null;
            }

            var length = horseName.Length;
            if (length > 1)
            {
                var lastChar = horseName.Substring(horseName.Length - 1, 1);
                int lastCharInt;
                if (int.TryParse(lastChar, out lastCharInt))
                {
                    horseName = horseName.Substring(0, length - 1);
                }
            }

            return horseName;
        }
    }
}