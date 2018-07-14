using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using WebApplication1.Business.Logic.Import;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ImportController : Controller
    {
        private RequestService _requestService;
        private UpdateService _updateService;

        public ImportController()
        {
            _requestService = new RequestService(Request);
            _updateService = new UpdateService();

        }

        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            _requestService = new RequestService(Request);
            var workbook = _requestService.GetWorkbook();
            var excelImportService = new ExcelImportService(workbook);
            //var _updateService = new UpdateService();
            var lungers = excelImportService.GetLungers();
            _updateService.UpdateLungers(lungers);

            var horses = excelImportService.GetHorses();
            _updateService.UpdateHorses(horses);

            var clubs = excelImportService.GetClubs();
            _updateService.UpdateClubs(clubs);

            var classes = excelImportService.GetClasses();
            _updateService.UpdateClasses(classes);


            var vaulters = excelImportService.GetVaulters();
            _updateService.UpdateVaulters(vaulters);

            var teams = excelImportService.GetTeams();
            _updateService.UpdateTeams(teams);

            var teamMembers = excelImportService.GetTeamMembers();
            _updateService.UpdateTeamMembers(teamMembers);

            
            ImportTeams(excelImportService);

            ImportIndividuals(excelImportService);


            return RedirectToAction("Index");
        }

        

        private void ImportTeams(ExcelImportService excelImportService)
        {
            int StartListClassStepId;
            int[] competionClassesTdbIds;
            int testNumber;


            //senior lag
            competionClassesTdbIds = new int[1] { 350446 };
            StartListClassStepId = 18; // Svår klass lag seniorer klass 1 -Grund 
            testNumber = 1;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 7; //Svår klass lag seniorer klass 1 – Kür
            testNumber = 2;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, StartListClassStepId, testNumber);
            //_updateService.UpdateTeamHorseOrders(horseOrders);

            //Junior lag
            competionClassesTdbIds = new [] { 350447 };
            StartListClassStepId = 16; // Svår klass juniorlag klass 2 -Grund  
            testNumber = 1;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 10; //Svår klass lag seniorer klass 1 – Kür
            testNumber = 2;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            //Mellanklass lag
            competionClassesTdbIds = new[] { 350456 };
            StartListClassStepId = 55; // Mellanklass lag klass 9 - Grund 
            testNumber = 1;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 56; //Mellanklass klass 9 - Kür
            testNumber = 2;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            //Lätt klass Lag galopp klass 12
            competionClassesTdbIds = new[] { 350458};
            StartListClassStepId = 43; // Lätt klass Lag galopp klass 12 grund
            testNumber = 1;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, StartListClassStepId, testNumber);
            //_updateService.UpdateTeamHorseOrders(horseOrders);

            StartListClassStepId = 43; // Lätt klass Lag galopp klass 12 kür
            testNumber = 2;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, StartListClassStepId, testNumber);
            //_updateService.UpdateTeamHorseOrders(horseOrders);

            //Lätt klass Lag galopp klass 13
            competionClassesTdbIds = new[] { 350459};
            StartListClassStepId = 40; // Lätt klass Lag galopp klass 13 grund
            testNumber = 1;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 40; // Lätt klass Lag galopp klass 13 kür
            testNumber = 2;
            ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);


        }

        private void ImportIndividuals(ExcelImportService excelImportService)
        {
            int StartListClassStepId;
            int[] competionClassesTdbIds;
            int testNumber;


            // individuella
            //Svår klass
            competionClassesTdbIds = new [] { 350449, 350450, 350451 }; //Svår klass individuella Juniorer-Miniorer-senior(utan tekn)

            StartListClassStepId = 1; // Svår klass individuella Juniorer-Miniorer-senior  klass 3, 4, 5,6 – Grund
            testNumber = 1;
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 2; // Svår klass individuella Juniorer-Miniorer-seniorklass 3, 4, 5,6 – Kür
            testNumber = 2;
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            competionClassesTdbIds = new [] { 350448}; // senior individuell (med tekn)

            StartListClassStepId = 1; // Svår klass individuella Juniorer-Miniorer-senior  klass 3, 4, 5,6 – Grund
            testNumber = 1;
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 1; // Svår klass individuella Juniorer-Miniorer-seniorklass 3, 4, 5,6 – Kür
            testNumber = 2; //tekn kür
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 2; // Svår klass individuella Juniorer-Miniorer-seniorklass 3, 4, 5,6 – Kür
            testNumber = 3;
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);



            competionClassesTdbIds = new [] { 350452};
            StartListClassStepId = 46; // Lätt klass individuell galopp klass 7
            testNumber = 1;
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 46; // Lätt klass individuell galopp klass 7
            testNumber = 2;
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            competionClassesTdbIds = new [] { 350455 };
            StartListClassStepId = 39; // Lätt klass individuell galopp klass 8
            testNumber = 1;
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            StartListClassStepId = 39; // Lätt klass individuell galopp klass 8
            testNumber = 2;
            ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

        }

        private void ImportIndividual(ExcelImportService excelImportService, int[] competionClassesTdbIds,
            int StartListClassStepId, int testNumber)
        {
            HorseOrder[] horseOrders;
            horseOrders =
                excelImportService.GetHorseOrderIndividual(competionClassesTdbIds, StartListClassStepId, testNumber);

            _updateService.UpdateIndividualHorseOrders(horseOrders);
        }

        private void ImportTeam(ExcelImportService excelImportService, int[] competionClassesTdbIds, int startListClassStepId,
            int testNumber)
        {
            var horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, startListClassStepId, testNumber);
            _updateService.UpdateTeamHorseOrders(horseOrders);
        }
    }
}
