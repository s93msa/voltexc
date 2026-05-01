using ClosedXML.Excel;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Excel
{
    public class ExcelIndividualService : ExcelScorecardBaseService
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
            string idString = ContestService.GetVaulterExcelId(_competitionData.Vaulter1, _competitionData.Horse1.HorseId, _competitionData.TestNumber, judgeTable);
            var cell = _excelBaseService.SetValueInWorksheet(worksheet, "id", idString);
            cell?.WorksheetColumn().Hide();
        }

        private void CreateExcelFromValuesJudgeA() => CreateExcelFromValues(_competitionData.ExcelWorksheetNameJudgesTableA?.Trim(), _competitionData.JudgeTableA);
        private void CreateExcelFromValuesJudgeB() => CreateExcelFromValues(_competitionData.ExcelWorksheetNameJudgesTableB?.Trim(), _competitionData.JudgeTableB);
        private void CreateExcelFromValuesJudgeC() => CreateExcelFromValues(_competitionData.ExcelWorksheetNameJudgesTableC?.Trim(), _competitionData.JudgeTableC);
        private void CreateExcelFromValuesJudgeD() => CreateExcelFromValues(_competitionData.ExcelWorksheetNameJudgesTableD?.Trim(), _competitionData.JudgeTableD);

        private void CreateExcelFromValues(string excelWorksheetNameJudgesTable, JudgeTable judgeTable)
        {
            if (judgeTable == null)
            {
                judgeTable = new JudgeTable();
                judgeTable.JudgeTableName = JudgeTableNames.Okänd;
            }
            if (string.IsNullOrWhiteSpace(excelWorksheetNameJudgesTable)) return;

            var worksheet = _competitionData.Workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTable);
            SetWorksheetIndividuell(worksheet, judgeTable);
            _excelBaseService.ShowOnlyWorksheet(worksheet);
            string fileOutputname = StartOrderInfileName
                ? GetOutputFilename(judgeTable, _competitionData.StartVaulterNumber.ToString())
                : GetOutputFilename(judgeTable);
            SaveExcelFile(fileOutputname);
        }

        private void SetWorksheetIndividuell(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            SetIdInSheet(worksheet, judgeTable);
            if (ContestService.IsTraHastTavling())
                SetHorsePoints(worksheet);

            switch (worksheet.Name)
            {
                case "Häst, individuell":
                    SetWorksheetHorse(worksheet, judgeTable); break;
                case "Individuell minior grund 1":
                    SetWorksheetIndividuellMiniorGrund1(worksheet, judgeTable); break;
                case "Individuell junior grund 2":
                    SetWorksheetIndividuellJuniorGrund2(worksheet, judgeTable); break;
                case "Individuell senior grund 3":
                    SetWorksheetIndividuellSeniorGrund3(worksheet, judgeTable); break;
                case "Ind kür tekn 1":
                    SetWorksheetIndkürtekn1(worksheet, judgeTable); break;
                case "Ind kür tekn 2 3":
                    SetWorksheetIndkürtekn2_3(worksheet, judgeTable); break;
                case "Individuell kür artistisk":
                    SetWorksheetIndKurArtistisk(worksheet, judgeTable); break;
                case "Individuell tekniska övningar":
                    SetWorksheetIndTekniskaOvningar(worksheet, judgeTable); break;
                case "Individuellt tekniskt artistisk":
                    SetWorksheetIndTekniskArtistisk(worksheet, judgeTable); break;
                default:
                    SetWorksheetIndividuellDefault(worksheet, judgeTable); break;
            }
        }

        private void SetWorksheetIndividuellDefault(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            SetHeaderPostfix(worksheet);
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
            SetInformationGroup2(worksheet, judgeTable, startRow, _competitionData.StartVaulterNumber.ToString());
        }
    }
}
