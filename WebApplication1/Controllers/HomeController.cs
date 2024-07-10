using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Business.Logic.Excel;
using WebApplication1.Classes;
using WebApplication1.Models;
using WebApplication1.Business.Logic.Excel.Results;
using WebApplication1.Business.Logic.Result;
using System;
using WebApplication1.Business.Logic.Pdf;

namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        public ExcelResultService _excelResultService;
        public ResultService _resultService;
        private StartlistExportService _startlistExportService;

        public HomeController()
        {
            _excelResultService = new ExcelResultService();
            _resultService = new ResultService();
            _startlistExportService = new StartlistExportService();
        }

        public ActionResult Index()
        {
            var contest = ContestService.GetNewDataFromDatabase();
            return View();
        }

        public ActionResult CreatePreResultInformationExcel()
        {
            var participants = _resultService.GetParticipants();
            var classes = _resultService.GetClasses();

            _excelResultService.SetVaulterList(participants.ToArray());
            _excelResultService.SetCompetitionClasses(classes.ToArray());

            _excelResultService.SaveWithTimeStamp();

            ViewBag.Message = "Excel skapad";

            return View();
        }

        public ActionResult CreateStartListExcel()
        {

            _startlistExportService.SetStartList();

            _startlistExportService.SaveWithTimeStamp();

            ViewBag.Message = "Excel skapad";

            return View();
        }
        public ActionResult CreatePdfPrintFile()
        {
            var startListNames = pdfbatfileService.GetStartListNames();
            foreach (var startDate in startListNames.Keys)
            {
                pdfbatfileService.WriteBatfile(startDate.ToShortDateString(), startListNames[startDate]);
            }

            ViewBag.Message = "pdfbat skapad";

            return View();
        }



        public ActionResult StartList()
        {
                var contest = ContestService.GetContestInstance();
                return View(contest);
        }

        public ActionResult CopyExcelWithStartnumber()
        {
            return CopyExcel(startNumberInFileName: true);
        }
        public ActionResult CopyExcel(bool startNumberInFileName = false)
        {
            var contest = ContestService.GetContestInstance();
           
            var startListClassesSteps = contest?.StartListClassStep ?? new List<StartListClassStep>();
        
            var startListClassStepOrdered = startListClassesSteps.OrderBy(x => x.StartOrder);
            foreach (var startListClassStep in startListClassStepOrdered)
            {
                //if (startListClassStep.Date > new DateTime(2018, 9, 7, 22, 0, 0) &&
                //    startListClassStep.Date < new DateTime(2018, 9, 8, 23, 0, 0))
                //if (startListClassStep.StartListClassStepId == 2095 || startListClassStep.StartListClassStepId == 3092 || startListClassStep.StartListClassStepId == 3093 || startListClassStep.StartListClassStepId == 2093) // Lätt klass Indiviuell sm 2022
                //if (startListClassStep.StartListClassStepId == 2093) // Lätt klass lag sm 2022
                //if (startListClassStep.StartListClassStepId == 5) // Svår klass 2, Juniorlag SM klass-  Grund sm 2022
                //if (startListClassStep.StartListClassStepId == 1059) // Svår klass 1, Seniorlag SM klass- Grund sm 2022
                //if (startListClassStep.StartListClassStepId == 1066) // Svår klass 3, 4, 5, 6 Individuella – Grund - Grund sm 2022
                //if (startListClassStep.StartListClassStepId == 5 || startListClassStep.StartListClassStepId == 1059 || startListClassStep.StartListClassStepId == 1066) // Svår klass 2, Juniorlag SM klass-  Grund sm 2022
                //if (startListClassStep.StartListClassStepId == 2093) // Lätt klass lag sm 2022
                //if (startListClassStep.StartListClassStepId == 9) // Svår klass 3, 4, 5, 6 Individuella – Kür  sm 2022
                //if (startListClassStep.StartListClassStepId == 20) // Pas de deux omg 1
                //if (startListClassStep.StartListClassStepId == 1062) // Svår klass 2, Juniorlag SM klass SM 2022
                //if (startListClassStep.StartListClassStepId == 1061) // Svår klass 1 Lag seniorer – Kür SM 2022
                //if (startListClassStep.StartListClassStepId == 9 || 
                //startListClassStep.StartListClassStepId == 20 ||
                //    startListClassStep.StartListClassStepId == 1062 ||
                //    startListClassStep.StartListClassStepId == 1061
                //    ) //  sm 2022 på fredag förmiddag innan final
                //                if (startListClassStep.Date.Day== new DateTime(2019,07, 14).Day)
                //if (startListClassStep.StartListClassStepId == 1 )

                //if (startListClassStep.StartListClassStepId == 1063) // Svår klass 2, Juniorlag SM klass - Kür  final SM 2022
                //if (startListClassStep.StartListClassStepId == 6) // Svår klass 1, Seniorlag SM klass – Kür final SM 2022
                //if (startListClassStep.StartListClassStepId == 1065) // Svår klass 3, 4, 5, 6  SM klass Individuella -  kür final SM 2022
                //if (startListClassStep.StartListClassStepId == 1064) // Svår klass 3, 4, 5, 6  SM klass Individuella -  Teknisk kür eller grund SM 2022
                //if (startListClassStep.StartListClassStepId == 22 || startListClassStep.StartListClassStepId == 1063 || startListClassStep.StartListClassStepId == 6) 
                // 22 pas de deux final - 
                // 1063 Svår klass 2, Juniorlag SM klass - Kür  final SM 2022
                // 6 Svår klass 1, Seniorlag SM klass – Kür final
                //if(startListClassStep.StartListClassStepId == 2088) // || startListClassStep.StartListClassStepId == 40)
                //{
                //if (startListClassStep.Date.Day == new DateTime(2023, 07, 08).Day)
                //{
                    SaveInExcel(contest, startListClassStep, startNumberInFileName);
                //}
            }
                
            return View("CopyExcel");
        }

        private static void SaveInExcel(Contest contest, StartListClassStep startListClassStep, bool startNumberInFileName = false)
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



            var startlistOrderByHorseOrder = startListClassStep.GetActiveStartList().OrderBy(x => x.StartNumber).ToArray();

            int startListNumber = 0;
                foreach (var horseOrder in startlistOrderByHorseOrder)
                {
                    //if(horseOrder.HorseId != 22659)
                    //{
                    //    continue;
                    //}
                    if (horseOrder.IsTeam)
                    {
                        startListNumber++;
                        var teamInformation = new ExcelPreCompetitionData(contest, startListClassStep, horseOrder,
                                startListNumber, horseOrder.VaultingTeam);
                        var excelTeamService =
                            new ExcelTeamService(teamInformation) {StartOrderInfileName = startNumberInFileName};
                        excelTeamService.CreateExcelforIndividual();
                    }

                    else if (horseOrder.Vaulters != null)
                    {
                        var vaultersSorted = horseOrder.GetActiveVaulters().OrderBy(x => x.StartOrder);
                        
                        foreach (var vaulter in vaultersSorted)
                        {
                            //if (vaulter.Participant.VaultingClass.ClassTdbId != 629759)
                            //{
                            //    continue;
                            //}
                        startListNumber++;
                            var vaulterInformation = new ExcelPreCompetitionData(contest, startListClassStep, horseOrder,
                                startListNumber, vaulter);
                            var excelIndividualService = new ExcelIndividualService(vaulterInformation);
                            excelIndividualService.StartOrderInfileName = startNumberInFileName;
                            excelIndividualService.CreateExcelforIndividual();

                            //CreateExcelforIndividual(contest, startListClassStep, startListNumber, horseOrder, startListNumber, vaulter);  
                        }
                    }
                }
        }


    }
}