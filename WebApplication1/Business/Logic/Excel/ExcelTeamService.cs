using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Business.Logic.Excel.OpenXml;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Excel
{
    public class ExcelTeamService : ExcelScorecardBaseService
    {
        public ExcelTeamService(ExcelPreCompetitionData competitionInformation)
            : base(competitionInformation) { }

        public void CreateExcelforIndividual()
        {
            CreateExcelForJudge(_competitionData.ExcelWorksheetNameJudgesTableA?.Trim(), _competitionData.JudgeTableA);
            CreateExcelForJudge(_competitionData.ExcelWorksheetNameJudgesTableB?.Trim(), _competitionData.JudgeTableB);
            CreateExcelForJudge(_competitionData.ExcelWorksheetNameJudgesTableC?.Trim(), _competitionData.JudgeTableC);
            CreateExcelForJudge(_competitionData.ExcelWorksheetNameJudgesTableD?.Trim(), _competitionData.JudgeTableD);
        }

        private void CreateExcelForJudge(string? sheetName, JudgeTable? judgeTable)
        {
            if (judgeTable == null)
                judgeTable = new JudgeTable { JudgeTableName = JudgeTableNames.Okänd };
            if (string.IsNullOrEmpty(sheetName)) return;

            string? fileOutputname = StartOrderInfileName
                ? GetOutputFilename(judgeTable, _competitionData.StartVaulterNumber.ToString())
                : GetOutputFilename(judgeTable);
            if (fileOutputname == null) return;

            SaveAsScorecard(fileOutputname, writer =>
            {
                if (!writer.SheetExists(sheetName)) return;

                writer.SetActiveSheet(sheetName);

                if (ContestService.IsTraHastTavling())
                    SetHorsePoints(writer, sheetName);

                string idString = ContestService.GetTeamExcelId(
                    _competitionData.Team1, _competitionData.Horse1.HorseId,
                    _competitionData.TestNumber, judgeTable);
                SetIdAndHide(writer, sheetName, idString);

                SetHeaderPostfix(writer);
                SetFirstInformationGroup(writer);
                SetInformationGroup2(writer, judgeTable, _competitionData.StartVaulterNumber.ToString());
                SetMemberNames(writer);
                SetJudgeName(writer, judgeTable);

                writer.ShowOnlySheet(sheetName);
            });
        }

        private void SetMemberNames(ScorecardWriter writer)
        {
            if (!writer.TryResolveDefinedName("firstvaulter", out var firstcell)) return;

            uint offset = 0;
            foreach (var vaulter in _competitionData.GetTeamVaultersSorted())
            {
                writer.SetCellString(firstcell.Below(offset), vaulter.Value?.Name?.Trim() ?? "");
                offset++;
            }
        }
    }
}
