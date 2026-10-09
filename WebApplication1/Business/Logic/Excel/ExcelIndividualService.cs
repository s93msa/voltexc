using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Business.Logic.Excel.OpenXml;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Excel
{
    public class ExcelIndividualService : ExcelScorecardBaseService
    {
        public ExcelIndividualService(ExcelPreCompetitionData competitionInformation)
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
            if (string.IsNullOrWhiteSpace(sheetName)) return;

            string? fileOutputname = StartOrderInfileName
                ? GetOutputFilename(judgeTable, _competitionData.StartVaulterNumber.ToString())
                : GetOutputFilename(judgeTable);
            if (fileOutputname == null) return;

            SaveAsScorecard(fileOutputname, writer =>
            {
                if (!writer.SheetExists(sheetName)) return;

                writer.SetActiveSheet(sheetName);
                PopulateSheet(writer, sheetName, judgeTable);
                writer.ShowOnlySheet(sheetName);
            });
        }

        private void PopulateSheet(ScorecardWriter writer, string sheetName, JudgeTable judgeTable)
        {
            string idString = ContestService.GetVaulterExcelId(
                _competitionData.Vaulter1, _competitionData.Horse1.HorseId,
                _competitionData.TestNumber, judgeTable);
            SetIdAndHide(writer, sheetName, idString);

            if (ContestService.IsTraHastTavling())
                SetHorsePoints(writer, sheetName);

            switch (sheetName)
            {
                case "Häst, individuell":
                    SetFirstInformationGroup(writer);
                    SetInformationGroup2(writer, judgeTable, _competitionData.StartVaulterNumber.ToString());
                    SetJudgeName(writer, judgeTable);
                    break;
                default:
                    SetHeaderPostfix(writer);
                    SetFirstInformationGroup(writer);
                    SetInformationGroup2(writer, judgeTable, _competitionData.StartVaulterNumber.ToString());
                    SetJudgeName(writer, judgeTable);
                    break;
            }
        }
    }
}
