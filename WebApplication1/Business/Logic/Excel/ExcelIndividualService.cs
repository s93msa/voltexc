using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Excel
{
    public class ExcelIndividualService : ExcelBaseService
    {
        private readonly ExcelPreCompetitionData _competitionData;
       
        public ExcelIndividualService(ExcelPreCompetitionData competitionInformation) : base(competitionInformation)
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

        private void SetIdInSheet(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            string idString = ContestService.GetVaulterExcelId(_competitionData.Vaulter1, _competitionData.TestNumber, judgeTable);
            var cell = SetValueInWorksheet(worksheet, "id", idString);
            cell?.WorksheetColumn().Hide();
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
            if (string.IsNullOrWhiteSpace(excelWorksheetNameJudgesTable))
                return;

            var worksheet = _competitionData.Workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTable);

            SetWorksheetIndividuell(worksheet, judgeTable);

            ShowOnlyWorksheet(worksheet);
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


            

        private void SetWorksheetIndividuell(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            SetIdInSheet(worksheet, judgeTable);
            if (ContestService.IsTraHastTavling())
            {
                SetHorsePoints(worksheet);
            }

            switch (worksheet.Name)

            {
                case "Häst, individuell":
                    SetWorksheetHorse(worksheet, judgeTable);
                    break;
                case "Individuell minior grund 1":
                    SetWorksheetIndividuellMiniorGrund1(worksheet,judgeTable);
                    break;
                case "Individuell junior grund 2":
                    SetWorksheetIndividuellJuniorGrund2(worksheet, judgeTable);                    
                    break;
                case "Individuell senior grund 3":
                    SetWorksheetIndividuellSeniorGrund3(worksheet, judgeTable);
                    break;
                case "Ind kür tekn 1":
                    SetWorksheetIndkürtekn1(worksheet, judgeTable);
                    break;
                case "Ind kür tekn 2 3":
                    SetWorksheetIndkürtekn2_3(worksheet, judgeTable);
                    break;
                case "Individuell kür artistisk":
                    SetWorksheetIndKurArtistisk(worksheet, judgeTable);
                    break;
                case "Individuell tekniska övningar":
                    SetWorksheetIndTekniskaOvningar(worksheet, judgeTable);
                    break;
                case "Individuellt tekniskt artistisk":
                    SetWorksheetIndTekniskArtistisk(worksheet, judgeTable);
                    break;
                default:
                    SetWorksheetIndividuellDefault(worksheet, judgeTable);
                    break;
            }


        }

        


        private void SetWorksheetIndividuellDefault(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 32, judgeTable);


        }

        private void SetWorksheetHorse(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            SetFirstInformationGroup(worksheet, 3);

            SetVaulterInformation(worksheet, judgeTable, 1);
     
            SetJudgeName(worksheet, 29, judgeTable);

           
        }

       

       


        private void SetWorksheetIndividuellMiniorGrund1(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);
            SetJudgeName(worksheet, 32, judgeTable);
        }

        private void SetWorksheetIndividuellJuniorGrund2(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 32, judgeTable);


        }

        private void SetWorksheetIndividuellSeniorGrund3(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 32, judgeTable);


        }

        private void SetWorksheetIndkürtekn1(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 37, judgeTable);
        }

        private void SetWorksheetIndkürtekn2_3(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 37, judgeTable);
        }
        private void SetWorksheetIndKurArtistisk(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 27, judgeTable);
        }
        private void SetWorksheetIndTekniskaOvningar(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 34, judgeTable);
        }

        private void SetWorksheetIndTekniskArtistisk(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 3);

            SetJudgeName(worksheet, 28, judgeTable);
        }


        private void SetVaulterInformation(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        {
            var startNumber = GetStartNumberForVaulterString();
            SetInformationGroup2(worksheet, judgeTable, startRow, startNumber);
        }

        private string GetStartNumberForVaulterString()
        {
            return _competitionData.StartVaulterNumber.ToString();
        }

    }
}