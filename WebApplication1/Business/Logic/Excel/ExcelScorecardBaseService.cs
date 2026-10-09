using System.IO;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Business.Logic.Excel.OpenXml;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Excel
{
    public abstract class ExcelScorecardBaseService
    {
        public bool StartOrderInfileName { get; set; } = false;

        protected readonly ExcelPreCompetitionData _competitionData;

        protected ExcelScorecardBaseService(ExcelPreCompetitionData competitionInformation)
        {
            _competitionData = competitionInformation;
        }

        // ARGB hex equivalents of the ClosedXML XLColor values previously used.
        private static readonly string[] ClassBackgroundColorsArgb =
        {
            "FFFFFFFF", "FFFFFFFF", "FF0000FF", "FF008000", "FFFF0000", "FFFFFF00",
            "FFFFFFFF", "FFFFFFFF", "FFFFFFFF", "FFFFFFFF", "FFFFFFFF", "FFFF0000", "FFFFFF00"
        };

        protected void SetHorsePoints(ScorecardWriter writer, string sheetName)
        {
            var horseSheetNames = new[] { "Häst, individuell", "Häst, lag", "Pas-de-Deux Häst" };
            if (System.Array.IndexOf(horseSheetNames, sheetName.Trim()) >= 0)
                SetAJudgeResult(writer);
            else
                SetLattClassHorseResult(writer);
        }

        private void SetAJudgeResult(ScorecardWriter writer)
        {
            if (writer.TryResolveDefinedName("result", out var addr))
                writer.SetCellNumber(addr, ContestService.HorsePointTraHastTavling());
        }

        private void SetLattClassHorseResult(ScorecardWriter writer)
        {
            if (writer.TryResolveDefinedName("Hästpoäng", out var addr))
                writer.SetCellNumber(addr, ContestService.HorsePointTraHastTavling());
        }

        protected void SetHeaderPostfix(ScorecardWriter writer)
        {
            if (!writer.TryResolveDefinedName("header", out var addr)) return;

            var headerPostfix = _competitionData.VaultingClass.ScoreSheet.HeaderPostfix;
            var current = writer.GetCellString(addr);
            if (string.IsNullOrEmpty(current) ||
                (!string.IsNullOrEmpty(headerPostfix) && !current.EndsWith(headerPostfix)))
            {
                writer.SetCellString(addr, current + " " + headerPostfix);
            }
        }

        protected void SetFirstInformationGroup(ScorecardWriter writer)
        {
            if (!writer.TryResolveDefinedName("datum", out var addr)) return;
            writer.SetCellString(addr, _competitionData.GetStepDate());
            writer.SetCellString(addr.Below(1), _competitionData.EventLocation);
            writer.SetCellString(addr.Below(2), _competitionData.GetName());
            writer.SetCellString(addr.Below(3), _competitionData.VaultingClubName);
            writer.SetCellString(addr.Below(4), _competitionData.Country);
            writer.SetCellString(addr.Below(5), RemoveNumberFromEnd(_competitionData.HorseName));
            writer.SetCellString(addr.Below(6), _competitionData.LungerName);
        }

        protected void SetJudgeName(ScorecardWriter writer, JudgeTable? judgeTable)
        {
            if (writer.TryResolveDefinedName("domare", out var addr))
                writer.SetCellString(addr, judgeTable?.JudgeName ?? "");
        }

        protected void SetInformationGroup2(ScorecardWriter writer, JudgeTable? judgeTable, string startNumber)
        {
            if (!writer.TryResolveDefinedName("bord", out var bord)) return;

            writer.SetCellString(bord.Above(1), startNumber);
            writer.SetCellString(bord, judgeTable?.JudgeTableName.ToString() ?? "");
            writer.SetCellString(bord.Below(1), _competitionData.VaultingClass.ClassNr);
            writer.SetCellString(bord.Below(2), _competitionData.MomentName);

            if (writer.TryResolveDefinedName("armnr", out var armnr))
            {
                writer.SetCellString(armnr, _competitionData.ArmNumber?.Trim() ?? "");

                if (int.TryParse(_competitionData.VaultingClass.ClassNr, out var classNr)
                    && classNr >= 1 && classNr <= ClassBackgroundColorsArgb.Length)
                {
                    writer.SetCellBackgroundColor(armnr.Below(1), ClassBackgroundColorsArgb[classNr - 1]);
                }
            }
        }

        protected void SetIdAndHide(ScorecardWriter writer, string sheetName, string idString)
        {
            if (!writer.TryResolveDefinedName("id", out var addr)) return;
            writer.SetCellString(addr, idString);
            writer.HideColumn(sheetName, addr.Column);
        }

        protected void SaveAsScorecard(string outputFileName, System.Action<ScorecardWriter> populate)
        {
            outputFileName = outputFileName.Replace("/", "");
            var outputPathAndName = SanitizeOutputPath(AppConfig.OutputPath + outputFileName);

            var templateBytes = TemplateCache.GetBytes(_competitionData.TemplatePath);
            using var writer = new ScorecardWriter(outputPathAndName, templateBytes);
            populate(writer);
        }

        private static string SanitizeOutputPath(string path) =>
            path.Replace("&", "och")
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("*", string.Empty);

        protected string? GetOutputFilename(JudgeTable? judgeTabel, string fileNamePrefix = "")
        {
            if (judgeTabel == null) return null;
            string pathPrefix = fileNamePrefix.Length > 0 ? "utskrift_" : "";

            var fileName = _competitionData.GetName().Replace("–", "").Replace(".xlsx", "");
            fileName = fileName.Trim() + '_' + judgeTabel.JudgeTableName +
                       "_klass" + _competitionData.VaultingClass.ClassNr + '_' + _competitionData.MomentName + "_" +
                       _competitionData.Horse1.HorseName.Trim() + '_' +
                       _competitionData.ListClassStep.Date.DayOfWeek.ToString().Substring(0, 2);

            var path = pathPrefix + _competitionData.ListClassStep.Date.ToShortDateString() +
                       Path.DirectorySeparatorChar + judgeTabel.JudgeTableName + Path.DirectorySeparatorChar +
                       _competitionData.ListClassStep.Name.Trim().Replace("–", "") + Path.DirectorySeparatorChar;

            return path + fileNamePrefix + fileName + ".xlsx";
        }

        private static string? RemoveNumberFromEnd(string? horseName)
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
