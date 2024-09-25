using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Business.Logic.Import;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Excel
{
    public class StartlistExportService
    {//Lag grund: 10 min/lag eller Lag kür: 8 min / lag PdD: 5 min/PdD
        //Individuella: grund och kür 1,5 min/häst + 2 min/voltigör
        public const string TEAMCOMPULSORY_CELL = "p1";

        private const string ExcelFileExtension = ".xlsx";
        private ExcelBaseService _excelBaseService;
        private string _ExcelPathAndName;
        public StartlistExportService()
        {
            var workingdirectory = System.Web.Hosting.HostingEnvironment.MapPath("~");
            _ExcelPathAndName = workingdirectory + @"..\output\startlist";
            var workbook = new XLWorkbook(_ExcelPathAndName + ExcelFileExtension);

            _excelBaseService = new ExcelBaseService(workbook);
        }

        public void Save()
        {
            _excelBaseService.SaveExcelFile();
        }
        public void SaveWithTimeStamp()
        {
            _excelBaseService.SaveExcelFile(_ExcelPathAndName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ExcelFileExtension);
        }

        public List<Row<Cell<string>>> CreateStartlist()
        {
            var rows = new List<Row<Cell<string>>>();
            var startList = new List<StartlistClass>();
            var contest = ContestService.GetContestInstance();
            var startlistclasses = contest.GetActiveStartListClassStep().OrderBy(x => x.StartOrder);
            foreach (var startlistClass in startlistclasses)
            {
                rows.Add(EmptyRow());
                List<Cell<string>> columns = ClassInformation(startlistClass);

                rows.Add(new Row<Cell<string>>(columns.ToArray()));
                columns = JudgesInformation(startlistClass);
                rows.Add(new Row<Cell<string>>(columns.ToArray()));

                int startnumer = 0;
                //var outputStartlistClass = new StartlistClass();
                //outputStartlistClass.startListClassId = startlistClass.StartListClassStepId;
                foreach (var startListItem in startlistClass.GetActiveStartList().OrderBy(x => x.StartNumber))
                {
                    if (startListItem.IsTeam)
                    {
                        double durationMinutes; //Lag grund: 10 min/lag eller Lag kür: 8 min / lag PdD: 5 min/PdD
                        if (startListItem.VaultingTeam.VaultingClass.ClassName.ToLower().Contains("pas de deux"))
                        {
                            durationMinutes = 5;
                        }
                        else if (startListItem.TeamTestnumber == 1)
                        {
                            durationMinutes = 10;
                        }
                        else
                        {
                            durationMinutes = 8;
                        }

                        startnumer++;
                        var testName = ExcelPreCompetitionData.GetCompetitionStep(contest.TypeOfContest, startListItem.VaultingTeam.VaultingClass, startListItem.TeamTestnumber)?.Name;
                        var classTestInformation = startListItem.VaultingTeam.VaultingClass.ClassName + " klass: " + startListItem.VaultingTeam.VaultingClass.ClassNr + " - " + testName;
                        columns = new List<Cell<string>>
                        {
                            //new Cell<string>(TEAMCOMPULSORY_CELL), //durationMinutes.ToString()),
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
                        {
                            teamParticipants.Add(teamItem.Participant.Name.Replace(" ", "\u00A0")); // \u00A0 = non-breaking space
                        }
                        columns = new List<Cell<string>>
                        {
                            new Cell<string>(""),
                            new Cell<string>(""),
                            new Cell<string>(""),
                            new Cell<string>(""),
                            new Cell<string>(""),
                            new Cell<string>(""),
                            new Cell<string>(""),
                            new Cell<string>(string.Join(", ", teamParticipants))
                        };
                        rows.Add(new Row<Cell<string>>(columns.ToArray()));
                        //columns = new List<Cell<string>>
                        //    {
                        //        new Cell<string>(""),
                        //    };
                        //rows.Add(new Row<Cell<string>>(columns.ToArray()));
                    }
                    else
                    {
                        var vaulters = startListItem.GetActiveVaulters().OrderBy(x => x.StartOrder).ToList();
                        double durationMinutes = 1.5 + (vaulters.Count() * 2); //Individuella: grund och kür 1,5 min/häst + 2 min/voltigör
                        var durationMinutesString = durationMinutes.ToString();
                        foreach (var vaulterItem in vaulters)
                        {
                            var testName = ExcelPreCompetitionData.GetCompetitionStep(contest.TypeOfContest, vaulterItem.Participant.VaultingClass, vaulterItem.Testnumber)?.Name;
                            var classTestInformation = vaulterItem.Participant.VaultingClass.ClassName + " klass: " + vaulterItem.Participant.VaultingClass.ClassNr + " - " + testName;
                            startnumer++;
                            columns = new List<Cell<string>>
                            {
                                //new Cell<string>(TEAMCOMPULSORY_CELL), //new Cell<string>(durationMinutesString),
                                new Cell<string>(durationMinutesString), 
                                new Cell<string>(""),
                                new Cell<string>(startnumer.ToString()),
                                new Cell<string>(vaulterItem.Participant.Name),
                                new Cell<string>(startListItem.HorseInformation.HorseName),
                                new Cell<string>(startListItem.HorseInformation.Lunger.LungerName),
                                new Cell<string>(classTestInformation),
                                new Cell<string>(vaulterItem.Participant.VaultingClub.ClubName),
                            };
                            rows.Add(new Row<Cell<string>>(columns.ToArray()));
                            //columns = new List<Cell<string>>
                            //{
                            //    new Cell<string>(""),
                            //};
                            //rows.Add(new Row<Cell<string>>(columns.ToArray()));
                            durationMinutesString = "";
                        }
                    }
                    columns = new List<Cell<string>>
                            {
                                new Cell<string>(""),
                            };
                    rows.Add(new Row<Cell<string>>(columns.ToArray()));
                    //    columns = new List<string>();
                    //    var horseLoungerVaulters = new HorseLoungerVaulters();
                    //    horseLoungerVaulters.excelRowsList
                    //    startlistForStartlistClass.
                }
                //outputStartlistClass.horseLoungerVaultersList =

                //startList.Add()
            }

            return rows;
        }

        private List<Cell<string>> JudgesInformation(StartListClassStep startlistClass)
        {
            List<Cell<string>> columns = new List<Cell<string>>
                {
                    new Cell<string>(""),
                    new Cell<string>(""),
                    new Cell<string>("")
                };

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
            var cell = BoldCell(judgesInformation.ToString());
            columns.Add(cell);
            return columns;
        }

        private static List<Cell<string>> ClassInformation(StartListClassStep startlistClass)
        {
            var columns = new List<Cell<string>>
                {
                    new Cell<string>(""),
                    new Cell<string>(""),
                    new Cell<string>("")
                };
            var cell = BoldCell(startlistClass.Name);
            columns.Add(cell);
            return columns;
        }

        private Row<Cell<string>> EmptyRow()
        {
            return new Row<Cell<string>>(new List<Cell<string>>());
        }

        

        public void SetStartList()
        {
            const string worksheetName = "startlista";

            var rows = CreateStartlist();

            _excelBaseService.SetValuesInWorkSheet(worksheetName, 1, rows);
            _excelBaseService.SetAutoRowHeight(worksheetName);
            //_excelBaseService.SetTimeConstants(worksheetName);
            _excelBaseService.SetFormulaInWorkSheet(worksheetName, rows.Count());
            _excelBaseService.ConvertToNumber(worksheetName, "A", "0.0");
            _excelBaseService.ConvertToNumber(worksheetName, "E", "0");
        }

        private static Cell<string> BoldCell(string cellValue)
        {
            var cell = new Cell<string>(cellValue);
            cell.FontStyle = ExcelFontStyle.Bold;
            return cell;
        }
    }
}