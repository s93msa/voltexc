using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using WebApplication1.Business.Logic.Import;
using WebApplication1.Models;
using WebApplication1.ViewModels;

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
        public ActionResult Upload(ImportViewModel model, HttpPostedFileBase file, string addNewCheckbox, string updateCheckbox, string excludeStartlist)
        {



            _requestService = new RequestService(Request);
            _updateService.AddNew = _requestService.IsCheckboxChecked(addNewCheckbox);
            _updateService.UpdateExisting = _requestService.IsCheckboxChecked(updateCheckbox);
            _updateService.ExcludeStartlist = _requestService.IsCheckboxChecked(excludeStartlist);

            var workbook = _requestService.GetWorkbook(file);
            var excelImportService = new ExcelImportService(workbook);
            //var _updateService = new UpdateService();
            Changed changedItems;
            var lungers = excelImportService.GetLungers();
            changedItems = _updateService.UpdateLungers(lungers);

            model.NewLungers = changedItems.New;
            model.UppdatedLungers = changedItems.Updated;


            var horses = excelImportService.GetHorses();
            changedItems = _updateService.UpdateHorses(horses);
            model.NewHorses = changedItems.New;
            model.UpdatedHorses = changedItems.Updated;

            var clubs = excelImportService.GetClubs();
            changedItems = _updateService.UpdateClubs(clubs);
            model.NewClubs = changedItems.New;
            model.UpdatedClubs = changedItems.Updated;

            var classes = excelImportService.GetClasses();
            changedItems = _updateService.UpdateClasses(classes);
            model.NewClasses = changedItems.New;
            model.UpdatedClasses = changedItems.Updated;

            var vaulters = excelImportService.GetVaulters();
            changedItems = _updateService.UpdateVaulters(vaulters);
            model.NewVaulters = changedItems.New;
            model.UpdatedVaulters = changedItems.Updated;

            var teams = excelImportService.GetTeams();
            changedItems = _updateService.UpdateTeams(teams);
            model.NewTeams = changedItems.New;
            model.UpdatedTeams = changedItems.Updated;

            var teamMembers = excelImportService.GetTeamMembers();
            changedItems = _updateService.UpdateTeamMembers(teamMembers); //Observera att den inte tar bort teammembers. Bara lägger till eller ändrar ordning
            model.NewTeamMembers = changedItems.New;
            model.UpdatedTeamMembers = changedItems.Updated;

            if (!_updateService.ExcludeStartlist)
            {
                var changedStartlist = ImportTeams(excelImportService);
                model.ChangedStartListTeamList = changedStartlist;

                var changedStartlistIndividual = ImportIndividuals(excelImportService);
                model.ChangedStartListIndividualList = changedStartlistIndividual;
            }

            return View("Index", model);
        }



        private Dictionary<int, Changed> ImportTeams(ExcelImportService excelImportService)
        {
            int StartListClassStepId;
            int[] competionClassesTdbIds;
            int testNumber;
            Dictionary<int, Changed> TeamsStartlistChanged = new Dictionary<int, Changed>();
            Changed changed;

        //senior lag
        competionClassesTdbIds = new []{ 367695, 367766, 367779 };
            StartListClassStepId = 1059; // Svår klass lag seniorer klass 1 -Grund 
            testNumber = 1;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged.Add(StartListClassStepId, changed);

            StartListClassStepId = 1061; //Svår klass lag seniorer klass 1 – Kür
            testNumber = 2;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged.Add(StartListClassStepId, changed);

            StartListClassStepId = 6; //Svår klass lag seniorer grund 2
            testNumber = 3;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged.Add(StartListClassStepId, changed);


            //horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, StartListClassStepId, testNumber);
            //_updateService.UpdateTeamHorseOrders(horseOrders);

            //Junior lag
            competionClassesTdbIds = new [] { 367763, 367768, 367780};
            StartListClassStepId = 5; // Svår klass juniorlag klass 2 -Grund  
            testNumber = 1;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged.Add(StartListClassStepId, changed);

            StartListClassStepId = 1062; //Svår klass lag seniorer klass 1 – Kür
            testNumber = 2;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged.Add(StartListClassStepId, changed);

            StartListClassStepId = 1063; //Svår klass lag seniorer klass 1 – Kür
            testNumber = 3;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged.Add(StartListClassStepId, changed);


            //Mellanklass lag
            //competionClassesTdbIds = new[] { 350456 };
            //StartListClassStepId = 55; // Mellanklass lag klass 9 - Grund 
            //testNumber = 1;
            //ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            //StartListClassStepId = 56; //Mellanklass klass 9 - Kür
            //testNumber = 2;
            //ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            ////Lätt klass Lag galopp klass 12
            //competionClassesTdbIds = new[] { 350458};
            //StartListClassStepId = 43; // Lätt klass Lag galopp klass 12 grund
            //testNumber = 1;
            //ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            ////horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, StartListClassStepId, testNumber);
            ////_updateService.UpdateTeamHorseOrders(horseOrders);

            //StartListClassStepId = 43; // Lätt klass Lag galopp klass 12 kür
            //testNumber = 2;
            //ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            ////horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, StartListClassStepId, testNumber);
            ////_updateService.UpdateTeamHorseOrders(horseOrders);

            ////Lätt klass Lag galopp klass 13
            //competionClassesTdbIds = new[] { 350459};
            //StartListClassStepId = 40; // Lätt klass Lag galopp klass 13 grund
            //testNumber = 1;
            //ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            //StartListClassStepId = 40; // Lätt klass Lag galopp klass 13 kür
            //testNumber = 2;
            //ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            return TeamsStartlistChanged;
        }

        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividuals(ExcelImportService excelImportService)
        {
            int StartListClassStepId;
            int[] competionClassesTdbIds;
            int testNumber;
            var individualStartlistChanged = new Dictionary<int, UpdateService.NewHordeorders>();
            UpdateService.NewHordeorders newHordeorders;

            // individuella
            //Svår klass
            competionClassesTdbIds = new [] { 367772, 367773, 367782, 367774, 367775, 368081}; //Svår klass individuella Juniorer-Miniorer

            StartListClassStepId = 1066; 
            testNumber = 1;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged.Add(StartListClassStepId,newHordeorders);

            StartListClassStepId = 9; // Svår klass individuella Juniorer-Miniorer – Kür
            testNumber = 2;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged.Add(StartListClassStepId, newHordeorders);

            StartListClassStepId = 8; // Svår klass individuella Juniorer-Miniorer – Kür
            testNumber = 3;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged.Add(StartListClassStepId, newHordeorders);

            StartListClassStepId = 1065; // Svår klass individuella Juniorer-Miniorer – Kür
            testNumber = 4;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged.Add(StartListClassStepId, newHordeorders);


            competionClassesTdbIds = new [] { 367769, 367770, 367781 }; // senior individuell 

            StartListClassStepId = 19; // senior   Grund
            testNumber = 1;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged.Add(StartListClassStepId, newHordeorders);

            StartListClassStepId = 1060; // Svår klass individuella senior– teknKür
            testNumber = 2; //tekn kür
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged.Add(StartListClassStepId, newHordeorders);

            StartListClassStepId = 1064; // Svår klass individuella seniorklass Kür
            testNumber = 3;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged.Add(StartListClassStepId, newHordeorders);



            //competionClassesTdbIds = new [] { 350452};
            //StartListClassStepId = 46; // Lätt klass individuell galopp klass 7
            //testNumber = 1;
            //ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            //StartListClassStepId = 46; // Lätt klass individuell galopp klass 7
            //testNumber = 2;
            //ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            //competionClassesTdbIds = new [] { 350455 };
            //StartListClassStepId = 39; // Lätt klass individuell galopp klass 8
            //testNumber = 1;
            //ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            //StartListClassStepId = 39; // Lätt klass individuell galopp klass 8
            //testNumber = 2;
            //ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            return individualStartlistChanged;
        }

        private UpdateService.NewHordeorders ImportIndividual(ExcelImportService excelImportService, int[] competionClassesTdbIds,
            int startListClassStepId, int testNumber)
        {
            var horseOrders = excelImportService.GetHorseOrderIndividual(competionClassesTdbIds, startListClassStepId, testNumber);

           var newHordeorders = _updateService.UpdateIndividualHorseOrders(horseOrders);

            return newHordeorders;
        }

        private Changed ImportTeam(ExcelImportService excelImportService, int[] competionClassesTdbIds, int startListClassStepId,
            int testNumber)
        {
            var horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, startListClassStepId, testNumber);
            var changed = _updateService.UpdateTeamHorseOrders(horseOrders);
            return changed;
        }
    }
}
