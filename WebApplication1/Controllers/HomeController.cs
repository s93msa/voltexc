using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Business.Logic.Excel;
using VoltigeCore.Business.Logic.Excel.Results;
using VoltigeCore.Business.Logic.Pdf;
using VoltigeCore.Business.Logic.Result;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExcelResultService _excelResultService;
        private readonly ResultService _resultService;
        private readonly StartlistExportService _startlistExportService;
        private readonly ExportStartListService _exportStartListService;

        public HomeController()
        {
            _exportStartListService = new ExportStartListService();
            _excelResultService = new ExcelResultService();
            _resultService = new ResultService();
            _startlistExportService = new StartlistExportService();
        }

        public IActionResult Index()
        {
            ContestService.GetNewDataFromDatabase();
            return View();
        }

        public IActionResult CreatePreResultInformationExcel()
        {
            var participants = _resultService.GetParticipants();
            var classes = _resultService.GetClasses();

            _excelResultService.SetVaulterList(participants.ToArray());
            _excelResultService.SetCompetitionClasses(classes.ToArray());
            _excelResultService.SaveWithTimeStamp();

            ViewBag.Message = "Excel skapad";
            return View();
        }

        public IActionResult CreateExportStartListExcel()
        {
            _exportStartListService.SetStartList();
            _exportStartListService.SaveWithTimeStamp();

            ViewBag.Message = "Excel skapad";
            return View();
        }

        public IActionResult CreateStartListExcel()
        {
            _startlistExportService.SetStartList();
            _startlistExportService.SaveWithTimeStamp();

            ViewBag.Message = "Excel skapad";
            return View();
        }

        public IActionResult CreatePdfPrintFile()
        {
            var startListNames = pdfbatfileService.GetStartListNames();
            foreach (var startDate in startListNames.Keys)
            {
                pdfbatfileService.WriteBatfile(startDate.ToShortDateString(), startListNames[startDate]);
            }

            ViewBag.Message = "pdfbat skapad";
            return View();
        }

        public IActionResult StartList()
        {
            var contest = ContestService.GetContestInstance();
            return View(contest);
        }

        public IActionResult CopyExcelWithStartnumber()
        {
            return CopyExcel(startNumberInFileName: true);
        }

        public IActionResult CopyExcel(bool startNumberInFileName = false)
        {
            var contest = ContestService.GetContestInstance();
            var startListClassesSteps = contest?.StartListClassStep ?? new List<StartListClassStep>();

            foreach (var startListClassStep in startListClassesSteps.OrderBy(x => x.StartOrder))
            {
                SaveInExcel(contest, startListClassStep, startNumberInFileName);
            }

            return View("CopyExcel");
        }

        private static void SaveInExcel(Contest contest, StartListClassStep startListClassStep, bool startNumberInFileName = false)
        {
            var startlistOrderByHorseOrder = startListClassStep.GetActiveStartList().OrderBy(x => x.StartNumber).ToArray();
            int startListNumber = 0;
            foreach (var horseOrder in startlistOrderByHorseOrder)
            {
                if (horseOrder.IsTeam)
                {
                    startListNumber++;
                    var teamInformation = new ExcelPreCompetitionData(contest, startListClassStep, horseOrder,
                            startListNumber, horseOrder.VaultingTeam);
                    var excelTeamService = new ExcelTeamService(teamInformation) { StartOrderInfileName = startNumberInFileName };
                    excelTeamService.CreateExcelforIndividual();
                }
                else if (horseOrder.Vaulters != null)
                {
                    var vaultersSorted = horseOrder.GetActiveVaulters().OrderBy(x => x.StartOrder);
                    foreach (var vaulter in vaultersSorted)
                    {
                        startListNumber++;
                        var vaulterInformation = new ExcelPreCompetitionData(contest, startListClassStep, horseOrder,
                            startListNumber, vaulter);
                        var excelIndividualService = new ExcelIndividualService(vaulterInformation);
                        excelIndividualService.StartOrderInfileName = startNumberInFileName;
                        excelIndividualService.CreateExcelforIndividual();
                    }
                }
            }
        }
    }
}
