using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Business.Logic.Import;

namespace VoltigeCore.Business.Logic.Excel
{
    public class ExportStartListService
    {
        private const string ExcelFileExtension = ".xlsx";
        private ExcelBaseService _excelBaseService;
        private string _ExcelPathAndName;

        public ExportStartListService()
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
                var columns = new List<Cell<string>>();
                columns.Add(BoldCell(startlistClass.StartListClassStepId.ToString()));
                columns.Add(BoldCell(startlistClass.Name));
                rows.Add(new Row<Cell<string>>(columns.ToArray()));

                foreach (var startListItem in startlistClass.GetActiveStartList().OrderBy(x => x.StartNumber))
                {
                    if (startListItem.IsTeam)
                    {
                        columns = new List<Cell<string>>();
                        columns.Add(new Cell<string>(startListItem.VaultingTeam.VaultingClass.ClassTdbId.ToString()));
                        columns.Add(new Cell<string>(startListItem.VaultingTeam.VaultingClass.ClassNr));
                        columns.Add(new Cell<string>(startListItem.VaultingTeam.VaultingClass.ClassName));
                        columns.Add(new Cell<string>(startListItem.HorseInformation.Lunger.LungerTdbId.ToString()));
                        columns.Add(new Cell<string>(startListItem.HorseInformation.Lunger.LungerName));
                        columns.Add(new Cell<string>(startListItem.HorseInformation.HorseTdbId.ToString()));
                        columns.Add(new Cell<string>(startListItem.HorseInformation.HorseName));
                        columns.Add(new Cell<string>(""));
                        columns.Add(new Cell<string>(""));
                        columns.Add(new Cell<string>(startListItem.VaultingTeam.VaultingClub.ClubTdbId.ToString()));
                        columns.Add(new Cell<string>(startListItem.VaultingTeam.VaultingClub.ClubName));
                        columns.Add(new Cell<string>(""));
                        columns.Add(new Cell<string>(startListItem.VaultingTeam.Name));
                        foreach (var teamItem in startListItem.VaultingTeam.TeamList.OrderBy(x => x.StartNumber))
                        {
                            columns.Add(new Cell<string>(teamItem.Participant.VaulterTdbId.ToString()));
                            columns.Add(new Cell<string>(teamItem.Participant.Name));
                        }
                        rows.Add(new Row<Cell<string>>(columns.ToArray()));
                    }
                    else
                    {
                        foreach (var vaulterItem in startListItem.GetActiveVaulters().OrderBy(x => x.StartOrder))
                        {
                            columns = new List<Cell<string>>();
                            columns.Add(new Cell<string>(vaulterItem.Participant.VaultingClass.ClassTdbId.ToString()));
                            columns.Add(new Cell<string>(vaulterItem.Participant.VaultingClass.ClassNr));
                            columns.Add(new Cell<string>(vaulterItem.Participant.VaultingClass.ClassName));
                            columns.Add(new Cell<string>(startListItem.HorseInformation.Lunger.LungerTdbId.ToString()));
                            columns.Add(new Cell<string>(startListItem.HorseInformation.Lunger.LungerName));
                            columns.Add(new Cell<string>(startListItem.HorseInformation.HorseTdbId.ToString()));
                            columns.Add(new Cell<string>(startListItem.HorseInformation.HorseName));
                            columns.Add(new Cell<string>(""));
                            columns.Add(new Cell<string>(""));
                            columns.Add(new Cell<string>(vaulterItem.Participant.VaultingClub.ClubTdbId.ToString()));
                            columns.Add(new Cell<string>(vaulterItem.Participant.VaultingClub.ClubName));
                            columns.Add(new Cell<string>(""));
                            columns.Add(new Cell<string>(""));
                            columns.Add(new Cell<string>(vaulterItem.Participant.VaulterTdbId.ToString()));
                            columns.Add(new Cell<string>(vaulterItem.Participant.Name));
                            rows.Add(new Row<Cell<string>>(columns.ToArray()));
                        }
                    }
                }
            }
            return rows;
        }

        public void SetStartList()
        {
            var rows = CreateStartlist();
            _excelBaseService.SetValuesInWorkSheet("startlista", 1, rows);
        }

        private static Cell<string> BoldCell(string cellValue)
        {
            var cell = new Cell<string>(cellValue);
            cell.FontStyle = ExcelFontStyle.Bold;
            return cell;
        }
    }
}
