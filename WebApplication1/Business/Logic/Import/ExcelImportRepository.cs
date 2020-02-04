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
                var tdbId = row.Cell("a").Value?.ToString();
                if (!string.IsNullOrWhiteSpace(tdbId))
                {
                    horse.HorseTdbId = Convert.ToInt32(tdbId);
                }
                horse.HorseName = row.Cell("b").Value.ToString();
                horses.Add(horse);
            }

            return horses.ToArray();
        }

        public Vaulter[] GetVaulters()
        {
            int vaulterTdbId;
            var vaulters = new List<Vaulter>();
            var worksheet = _workbook.Worksheets?.Worksheet("voltigörer");
            foreach (var row in worksheet.Rows())
            {

                if (row.Cell("a").Value is string && string.IsNullOrWhiteSpace((string)row.Cell("a").Value))
                {
                    continue;
                }
                vaulterTdbId = Convert.ToInt32(row.Cell("a").Value);

                var vaulter = new Vaulter
                {
                    VaulterTdbId = vaulterTdbId,
                    Name = row.Cell("b").Value.ToString()
                };
                var armBand = (string) row.Cell("c").Value;
                if (!string.IsNullOrWhiteSpace(armBand))
                {
                    vaulter.Armband = armBand;
                }
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
                    ClubName = GetString(row, "b"),
                    Country  = GetString(row,"c")
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
                    ClassName = GetString(row, "c"),
                    ScoreSheetId = GetInt(row,"d",0)
                };
                competitionClasses.Add(competitionClass);
            }

            return competitionClasses.ToArray();
        }

        //public CompetitionClass[] GetStartListClass()
        //{
        //    var startListClassStepsList = new List<StartListClassStep>();
        //    var worksheet = _workbook.Worksheets?.Worksheet("startlisteklass");
        //    foreach (var row in worksheet.Rows())
        //    {
        //        var startListClassStep = new StartListClassStep
        //        {
        //            //ClassTdbId = GetInt(row, "a"),
        //            Name = = GetString(row, "b"),
        //            ClassName = Get(row, "c"),
        //            ScoreSheetId = GetInt(row, "d", 0)
        //        };
        //        startListClassStepsList.Add(startListClassStep);
        //    }

        //    return startListClassStepsList.ToArray();
        //}

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
            var vaulterId2 = GetInt(row, "n");
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
                TeamName = GetString(row, "k", clubName + className),
                VaulterId1 = GetInt(row, "l"),
                VaulterName1 = GetString(row, "m"),
                VaulterId2 = vaulterId2,
                VaulterName2 = GetString(row, "o"),
                VaulterId3 = GetInt(row, "p"),
                VaulterName3 = GetString(row, "q"),
                VaulterId4 = GetInt(row, "r"),
                VaulterName4 = GetString(row, "s"),
                VaulterId5 = GetInt(row, "t"),
                VaulterName5 = GetString(row, "u"),
                VaulterId6 = GetInt(row, "v"),
                VaulterName6 = GetString(row, "w"),
                VaulterId7 = GetInt(row, "x"),
                VaulterName7 = GetString(row, "y"),
                VaulterId8 = GetInt(row, "z"),
                VaulterName8 = GetString(row, "aa"),
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

        private static DateTime GetDate(IXLRow row, string cell)
        {
            DateTime returnValue;
            var cellValue = row?.Cell(cell)?.Value?.ToString();

            if (DateTime.TryParse(cellValue, out returnValue))
            {
                return returnValue;
            }

            return DateTime.Now;
        }
    }
}