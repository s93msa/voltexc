using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Business.Logic.Excel;
using WebApplication1.Classes;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var contest = ContestService.GetNewDataFromDatabase();
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

        public ActionResult CompetitionClasses()
        {

            var contest = ContestService.GetInstance();
            var judges = ContestService.GetJudgesPerStep();

            var competitionClassesViewModel = new CompetitionClassesViewModel();
            using (var db = new VaultingContext())
            {
                var competitionClasses = db.CompetitionClasses;
                var startListClassSteps = db.StartListClassSteps.ToDictionary(x => x.StartListClassStepId, x => x);



                foreach (var competitionClass in competitionClasses.ToList())
                {
                    var classNumber = competitionClass.ClassNr.ToString();
                    var className = competitionClass.ClassName;
                    var numberOfJudges = "0";
                    var steps = new string[4];
                    var momentText = new string[4];
                    var judgesString1 = GetJudgesString(judges, startListClassSteps, classNumber, 1);
                    var judgesString2 = GetJudgesString(judges, startListClassSteps, classNumber, 2);
                    var judgesString3 = GetJudgesString(judges, startListClassSteps, classNumber, 3);
                    var judgesString4 = GetJudgesString(judges, startListClassSteps, classNumber, 4);

                    foreach (var step in competitionClass.Steps)
                    {
                       
                        if (step.TestNumber < 1) continue;

                        steps[step.TestNumber - 1] = step.Name;
                        momentText[step.TestNumber - 1] = step.ResultMomentText;

                    }

                    var competitionClassInfo = new CompetitionClassViewModel
                    {
                        ClassNumber = classNumber,
                        ClassName = className,
                        NumberOfJudges = numberOfJudges,
                        Moment1 = steps[0],
                        Moment2 = steps[1],
                        Moment3 = steps[2],
                        Moment4 = steps[3],
                        Moment1Header = momentText[0],
                        Moment2Header = momentText[1],
                        Moment3Header = momentText[2],
                        Moment4Header = momentText[3],
                        JudgesMoment1 = judgesString1,
                        JudgesMoment2 = judgesString2,
                        JudgesMoment3 = judgesString3,
                        JudgesMoment4 = judgesString4
                    };

                    competitionClassesViewModel.CompetitionClassesInformation.Add(competitionClassInfo);
                }

            }

            return View(competitionClassesViewModel);
        }

        private static string GetJudgesString(Dictionary<string, int> judges, Dictionary<int, StartListClassStep> startListClassSteps, string classNumber, int testNumber)
        {
            string judgesString = GetJudgeName(judges, classNumber, startListClassSteps, JudgeTableNames.A, testNumber);
            judgesString += GetJudgeName(judges, classNumber, startListClassSteps, JudgeTableNames.B, testNumber);
            judgesString += GetJudgeName(judges, classNumber, startListClassSteps, JudgeTableNames.C, testNumber);
            judgesString += GetJudgeName(judges, classNumber, startListClassSteps, JudgeTableNames.D, testNumber);
            return judgesString.TrimStart(',').TrimStart();
        }

        private static string GetJudgeName(Dictionary<string, int> stepsrelation, string classNumber, Dictionary<int, StartListClassStep> startListClassSteps,
            JudgeTableNames judgeTableName, int testNumber)
        {

            int startliststep;
            StartListClassStep startListClassStep = null;
            if(stepsrelation.TryGetValue(classNumber + "_" + testNumber.ToString(), out startliststep))
                startListClassSteps.TryGetValue(startliststep, out startListClassStep);
            var judgeName = startListClassStep?.GetJudgeName(judgeTableName);
            if (string.IsNullOrWhiteSpace(judgeName))
                return "";

            return ", " + judgeName;
        }

        public ActionResult ResultVaulterList()
        {
            var rows = new List<string[]>();   

            var contest = ContestService.GetInstance();
            foreach (var startListClassStep in contest.StartListClassStep.OrderBy(x => x.StartOrder))
            {
                foreach (var startListItem in startListClassStep.StartList.OrderBy(x => x.StartNumber))
                {
                    var horseName = startListItem.HorseInformation?.HorseName;
                    var lungerName = startListItem.HorseInformation?.Lunger?.LungerName;

                    if (startListItem.IsTeam)
                    {
                        if (startListItem.TeamTestnumber > 1) continue;
                        var vaultingClass =  startListItem.VaultingTeam.VaultingClass?.ClassNr.ToString();
                        var teamName = startListItem.VaultingTeam.Name;
                        var clubName = startListItem.VaultingTeam.VaultingClub.ClubName;
                        var teamId = ContestService.GetTeamAndClassId(startListItem.VaultingTeam);
                        var row = new string[] { vaultingClass, teamName, lungerName, clubName, horseName, teamId };
                        rows.Add(row);
                    }
                    else
                    {
                        foreach (var participant in startListItem.GetActiveVaulters().OrderBy(x => x.StartOrder))
                        {
                            if (participant.Testnumber > 1) continue;

                            var vaultingClass = participant.Participant.VaultingClass?.ClassNr.ToString();
                            var vaulterName = participant.Participant.Name;
                            var clubName = participant.Participant.VaultingClub?.ClubName;
                            string vaulterId = ContestService.GetVaulterAndClassId(participant.Participant);
                            var row = new string[] { vaultingClass, vaulterName, lungerName, clubName, horseName, vaulterId };

                            rows.Add(row);
                        }
                    }
                }
                
            }
           
            return View(rows);
        }

        


        public ActionResult StartList()
        {
                var contest = ContestService.GetInstance();
                return View(contest);
        }

        public ActionResult CopyExcel()
        {
            var contest = ContestService.GetInstance();
           
            var startListClassesSteps = contest?.StartListClassStep ?? new List<StartListClassStep>();
        
            var startListClassStepOrdered = startListClassesSteps.OrderBy(x => x.StartOrder);
            foreach (var startListClassStep in startListClassStepOrdered)
            {
                //if (startListClassStep.Date > new DateTime(2017,7,7,23,0,0) && startListClassStep.Date < new DateTime(2017, 7, 8, 13, 0, 0))
                if (startListClassStep.StartListClassStepId == 1)
                    SaveInExcel(contest, startListClassStep);
            }
                
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



            var startlistOrderByHorseOrder = startListClassStep.StartList.OrderBy(x => x.StartNumber);

            int startListNumber = 0;
                foreach (var horseOrder in startlistOrderByHorseOrder)
                {
                    startListNumber++;
                    if (horseOrder.IsTeam)
                    {
                    var teamInformation = new ExcelPreCompetitionData(contest, startListClassStep, horseOrder,
                            startListNumber, horseOrder.VaultingTeam);
                    var excelTeamService = new ExcelTeamService(teamInformation);
                    excelTeamService.CreateExcelforIndividual();
                    }

                    else if (horseOrder.Vaulters != null)
                    {
                        var vaultersSorted = horseOrder.GetActiveVaulters().OrderBy(x => x.StartOrder);
                        
                        foreach (var vaulter in vaultersSorted)
                        {
                            var vaulterInformation = new ExcelPreCompetitionData(contest, startListClassStep, horseOrder,
                                startListNumber, vaulter);
                            var excelIndividualService = new ExcelIndividualService(vaulterInformation);
                            excelIndividualService.CreateExcelforIndividual();

                            //CreateExcelforIndividual(contest, startListClassStep, startListNumber, horseOrder, startListNumber, vaulter);                            
                        }
                    }
                }
        }

        //private static void CreateExcelforIndividual(Contest contest, StartListClassStep startListClassStep,int startNumber, HorseOrder horseOrder, int startVaulterNumber, VaulterOrder vaulterOrder)
        //{
        //    var vaulter = vaulterOrder.Participant;
        //    var vaulterClass = vaulter.VaultingClass;

        //    var testNumber = vaulterOrder.Testnumber;
        //    var step = GetCompetitionStep(vaulterClass, testNumber);
        //    var vaultingClubName = vaulter.VaultingClub?.ClubName.Trim();
        //    var horse = horseOrder.HorseInformation;
        //    var lungerName = horse.Lunger.LungerName?.Trim();
        //    var judgeTableA = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.A);
        //    var judgeTableB = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.B);
        //    var judgeTableC = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.C);
        //    var judgeTableD = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.D);
        //    //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        //    //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        //    //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        //    //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
        //    var fileName = vaulterClass.Excelfile;
        //    var workbook = new XLWorkbook(fileName);
        //    //var vaulter = horseOrder.Participant;

        //    if (vaulterClass.ClassNr == 4)
        //    {
        //        CreateExcelForClass4(contest, startListClassStep, startNumber,  vaulter, vaultingClubName,
        //            horse, lungerName, judgeTableA, judgeTableB, judgeTableC, judgeTableD,
        //            step, workbook);

        //    }
        //    else if (vaulterClass.ClassNr == 5)
        //    {
        //        CreateExcelForClass5(contest, startListClassStep, startNumber,  vaulter, vaultingClubName,
        //            horse, lungerName, judgeTableA, judgeTableB, judgeTableC, judgeTableD,
        //            step, workbook);
        //        //workbook.Worksheets.Worksheet(3).Visibility = XLWorksheetVisibility.Hidden;

        //        //var worksheetHorse = workbook.Worksheet(2);
        //        //worksheetHorse.Cell(7, "c").Value = country;
        //    }


        //    // worksheetHorse.Cell(7, "d").Value = d?.Count;

        //    //}
        //}


        //Individuella juniorer
        //private static void CreateExcelForClass4(Contest contest, StartListClassStep startListClassStep, int  startNumber,  Vaulter vaulter, string vaultingClubName, Horse horse, string lungerName, JudgeTable JudgeTableA, JudgeTable JudgeTableB, JudgeTable JudgeTableC, JudgeTable JudgeTableD, Step step, XLWorkbook workbook)
        //{

        //    var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        //    var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        //    var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        //    var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
        //    string testNumber = Convert.ToString(step?.TestNumber) + step?.Name;

        //    var worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableA?.Trim());

        //    SetWorksheetHorse(worksheet, contest, startListClassStep.Date, testNumber, startNumber, vaulter,
        //            vaultingClubName, horse.HorseName, lungerName, JudgeTableA?.JudgeName);

        //    ShowOnlyWorksheet(workbook.Worksheets, worksheet);
        //    var fileOutputname = GetOutputFilename(JudgeTableA, startListClassStep, vaulter);
        //    SaveExcelFile(workbook, fileOutputname);


        //    worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableB?.Trim());

        //    SetWorksheetIndividuellJuniorGrund2(worksheet, contest, startListClassStep.Date, testNumber, startNumber, vaulter,
        //            vaultingClubName, horse.HorseName, lungerName, JudgeTableB);

        //    ShowOnlyWorksheet(workbook.Worksheets, worksheet);
        //    fileOutputname = GetOutputFilename(JudgeTableB, startListClassStep, vaulter);
        //    SaveExcelFile(workbook, fileOutputname);


        //    worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableC?.Trim());
        //    SetWorksheetIndividuellJuniorGrund2(worksheet, contest, startListClassStep.Date, testNumber, startNumber, vaulter,
        //            vaultingClubName, horse.HorseName, lungerName, JudgeTableC);

        //    ShowOnlyWorksheet(workbook.Worksheets, worksheet);
        //    fileOutputname = GetOutputFilename(JudgeTableC, startListClassStep, vaulter);
        //    SaveExcelFile(workbook, fileOutputname);



        //    worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableD?.Trim());
        //    SetWorksheetIndividuellJuniorGrund2(worksheet, contest, startListClassStep.Date, testNumber, startNumber, vaulter,
        //            vaultingClubName, horse.HorseName, lungerName, JudgeTableD);

        //    ShowOnlyWorksheet(workbook.Worksheets, worksheet);
        //    fileOutputname = GetOutputFilename(JudgeTableD, startListClassStep, vaulter);
        //    SaveExcelFile(workbook, fileOutputname);
        //}

        ////Individuella miniorer
        //private static void CreateExcelForClass5(Contest contest, StartListClassStep startListClassStep, int startNumber,  Vaulter vaulter, string vaultingClubName, Horse horse, string lungerName, JudgeTable JudgeTableA, JudgeTable JudgeTableB, JudgeTable JudgeTableC, JudgeTable JudgeTableD, Step step, XLWorkbook workbook)
        //{

        //    var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        //    var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        //    var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        //    var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;

        //    string testNumber = Convert.ToString(step?.TestNumber) + step?.Name;

        //    var worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableA?.Trim());

        //    SetWorksheetHorse(worksheet, contest, startListClassStep.Date, testNumber, startNumber, vaulter,
        //            vaultingClubName, horse.HorseName, lungerName, JudgeTableA?.JudgeName);

        //    ShowOnlyWorksheet(workbook.Worksheets, worksheet);
        //    var fileOutputname = GetOutputFilename(JudgeTableA, startListClassStep, vaulter);
        //    SaveExcelFile(workbook, fileOutputname);


        //    worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableB?.Trim());

        //    SetWorksheetIndividuellMiniorGrund1(worksheet, contest, startListClassStep.Date, testNumber, startNumber, vaulter,
        //            vaultingClubName, horse.HorseName, lungerName, JudgeTableB);

        //    ShowOnlyWorksheet(workbook.Worksheets, worksheet);
        //    fileOutputname = GetOutputFilename(JudgeTableB, startListClassStep, vaulter);
        //    SaveExcelFile(workbook, fileOutputname);


        //    worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableC?.Trim());
        //    SetWorksheetIndividuellMiniorGrund1(worksheet, contest, startListClassStep.Date, testNumber, startNumber, vaulter,
        //            vaultingClubName, horse.HorseName, lungerName, JudgeTableC);

        //    ShowOnlyWorksheet(workbook.Worksheets, worksheet);
        //    fileOutputname = GetOutputFilename(JudgeTableC, startListClassStep, vaulter);
        //    SaveExcelFile(workbook, fileOutputname);



        //    worksheet = workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTableD?.Trim());
        //    SetWorksheetIndividuellMiniorGrund1(worksheet, contest, startListClassStep.Date, testNumber, startNumber, vaulter,
        //            vaultingClubName, horse.HorseName, lungerName, JudgeTableD);

        //    ShowOnlyWorksheet(workbook.Worksheets, worksheet);
        //    fileOutputname = GetOutputFilename(JudgeTableD, startListClassStep, vaulter);
        //    SaveExcelFile(workbook, fileOutputname);
        //}

        //private static void SaveExcelFile(XLWorkbook workbook, string fileName)
        //{
        //    string fileoutputname = @"C:\Temp\Test_Voligemallar\output\" + fileName + ".xlsx";
        //    workbook.SaveAs(fileoutputname);
        //}

        //private static string GetOutputFilename(JudgeTable judgeTabel, StartListClassStep startListClassStep, Vaulter vaulter)
        //{
        //    return startListClassStep.Date.ToShortDateString() +  @"\" + judgeTabel.JudgeTableName + @"\" + startListClassStep.Name + @"\" + vaulter.Name + ".xlsx";
        //}

        //private static void ShowOnlyWorksheet(IXLWorksheets workbookWorksheets, IXLWorksheet worksheet)
        //{
        //    foreach (var currrentWorksheet in workbookWorksheets)
        //    {
        //        if (currrentWorksheet == worksheet)
        //        {
        //            currrentWorksheet.Visibility = XLWorksheetVisibility.Visible;
        //            currrentWorksheet.TabActive = true;
        //        }
        //        else
        //            currrentWorksheet.Hide(); //.Visibility = XLWorksheetVisibility.Hidden;
        //    }
        //}

        //private static JudgeTable GetJudge(List<JudgeTable> judgeTables, JudgeTableNames tableName)
        //{
        //    foreach (var table in judgeTables)
        //    {
        //        if (table.JudgeTableName == tableName)
        //            return table;
        //    }
        //    return null;
        //}

        //private static void SetWorksheetHorse(IXLWorksheet worksheet, Contest contest, DateTime stepDate, string moment , int startnumber, Vaulter participant, string vaultingClubName, string horseName, string lungerName, string judgeName)
        //{
        //    const string tableName = "A";
        //    var country = contest?.Country;
        //    var eventLocation = contest?.Location;
        //    var vaulterName = participant.Name;
        //    var vaulterClass = participant.VaultingClass;
        //    var armNumber = participant.Armband;

        //    SetValueInWorksheet(worksheet, 3, "c", stepDate.ToShortDateString());
        //    SetValueInWorksheet(worksheet, 4, "c", eventLocation);
        //    SetValueInWorksheet(worksheet, 5, "c", vaulterName);
        //    SetValueInWorksheet(worksheet, 6, "c", vaultingClubName);
        //    SetValueInWorksheet(worksheet, 7, "c", country);
        //    SetValueInWorksheet(worksheet, 8, "c", horseName);
        //    SetValueInWorksheet(worksheet, 9, "c", lungerName);

        //    SetValueInWorksheet(worksheet, 1, "l", startnumber.ToString());
        //    SetValueInWorksheet(worksheet, 2, "l", tableName);
        //    SetValueInWorksheet(worksheet, 3, "l", vaulterClass.ClassNr + " (" + vaulterClass.ClassName + ")");
        //    SetValueInWorksheet(worksheet, 4, "l", moment);
        //    SetValueInWorksheet(worksheet, 6, "l", armNumber);

        //    SetValueInWorksheet(worksheet, 29, "c", judgeName);


        //}
        
        //private static void SetWorksheetIndividuellMiniorGrund1(IXLWorksheet worksheet, Contest contest, DateTime stepDate, string moment, int startnumber, Vaulter participant, string vaultingClubName, string horseName, string lungerName, JudgeTable judgeTable)
        //{

        //    var country = contest?.Country;
        //    var eventLocation = contest?.Location;
        //    var vaulterName = participant.Name;
        //    var vaulterClass = participant.VaultingClass;
        //    var armNumber = participant.Armband;

        //    SetValueInWorksheet(worksheet, 4, "c", stepDate.ToShortDateString());
        //    SetValueInWorksheet(worksheet, 5, "c", eventLocation);
        //    SetValueInWorksheet(worksheet, 6, "c", vaulterName);
        //    SetValueInWorksheet(worksheet, 7, "c", vaultingClubName);
        //    SetValueInWorksheet(worksheet, 8, "c", country);
        //    SetValueInWorksheet(worksheet, 9, "c", horseName);
        //    SetValueInWorksheet(worksheet, 10, "c", lungerName);

        //    SetValueInWorksheet(worksheet, 2, "l", startnumber.ToString());
        //    SetValueInWorksheet(worksheet, 3, "l", judgeTable?.JudgeTableName.ToString());
        //    SetValueInWorksheet(worksheet, 4, "l", vaulterClass.ClassNr + " (" + vaulterClass.ClassName + ")");
        //    SetValueInWorksheet(worksheet, 5, "l", moment);
        //    SetValueInWorksheet(worksheet, 7, "l", armNumber);

        //    SetValueInWorksheet(worksheet, 32, "c", judgeTable?.JudgeName);


        //}

        //private static void SetWorksheetIndividuellJuniorGrund2(IXLWorksheet worksheet, Contest contest,  DateTime stepDate, string moment, int startnumber, Vaulter participant, string vaultingClubName, string horseName, string lungerName, JudgeTable judgeTable)
        //{
            
        //    var country = contest?.Country;
        //    var eventLocation = contest?.Location;
        //    var vaulterName = participant.Name;
        //    var vaulterClass = participant.VaultingClass;
        //    var armNumber = participant.Armband;

        //    SetValueInWorksheet(worksheet, 4, "c", stepDate.ToShortDateString());
        //    SetValueInWorksheet(worksheet, 5, "c", eventLocation);
        //    SetValueInWorksheet(worksheet, 6, "c", vaulterName);
        //    SetValueInWorksheet(worksheet, 7, "c", vaultingClubName);
        //    SetValueInWorksheet(worksheet, 8, "c", country);
        //    SetValueInWorksheet(worksheet, 9, "c", horseName);
        //    SetValueInWorksheet(worksheet, 10, "c", lungerName);

        //    SetValueInWorksheet(worksheet, 2, "l", startnumber.ToString());
        //    SetValueInWorksheet(worksheet, 3, "l", judgeTable?.JudgeTableName.ToString());
        //    SetValueInWorksheet(worksheet, 4, "l", vaulterClass.ClassNr + " (" + vaulterClass.ClassName + ")");
        //    SetValueInWorksheet(worksheet, 5, "l", moment);
        //    SetValueInWorksheet(worksheet, 7, "l", armNumber);

        //    SetValueInWorksheet(worksheet, 32, "c", judgeTable?.JudgeName);


        //}

        //private static void SetWorksheetIndkürtekn2_3(IXLWorksheet worksheet, Contest contest, DateTime stepDate, string moment, int startnumber, Vaulter participant, string vaultingClubName, string horseName, string lungerName, JudgeTable judgeTable)
        //{

        //    var country = contest?.Country;
        //    var eventLocation = contest?.Location;
        //    var vaulterName = participant.Name;
        //    var vaulterClass = participant.VaultingClass;
        //    var armNumber = participant.Armband;

        //    SetValueInWorksheet(worksheet, 4, "c", stepDate.ToShortDateString());
        //    SetValueInWorksheet(worksheet, 5, "c", eventLocation);
        //    SetValueInWorksheet(worksheet, 6, "c", vaulterName);
        //    SetValueInWorksheet(worksheet, 7, "c", vaultingClubName);
        //    SetValueInWorksheet(worksheet, 8, "c", country);
        //    SetValueInWorksheet(worksheet, 9, "c", horseName);
        //    SetValueInWorksheet(worksheet, 10, "c", lungerName);

        //    SetValueInWorksheet(worksheet, 2, "l", startnumber.ToString());
        //    SetValueInWorksheet(worksheet, 3, "l", judgeTable?.JudgeTableName.ToString());
        //    SetValueInWorksheet(worksheet, 4, "l", vaulterClass.ClassNr + " (" + vaulterClass.ClassName + ")");
        //    SetValueInWorksheet(worksheet, 5, "l", moment);
        //    SetValueInWorksheet(worksheet, 7, "l", armNumber);

        //    SetValueInWorksheet(worksheet, 37, "c", judgeTable?.JudgeName);


        //}


        //private static void SetValueInWorksheet(IXLWorksheet worksheet, int row, string column , string value)
        //{
        //    worksheet.Cell(row, column).Value = value;
        //}

        //private static void SetValueInWorksheet(IXLWorksheet worksheet, string cellName, string value)
        //{
        //    var linkTocell = worksheet.Workbook.NamedRange(cellName);
        //    var g = GetNamedCell(worksheet, cellName);
        //    worksheet.Cell(linkTocell.RefersTo.Split('!')[1]).Value = value;
        //    //          worksheet.Workbook.Range(cellName).Cells();
        //        //= value;
        //}

        //public static IXLCell GetNamedCell(IXLWorksheet worksheet, string namedCell)
        //{
        //    IXLNamedRange xlNamedRange = worksheet.NamedRange(namedCell);
        //    if (xlNamedRange == null)
        //        return (IXLCell)null;
        //    IXLRange xlRange = xlNamedRange.Ranges.FirstOrDefault<IXLRange>();
        //    if (xlRange == null)
        //        return (IXLCell)null;
        //    return xlRange.FirstCell();
        //}


       

        //private static Step GetCompetitionStep(CompetitionClass vaulterClass, int testNumber)
        //{
        //    foreach (var step in vaulterClass.Steps)
        //    {
        //        if (testNumber == step.TestNumber)
        //            return step;
        //    }

        //    return null;
        //}

    }
}