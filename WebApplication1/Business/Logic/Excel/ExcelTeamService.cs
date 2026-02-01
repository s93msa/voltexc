using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Excel
{
    public class ExcelTeamService : ExcelScorecardBaseService
    {
        private readonly ExcelPreCompetitionData _competitionData;

        public ExcelTeamService(ExcelPreCompetitionData competitionInformation) : base(competitionInformation)
        {
            _competitionData = competitionInformation;

        }

        public void CreateExcelforIndividual()
        {
            CreateExcelFromValuesJudgeA();
            CreateExcelFromValuesJudgeB();
            CreateExcelFromValuesJudgeC();
            CreateExcelFromValuesJudgeD();
        }



    

    private void CreateExcelFromValuesJudgeA()
        {
            var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableA?.Trim();
            CreateExcelFromValues(excelWorksheetNameJudgesTable, _competitionData.JudgeTableA);
        }
        private void CreateExcelFromValuesJudgeB()
        {
            var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableB?.Trim();
            CreateExcelFromValues(excelWorksheetNameJudgesTable, _competitionData.JudgeTableB);
        }
        private void CreateExcelFromValuesJudgeC()
        {
            var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableC?.Trim();
            CreateExcelFromValues(excelWorksheetNameJudgesTable, _competitionData.JudgeTableC);
        }

        private void CreateExcelFromValuesJudgeD()
        {
            var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableD?.Trim();
            CreateExcelFromValues(excelWorksheetNameJudgesTable, _competitionData.JudgeTableD);
        }

        private void CreateExcelFromValues(string excelWorksheetNameJudgesTable, JudgeTable judgeTable)
        {
            //if (!_competitionData.VaultingClass.ClassNr.StartsWith("28") && !_competitionData.VaultingClass.ClassNr.StartsWith("7"))
            //{
            //    return;
            //}
            string fileOutputname;
            if (StartOrderInfileName)
            {
                fileOutputname = GetOutputFilename(judgeTable, _competitionData.StartVaulterNumber.ToString());
            }
            else
            {
                fileOutputname = GetOutputFilename(judgeTable);

            }

            if (judgeTable == null)
            {
                judgeTable = new JudgeTable();
                judgeTable.JudgeTableName = JudgeTableNames.Okänd;
            }
            if (excelWorksheetNameJudgesTable == null)
                return;
            var worksheet = _competitionData.Workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTable);


            SetWorksheetTeam(worksheet, judgeTable);

                _excelBaseService.ShowOnlyWorksheet(worksheet);
              

                SaveExcelFile(fileOutputname);

            //New
            //// Copy template -> output (overwrite if exists)
            // Copy template to output directory using relative paths
            var workingDirectory = HttpContext.Current.Server.MapPath("~");
            var templatePath = System.IO.Path.Combine(workingDirectory, _competitionData.InputFileName);

            // Build output directory path with class name
            var outputFilePath = System.IO.Path.Combine(
                workingDirectory,
                "..",
                "output",
                fileOutputname
            );

            // Create output directory if it doesn't exist
            //if (!System.IO.Directory.Exists(outputDir))
            //{
            //    System.IO.Directory.CreateDirectory(outputDir);
            //}

            // Define output file path
            //var outputFilePath = System.IO.Path.Combine(outputDir, fileOutputname);
            outputFilePath = outputFilePath.Replace(".xlsx", "_v2.xlsx");
            System.IO.File.Copy(templatePath, outputFilePath, true);
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(outputFilePath, true))
            {
                WorkbookPart workbookPart = doc.WorkbookPart;

                SetIdInSheet(workbookPart, excelWorksheetNameJudgesTable, judgeTable.JudgeTableName);
                SetJudgeName(workbookPart, excelWorksheetNameJudgesTable, judgeTable.JudgeName);
                Sheet sheet = workbookPart.Workbook.Sheets.Cast<Sheet>().FirstOrDefault(s => s.Name == excelWorksheetNameJudgesTable);
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                Worksheet worksheetOPenXml = worksheetPart.Worksheet;
                _excelBaseService.ShowOnlyWorksheetOpenXml(workbookPart, worksheetOPenXml);
                //worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();



            }
        }

        private void SetWorksheetTeam(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            if (ContestService.IsTraHastTavling())
            {
                SetHorsePoints(worksheet);
            }

            SetIdInSheet(worksheet, judgeTable.JudgeTableName);
           
            switch (worksheet.Name)

            {
                //case "Häst, individuell":
                //    SetWorksheetHorse(worksheet, judgeTable);
                //    break;
                //case "Individuell minior grund 1":
                //    SetWorksheetIndividuellMiniorGrund1(worksheet, judgeTable);
                //    break;
                //case "Individuell junior grund 2":
                //    SetWorksheetIndividuellJuniorGrund2(worksheet, judgeTable);
                //    break;
                //case "Individuell senior grund 3":
                //    SetWorksheetIndividuellSeniorGrund3(worksheet, judgeTable);
                //    break;
                //case "Ind kür tekn 1":
                //    SetWorksheetIndkürtekn1(worksheet, judgeTable);
                //    break;
                //case "Ind kür tekn 2 3":
                //    SetWorksheetIndkürtekn2_3(worksheet, judgeTable);
                //    break;
                //case "Individuell kür artistisk":
                //    SetWorksheetIndKurArtistisk(worksheet, judgeTable);
                //    break;
                //case "Individuell tekniska övningar":
                //    SetWorksheetIndTekniskaOvningar(worksheet, judgeTable);
                //    break;
                //case "Individuellt tekniskt artistisk":
                //    SetWorksheetIndTekniskArtistisk(worksheet, judgeTable);
                //    break;
                default:
                    SetWorksheetDefault(worksheet, judgeTable);
                    break;
            }


        }

        private void SetIdInSheet(WorkbookPart workbookPart, string worksheetName, JudgeTableNames judgeTable)
        {
            string idString = ContestService.GetTeamExcelId(_competitionData.Team1, _competitionData.Horse1.HorseId, _competitionData.TestNumber, judgeTable);

            Sheet sheet = workbookPart.Workbook.Sheets.Cast<Sheet>()
               .FirstOrDefault(s => s.Name == worksheetName);

            WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
            Worksheet worksheet = worksheetPart.Worksheet;

            string cellCordinates = _excelBaseService.GetCellRef(workbookPart, worksheetName, "id");
            _excelBaseService.SetValueInWorksheetOpenXML(worksheet, cellCordinates, idString);
            _excelBaseService.HideColumnOpenXml(workbookPart, worksheet, cellCordinates);
        }

        /// <summary>
        /// Hides the column that contains the given cell address in the provided Worksheet (OpenXML).
        /// </summary>
        


        private void SetIdInSheet(IXLWorksheet worksheet, JudgeTableNames judgeTable)
        {
            string idString = ContestService.GetTeamExcelId(_competitionData.Team1, _competitionData.Horse1.HorseId, _competitionData.TestNumber, judgeTable);
            var cell = _excelBaseService.SetValueInWorksheet(worksheet, "id", idString);
            cell?.WorksheetColumn().Hide();
        }

        private void SetWorksheetDefault(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            SetHeaderPostfix(worksheet);

            SetFirstInformationGroup(worksheet, 4);
            SetTeamInformation(worksheet, judgeTable.JudgeTableName, 2);

            SetJudgeName(worksheet, 32, judgeTable.JudgeName);
        }

        private void SetTeamInformation(IXLWorksheet worksheet, JudgeTableNames judgeTableName, int startRow)
        {
            var startNumber = GetStartNumberForVaulterString();
            SetInformationGroup2(worksheet, judgeTableName, startRow, startNumber);
            SetMemberNames(worksheet, startRow + 5);
        }

        private string GetStartNumberForVaulterString()
        {
            return _competitionData.StartVaulterNumber.ToString();
        }

        protected void SetMemberNames(IXLWorksheet worksheet, int startRow)
        {
            var firstcell = _excelBaseService.GetNamedCell(worksheet, "firstvaulter");
            //string tableName = GetJudgeTableName(judgeTable);
            int offset = 0;
            foreach (var vaulter in _competitionData.GetTeamVaultersSorted())
            {
                firstcell.CellBelow(offset).Value = vaulter.Value?.Name?.Trim();
                //SetValueInWorksheet(worksheet, startRow, "h", vaulter.Value?.Name);
                //startRow++;
                offset++;
            }
           
            
        }


    }
}