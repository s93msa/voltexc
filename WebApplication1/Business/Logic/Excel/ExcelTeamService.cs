using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using ClosedXML.Excel;
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
            string fileOutputname;
            if (StartOrderInfileName)
            {
                fileOutputname = GetOutputFilename(judgeTable, _competitionData.StartVaulterNumber.ToString());
            }
            else
            {
                fileOutputname = GetOutputFilename(judgeTable);

            }
            SaveExcelFile(fileOutputname);
        }

        private void SetWorksheetTeam(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            if (ContestService.IsTraHastTavling())
            {
                SetHorsePoints(worksheet);
            }

            SetIdInSheet(worksheet, judgeTable);
           
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

   

        private void SetIdInSheet(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            string idString = ContestService.GetTeamExcelId(_competitionData.Team1, _competitionData.Horse1.HorseId, _competitionData.TestNumber, judgeTable);
            var cell = _excelBaseService.SetValueInWorksheet(worksheet, "id", idString);
            cell?.WorksheetColumn().Hide();
        }

        private void SetWorksheetDefault(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetTeamInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 32, judgeTable);


        }

        private void SetTeamInformation(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        {
            var startNumber = GetStartNumberForVaulterString();
            SetInformationGroup2(worksheet, judgeTable, startRow, startNumber);
            SetMemberNames(worksheet, judgeTable, startRow + 5);
        }

        private string GetStartNumberForVaulterString()
        {
            return _competitionData.StartVaulterNumber.ToString();
        }

        protected void SetMemberNames(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
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