using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Import
{
    public class ExcelImportRepository
    {
        private XLWorkbook _workbook;

        public ExcelImportRepository(XLWorkbook workbook)
        {
            this._workbook = workbook;
            _workbook = workbook;




            //var fileName = Path.GetFileName(file.FileName);
            //var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            //file.SaveAs(path);
        }
        public ExcelImportMergedModel[] GetMergedInfo()
        {
            var excelImportMergedList = new List<ExcelImportMergedModel>();
            var worksheet = _workbook.Worksheets?.Worksheet("ekipage");
            foreach (var row in worksheet.Rows())
            {
                var excelImportMergedModel = GetAllRowInformation(row);
                excelImportMergedList.Add(excelImportMergedModel);
            }
            return excelImportMergedList.ToArray();
        }

       
        public Horse[] GetHorses()
        {
            var horses = new List<Horse>();
            var worksheet = _workbook.Worksheets?.Worksheet("hästar");
            foreach (var row in worksheet.Rows())
            {
                var horse = new Horse();
                horse.HorseTdbId = Convert.ToInt32(row.Cell("a").Value);
                horse.HorseName = row.Cell("b").Value.ToString();
                horses.Add(horse);
            }

            return horses.ToArray();
        }

        public Vaulter[] GetVaulters()
        {
            var vaulters = new List<Vaulter>();
            var worksheet = _workbook.Worksheets?.Worksheet("tävlande");
            foreach (var row in worksheet.Rows())
            {
                var vaulter = new Vaulter();
                vaulter.VaulterTdbId = Convert.ToInt32(row.Cell("a").Value);
                vaulter.Name = row.Cell("b").Value.ToString();
                vaulters.Add(vaulter);
            }

            return vaulters.ToArray();
        }

        public Vaulter[] GetTeamVaulters()
        {
            var vaulters = new List<Vaulter>();
            var worksheet = _workbook.Worksheets?.Worksheet("ekipage");
            foreach (var row in worksheet.Rows())
            {
                vaulters = AddVaulter(row, vaulters, "m", "n");
                vaulters = AddVaulter(row, vaulters, "o", "p");
                vaulters = AddVaulter(row, vaulters, "q", "r");
                vaulters = AddVaulter(row, vaulters, "s", "t");
                vaulters = AddVaulter(row, vaulters, "u", "v");
            }

            return vaulters.ToArray();
        }

       


        private static List<Vaulter> AddVaulter(IXLRow row, List<Vaulter> vaulters, string tdbIdColumn, string nameColumn)
        {
            int tdbId;
            if (row.Cell(tdbIdColumn).TryGetValue(out tdbId))
            {
                var vaulter = new Vaulter
                {
                    VaulterTdbId = tdbId,
                    Name = row.Cell(nameColumn).Value.ToString()
                };
                vaulters.Add(vaulter);
            }
            return vaulters;
        }

        public Club[] GetClubs()
        {
            var clubs = new List<Club>();
            var worksheet = _workbook.Worksheets?.Worksheet("klubbar");
            foreach (var row in worksheet.Rows())
            {
                var club = new Club
                {
                    ClubTdbId = GetInt(row, "a"),
                    ClubName = GetString(row, "b")
                };
                clubs.Add(club);
            }

            return clubs.ToArray();
        }

        public CompetitionClass[] GetClasses()
        {
            var competitionClasses = new List<CompetitionClass>();
            var worksheet = _workbook.Worksheets?.Worksheet("klasser");
            foreach (var row in worksheet.Rows())
            {
                var competitionClass = new CompetitionClass
                {
                    ClassTdbId = GetInt(row, "a"),
                    ClassNr = GetString(row, "b"),
                    ClassName = GetString(row, "c")
                };
                competitionClasses.Add(competitionClass);
            }

            return competitionClasses.ToArray();
        }

        public Lunger[] GetLungers()
        {
            var lungers = new List<Lunger>();
            var worksheet = _workbook.Worksheets?.Worksheet("linförare");
            //            var worksheet = _workbook.Worksheets?.Worksheet("Voltigörer");
            foreach (var row in worksheet.Rows())
            {
                var lunger = new Lunger();
                lunger.LungerTdbId = GetInt(row, "a");
                lunger.LungerName = GetString(row, "b");
                lungers.Add(lunger);
            }

            return lungers.ToArray();


        }

        

        private static ExcelImportMergedModel GetAllRowInformation(IXLRow row)
        {
            var vaulterId2 = GetInt(row, "m");
            var clubName = GetString(row, "i");
            var className = GetString(row, "c");
            var excelImportMergedModel = new ExcelImportMergedModel
            {
                ClassTdbId = GetInt(row, "a"),
                ClassNr = GetString(row, "b"),
                ClassName = className,
                LungerTdbId = GetInt(row, "d"),
                LungerName = GetString(row, "e"),
                HorseTdbId = GetInt(row, "f"),
                HorseName = GetString(row, "g"),
                ClubTdbId = GetInt(row, "h"),
                ClubName = clubName,
                VaulterId1 = GetInt(row, "k"),
                VaulterName1 = GetString(row, "l"),
                VaulterId2 = vaulterId2,
                VaulterName2 = GetString(row, "n"),
                VaulterId3 = GetInt(row, "o"),
                VaulterName3 = GetString(row, "p"),
                VaulterId4 = GetInt(row, "q"),
                VaulterName4 = GetString(row, "r"),
                VaulterId5 = GetInt(row, "s"),
                VaulterName5 = GetString(row, "t"),
                VaulterId6 = GetInt(row, "u"),
                VaulterName6 = GetString(row, "v"),
                TeamName = GetString(row, "w", clubName + className),
                IsTeam = !IsEmpty(vaulterId2)
            };
            return excelImportMergedModel;
        }

        private static bool IsEmpty(int vaulterId2)
        {
            return vaulterId2 <= 0;
        }

        private static string GetString(IXLRow row, string cell, string defaultValue = "")
        {
            var returnValue = row?.Cell(cell)?.Value?.ToString().Trim();

            if(defaultValue != "" && string.IsNullOrWhiteSpace(returnValue))
            {
                return defaultValue;
            }

            return returnValue;
        }
        private static int GetInt(IXLRow row, string cell, int defaultValue = 0)
        {
            int returnValue;
            var cellValue = row?.Cell(cell)?.Value?.ToString();

            if (int.TryParse(cellValue, out returnValue))
            {
                return returnValue;
            }

            return defaultValue;
        }
    }
}