using ClosedXML.Excel;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Excel
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
            if (excelWorksheetNameJudgesTable == null) return;

            var worksheet = _competitionData.Workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTable);
            SetWorksheetTeam(worksheet, judgeTable);
            _excelBaseService.ShowOnlyWorksheet(worksheet);
            string fileOutputname = StartOrderInfileName
                ? GetOutputFilename(judgeTable, _competitionData.StartVaulterNumber.ToString())
                : GetOutputFilename(judgeTable);
            SaveExcelFile(fileOutputname);
        }

        private void SetWorksheetTeam(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            if (ContestService.IsTraHastTavling())
                SetHorsePoints(worksheet);
            SetIdInSheet(worksheet, judgeTable);
            SetWorksheetDefault(worksheet, judgeTable);
        }

        private void SetIdInSheet(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            string idString = ContestService.GetTeamExcelId(_competitionData.Team1, _competitionData.Horse1.HorseId, _competitionData.TestNumber, judgeTable);
            var cell = _excelBaseService.SetValueInWorksheet(worksheet, "id", idString);
            cell?.WorksheetColumn().Hide();
        }

        private void SetWorksheetDefault(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            SetHeaderPostfix(worksheet);
            SetFirstInformationGroup(worksheet, 4);
            SetTeamInformation(worksheet, judgeTable, 2);
            SetJudgeName(worksheet, 32, judgeTable);
        }

        private void SetTeamInformation(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        {
            SetInformationGroup2(worksheet, judgeTable, startRow, _competitionData.StartVaulterNumber.ToString());
            SetMemberNames(worksheet, judgeTable, startRow + 5);
        }

        protected void SetMemberNames(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        {
            var firstcell = _excelBaseService.GetNamedCell(worksheet, "firstvaulter");
            int offset = 0;
            foreach (var vaulter in _competitionData.GetTeamVaultersSorted())
            {
                firstcell.CellBelow(offset).Value = vaulter.Value?.Name?.Trim();
                offset++;
            }
        }
    }
}
