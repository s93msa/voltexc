using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CopyExcel()
        {
            Contest contest;
            List<StartListClass> startListClasses;
            using (var db = new VaultingContext())
            {
                contest = (from contestTable in db.Contests
                             select contestTable).FirstOrDefault();
                //var country = contest?.Country;
                //var eventLocation = contest?.Location; 
                startListClasses = contest?.StartListClass?? new List<StartListClass>();

                foreach (var startListClass in startListClasses)
                {
                    foreach (var startListClassStep in startListClass.StartListClassStep)
                    {
//                        var date = startListClassStep.Date;
                        //var isTeam = startListClassStep.IsTeam;

                        //SortedList<int,Vaulter> vaulters = new SortedList<int, Vaulter>();
                        //if (!isTeam)
                        SaveInExcel(contest,   startListClassStep);
                    }
                        
                }



                //foreach (var item in query)
                //{
                //    country = item.Country;
                //}
            }
            ////CreateFolders();
            //ViewBag.Message = "CopyExcel.";

            return View();
        }

        private static void SaveInExcel(Contest contest, StartListClassStep startListClassStep)
        {
            
            //{ 'Häst, individuell'!A1:XFD1048576}
            //{ 'Individuell minior grund 1'!A1:XFD1048576}
            //{ 'Individuell junior grund 2'!A1:XFD1048576}
            //{ 'Individuell senior grund 3'!A1:XFD1048576}
            //{ 'Ind kür tekn 1'!A1:XFD1048576}
            //{ 'Ind kür tekn 2 3'!A1:XFD1048576}
            //{ 'Individuell kür artistisk'!A1:XFD1048576}
            //{ 'Individuell tekniska övningar'!A1:XFD1048576}
            //{ 'Individuellt tekniskt artistisk'!A1:XFD1048576}


            //foreach (var judgesTable in startListClassStep.JudgeTables)
            //{
                //var tableChar = judgesTable.JudgeTableName;
                //var judgeName = judgesTable.JudgeName;

                foreach (var startListItem in startListClassStep.StartList)
                {
                    if (startListItem.Participant != null)
                    {
                        //var vaulterName = startListItem.Participant.Name;
                        var vaulterClass = startListItem.Participant.VaultingClass;
                        //var armNumber = startListItem.Participant.Armband;
                        //var vaulterClub = startListItem.Participant.VaultingClub?.ClubName;
                        //var startNumber = startListItem.StartNumber;

                        string fileName;
                        if (vaulterClass.ClassNr == 4)
                        {
                            
                            var step = GetCompetitionStep(startListItem);
                            var participant = startListItem.Participant;
                            var vaultingClubName = participant.VaultingClub?.ClubName;

                            var horse= startListItem.VaultingHorse;
                            var lungerName = horse.Lunger.LungerName;
                            var JudgeTableA = GetJudgeName(startListClassStep.JudgeTables, JudgeTableNames.A);
                            var JudgeTableB = GetJudgeName(startListClassStep.JudgeTables, JudgeTableNames.B);
                            var JudgeTableC = GetJudgeName(startListClassStep.JudgeTables, JudgeTableNames.C);
                            var JudgeTableD = GetJudgeName(startListClassStep.JudgeTables, JudgeTableNames.D);

                            var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
                            var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableA;
                            var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableA;
                            var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableA;
                            //startListClassStep
                            //vaulterClass.Steps


                            //fileName = @"C:\Temp\Test_Voligemallar\Protokoll 2017 individuell klass.xlsx";
                            fileName = vaulterClass.Excelfile;
                            var workbook = new XLWorkbook(fileName);

                            var worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableA);
                            SetWorksheetHorse(worksheet, contest, startListClassStep.Date, startListItem.StartNumber, startListItem.Participant,
                                vaultingClubName, horse.HorseName, lungerName, JudgeTableA);


                            //workbook.Worksheets.Worksheet(3).Visibility = XLWorksheetVisibility.Hidden;

                            //var worksheetHorse = workbook.Worksheet(2);
                            //worksheetHorse.Cell(7, "c").Value = country;
                            workbook.SaveAs(@"C:\Temp\Test_Voligemallar\HelloWorld.xlsx");
                        }


                        // worksheetHorse.Cell(7, "d").Value = d?.Count;

                    //}
                }
            }
        }

        private static string GetJudgeName(List<JudgeTable> judgeTables, JudgeTableNames tableName)
        {
            foreach (var table in judgeTables)
            {
                if (table.JudgeTableName == tableName)
                    return table.JudgeName;
            }
            return null;
        }

        private static void SetWorksheetHorse(IXLWorksheet worksheet, Contest contest, DateTime stepDate, int startnumber, Vaulter participant, string vaultingClubName, string HorseName, string LungerName, string JudgeName)

            //DateTime eventDate, string eventLocation, string vaulterName, string country, string horseName, string lungerName, string Judge)
        {
            var country = contest?.Country;
            var eventLocation = contest?.Location;
            var vaulterName = participant.Name;
            var vaulterClass = participant.VaultingClass;
            var armNumber = participant.Armband;

            //var startNumber = startListItem.StartNumber;
            //startListItem.VaultingHorse.HorseName
            SetValueInWorksheet(worksheet, 3, "d", stepDate.ToShortDateString());
        }

        private static void SetValueInWorksheet(IXLWorksheet worksheet, int row, string column , string value)
        {
            worksheet.Cell(row, column).Value = value;
        }

        private static Step GetCompetitionStep(StartList startListItem)
        {
            var vaulterClass = startListItem.Participant.VaultingClass;
            //List<Step> steps = GetSteps(vaulterClass);
            var testNumber = startListItem.TestNumber;
            Step step = GetStep(vaulterClass, testNumber);
            return step;

        }

        private static Step GetStep(CompetitionClass vaulterClass, int testNumber)
        {
            foreach (var step in vaulterClass.Steps)
            {
                if (testNumber == step.TestNumber)
                    return step;
            }

            return null;
        }

    }
}