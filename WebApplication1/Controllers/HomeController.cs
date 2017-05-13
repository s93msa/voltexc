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

                        var horse = startListItem.VaultingHorse;
                        var lungerName = horse.Lunger.LungerName;
                        var JudgeTableA = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.A);
                        var JudgeTableB = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.B);
                        var JudgeTableC = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.C);
                        var JudgeTableD = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.D);

                        var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
                        var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
                        var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
                        var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
                        //startListClassStep
                        //vaulterClass.Steps


                        //fileName = @"C:\Temp\Test_Voligemallar\Protokoll 2017 individuell klass.xlsx";
                        fileName = vaulterClass.Excelfile;
                        var workbook = new XLWorkbook(fileName);


                        var vaulter = startListItem.Participant;

                        var worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableA);

                        SetWorksheetHorse(worksheet, contest, startListClassStep.Date, startListItem.TestNumber, startListItem.StartNumber, vaulter,
                                vaultingClubName, horse.HorseName, lungerName, JudgeTableA?.JudgeName);

                        ShowOnlyWorksheet(workbook.Worksheets, worksheet);
                        var fileOutputname = GetOutputFilename(JudgeTableA,startListClassStep, vaulter);
                        SaveExcelFile(workbook, fileOutputname);

                        
                        worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableB);

                        SetWorksheetIndividuellJuniorGrund2(worksheet, contest, startListClassStep.Date, startListItem.TestNumber, startListItem.StartNumber, vaulter,
                                vaultingClubName, horse.HorseName, lungerName, JudgeTableB);

                        ShowOnlyWorksheet(workbook.Worksheets, worksheet);
                        fileOutputname = GetOutputFilename(JudgeTableB, startListClassStep, vaulter);
                        SaveExcelFile(workbook, fileOutputname);


                        worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableC);
                        SetWorksheetIndividuellJuniorGrund2(worksheet, contest, startListClassStep.Date, startListItem.TestNumber, startListItem.StartNumber, vaulter,
                                vaultingClubName, horse.HorseName, lungerName, JudgeTableC);

                        ShowOnlyWorksheet(workbook.Worksheets, worksheet);
                        fileOutputname = GetOutputFilename(JudgeTableC, startListClassStep, vaulter);
                        SaveExcelFile(workbook, fileOutputname);



                        worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableD);
                        SetWorksheetIndividuellJuniorGrund2(worksheet, contest,  startListClassStep.Date, startListItem.TestNumber, startListItem.StartNumber, vaulter,
                                vaultingClubName, horse.HorseName, lungerName, JudgeTableD);

                        ShowOnlyWorksheet(workbook.Worksheets, worksheet);
                        fileOutputname = GetOutputFilename(JudgeTableD, startListClassStep, vaulter);
                        SaveExcelFile(workbook, fileOutputname);




                        //workbook.Worksheets.Worksheet(3).Visibility = XLWorksheetVisibility.Hidden;

                        //var worksheetHorse = workbook.Worksheet(2);
                        //worksheetHorse.Cell(7, "c").Value = country;
                    }


                    // worksheetHorse.Cell(7, "d").Value = d?.Count;

                    //}
                }
            }
        }
        
        private static void SaveExcelFile(XLWorkbook workbook, string fileName)
        {
            string fileoutputname = @"C:\Temp\Test_Voligemallar\output\" + fileName + ".xlsx";
            workbook.SaveAs(fileoutputname);
        }

        private static string GetOutputFilename(JudgeTable judgeTabel, StartListClassStep startListClassStep, Vaulter vaulter)
        {
            return judgeTabel.JudgeTableName + @"\" + startListClassStep.Name + @"\" + vaulter.Name + ".xlsx";
        }

        private static void ShowOnlyWorksheet(IXLWorksheets workbookWorksheets, IXLWorksheet worksheet)
        {
            foreach (var currrentWorksheet in workbookWorksheets)
            {
                if (currrentWorksheet == worksheet)
                    currrentWorksheet.Visibility = XLWorksheetVisibility.Visible;
                else
                    currrentWorksheet.Visibility = XLWorksheetVisibility.Hidden;
            }
        }

        private static JudgeTable GetJudge(List<JudgeTable> judgeTables, JudgeTableNames tableName)
        {
            foreach (var table in judgeTables)
            {
                if (table.JudgeTableName == tableName)
                    return table;
            }
            return null;
        }

        private static void SetWorksheetHorse(IXLWorksheet worksheet, Contest contest, DateTime stepDate, int testNumber , int startnumber, Vaulter participant, string vaultingClubName, string horseName, string lungerName, string judgeName)
        {
            const string tableName = "A";
            var country = contest?.Country;
            var eventLocation = contest?.Location;
            var vaulterName = participant.Name;
            var vaulterClass = participant.VaultingClass;
            var armNumber = participant.Armband;

            //var startNumber = startListItem.StartNumber;
            //startListItem.VaultingHorse.HorseName
            SetValueInWorksheet(worksheet, 3, "c", stepDate.ToShortDateString());
            SetValueInWorksheet(worksheet, 4, "c", eventLocation);
            SetValueInWorksheet(worksheet, 5, "c", vaulterName);
            SetValueInWorksheet(worksheet, 6, "c", vaultingClubName);
            SetValueInWorksheet(worksheet, 7, "c", country);
            SetValueInWorksheet(worksheet, 8, "c", horseName);
            SetValueInWorksheet(worksheet, 9, "c", lungerName);

            SetValueInWorksheet(worksheet, 1, "l", startnumber.ToString());
            SetValueInWorksheet(worksheet, 2, "l", tableName);
            SetValueInWorksheet(worksheet, 3, "l", vaulterClass.ClassNr + " (" + vaulterClass.ClassName + ")");
            SetValueInWorksheet(worksheet, 4, "l", testNumber.ToString());
            SetValueInWorksheet(worksheet, 6, "l", armNumber);

            SetValueInWorksheet(worksheet, 29, "c", judgeName);


        }

        private static void SetWorksheetIndividuellJuniorGrund2(IXLWorksheet worksheet, Contest contest,  DateTime stepDate, int testNumber, int startnumber, Vaulter participant, string vaultingClubName, string horseName, string lungerName, JudgeTable judgeTable)
        {
            
            var country = contest?.Country;
            var eventLocation = contest?.Location;
            var vaulterName = participant.Name;
            var vaulterClass = participant.VaultingClass;
            var armNumber = participant.Armband;

            //var startNumber = startListItem.StartNumber;
            //startListItem.VaultingHorse.HorseName
            SetValueInWorksheet(worksheet, 4, "c", stepDate.ToShortDateString());
            SetValueInWorksheet(worksheet, 5, "c", eventLocation);
            SetValueInWorksheet(worksheet, 6, "c", vaulterName);
            SetValueInWorksheet(worksheet, 7, "c", vaultingClubName);
            SetValueInWorksheet(worksheet, 8, "c", country);
            SetValueInWorksheet(worksheet, 9, "c", horseName);
            SetValueInWorksheet(worksheet, 10, "c", lungerName);

            SetValueInWorksheet(worksheet, 2, "l", startnumber.ToString());
            SetValueInWorksheet(worksheet, 3, "l", judgeTable?.JudgeTableName.ToString());
            SetValueInWorksheet(worksheet, 4, "l", vaulterClass.ClassNr + " (" + vaulterClass.ClassName + ")");
            SetValueInWorksheet(worksheet, 5, "l", testNumber.ToString());
            SetValueInWorksheet(worksheet, 7, "l", armNumber);

            SetValueInWorksheet(worksheet, 32, "c", judgeTable?.JudgeName);


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