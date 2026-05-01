using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Excel
{
    public class StartlistExportService
    {
        public const string TEAMCOMPULSORY_CELL = "p1";
        private const string ExcelFileExtension = ".xlsx";
        private ExcelBaseService _excelBaseService;
        private string _ExcelPathAndName;

        public StartlistExportService()
        {
            _ExcelPathAndName = AppConfig.StartlistOutputPath + "startlist";
            var workbook = new XLWorkbook(_ExcelPathAndName + ExcelFileExtension);
            _excelBaseService = new ExcelBaseService(workbook);
        }

        public void Save() => _excelBaseService.SaveExcelFile();

        public void SaveWithTimeStamp()
        {
            _excelBaseService.SaveExcelFile(_ExcelPathAndName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ExcelFileExtension);
        }

        public List<Row<Cell<string>>> CreateStartlist()
        {
            var rows = new List<Row<Cell<string>>>();
            var contest = ContestService.GetContestInstance();
            var startlistclasses = contest.GetActiveStartListClassStep().OrderBy(x => x.StartOrder);

            foreach (var startlistClass in startlistclasses)
            {
                rows.Add(EmptyRow());
                rows.Add(new Row<Cell<string>>(ClassInformation(startlistClass).ToArray()));
                rows.Add(new Row<Cell<string>>(JudgesInformation(startlistClass).ToArray()));

                int startnumer = 0;
                foreach (var startListItem in startlistClass.GetActiveStartList().OrderBy(x => x.StartNumber))
                {
                    if (startListItem.IsTeam)
                    {
                        double durationMinutes;
                        if (startListItem.VaultingTeam.VaultingClass.ClassName.ToLower().Contains("pas de deux"))
                            durationMinutes = 5;
                        else if (startListItem.TeamTestnumber == 1)
                            durationMinutes = 10;
                        else
                            durationMinutes = 8;

                        startnumer++;
                        var testName = ExcelPreCompetitionData.GetCompetitionStep(contest.TypeOfContest, startListItem.VaultingTeam.VaultingClass, startListItem.TeamTestnumber)?.Name;
                        var classTestInformation = startListItem.VaultingTeam.VaultingClass.ClassName + " klass: " + startListItem.VaultingTeam.VaultingClass.ClassNr + " - " + testName;
                        var columns = new List<Cell<string>>
                        {
                            new Cell<string>(durationMinutes.ToString()),
                            new Cell<string>(""),
                            new Cell<string>(startnumer.ToString()),
                            new Cell<string>(startListItem.VaultingTeam.Name),
                            new Cell<string>(startListItem.HorseInformation.HorseName),
                            new Cell<string>(startListItem.HorseInformation.Lunger.LungerName),
                            new Cell<string>(classTestInformation),
                            new Cell<string>(startListItem.VaultingTeam.VaultingClub.ClubName),
                        };
                        rows.Add(new Row<Cell<string>>(columns.ToArray()));

                        var teamParticipants = new List<string>();
                        foreach (var teamItem in startListItem.VaultingTeam.TeamList.OrderBy(x => x.StartNumber))
                            teamParticipants.Add(teamItem.Participant.Name.Replace(" ", "\u00A0"));
                        rows.Add(new Row<Cell<string>>(new List<Cell<string>>
                        {
                            new Cell<string>(""), new Cell<string>(""), new Cell<string>(""),
                            new Cell<string>(""), new Cell<string>(""), new Cell<string>(""),
                            new Cell<string>(""), new Cell<string>(string.Join(", ", teamParticipants))
                        }.ToArray()));
                    }
                    else
                    {
                        var vaulters = startListItem.GetActiveVaulters().OrderBy(x => x.StartOrder).ToList();
                        double durationMinutes = 1.5 + (vaulters.Count() * 2);
                        var durationMinutesString = durationMinutes.ToString();
                        foreach (var vaulterItem in vaulters)
                        {
                            var testName = ExcelPreCompetitionData.GetCompetitionStep(contest.TypeOfContest, vaulterItem.Participant.VaultingClass, vaulterItem.Testnumber)?.Name;
                            var classTestInformation = vaulterItem.Participant.VaultingClass.ClassName + " klass: " + vaulterItem.Participant.VaultingClass.ClassNr + " - " + testName;
                            startnumer++;
                            rows.Add(new Row<Cell<string>>(new List<Cell<string>>
                            {
                                new Cell<string>(durationMinutesString), new Cell<string>(""),
                                new Cell<string>(startnumer.ToString()),
                                new Cell<string>(vaulterItem.Participant.Name),
                                new Cell<string>(startListItem.HorseInformation.HorseName),
                                new Cell<string>(startListItem.HorseInformation.Lunger.LungerName),
                                new Cell<string>(classTestInformation),
                                new Cell<string>(vaulterItem.Participant.VaultingClub.ClubName),
                            }.ToArray()));
                            durationMinutesString = "";
                        }
                    }
                    rows.Add(new Row<Cell<string>>(new List<Cell<string>> { new Cell<string>("") }.ToArray()));
                }
            }
            return rows;
        }

        private List<Cell<string>> JudgesInformation(StartListClassStep startlistClass)
        {
            var columns = new List<Cell<string>> { new Cell<string>(""), new Cell<string>(""), new Cell<string>("") };
            var judgesInformation = new StringBuilder();
            judgesInformation.AppendLine("Domare ");
            for (int judgeTable = 1; judgeTable <= 4; judgeTable++)
            {
                if (!string.IsNullOrWhiteSpace(startlistClass.GetJudgeName((JudgeTableNames)judgeTable)))
                {
                    var judgeTableName = ((JudgeTableNames)judgeTable);
                    judgesInformation.AppendLine(judgeTableName.ToString() + ": " + startlistClass.GetJudgeName(judgeTableName) + " ");
                }
            }
            columns.Add(BoldCell(judgesInformation.ToString()));
            return columns;
        }

        private static List<Cell<string>> ClassInformation(StartListClassStep startlistClass)
        {
            var columns = new List<Cell<string>> { new Cell<string>(""), new Cell<string>(""), new Cell<string>("") };
            columns.Add(BoldCell(startlistClass.Name));
            return columns;
        }

        private Row<Cell<string>> EmptyRow() => new Row<Cell<string>>(new List<Cell<string>>());

        public void SetStartList()
        {
            var rows = CreateStartlist();
            _excelBaseService.SetValuesInWorkSheet("startlista", 1, rows);
            _excelBaseService.SetAutoRowHeight("startlista");
            _excelBaseService.SetFormulaInWorkSheet("startlista", rows.Count());
            _excelBaseService.ConvertToNumber("startlista", "A", "0.0");
            _excelBaseService.ConvertToNumber("startlista", "E", "0");
        }

        private static Cell<string> BoldCell(string cellValue)
        {
            var cell = new Cell<string>(cellValue);
            cell.FontStyle = ExcelFontStyle.Bold;
            return cell;
        }
    }
}
