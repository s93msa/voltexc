using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using WebApplication1.Business.Logic.Import;
using WebApplication1.Controllers.DTO;
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
        public ActionResult Upload(ImportViewModel model, HttpPostedFileBase file, string addNewCheckbox, string updateCheckbox, string excludeStartlist, string excludeTeam)
        {



            _requestService = new RequestService(Request);
            _updateService.AddNew = _requestService.IsCheckboxChecked(addNewCheckbox);
            _updateService.UpdateExisting = _requestService.IsCheckboxChecked(updateCheckbox);
            _updateService.ExcludeStartlist = _requestService.IsCheckboxChecked(excludeStartlist);
            _updateService.ExcludeTeam = _requestService.IsCheckboxChecked(excludeTeam);
            
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

            if (!_updateService.ExcludeTeam)
            {
                var teams = excelImportService.GetTeams();
                changedItems = _updateService.UpdateTeams(teams);
                model.NewTeams = changedItems.New;
                model.UpdatedTeams = changedItems.Updated;

                var teamMembers = excelImportService.GetTeamMembers();
                changedItems = _updateService.UpdateTeamMembers(teamMembers); //Observera att den inte tar bort teammembers. Bara lägger till eller ändrar ordning
                model.NewTeamMembers = changedItems.New;
                model.UpdatedTeamMembers = changedItems.Updated;
            }

            if (!_updateService.ExcludeStartlist)
            {
                if (!_updateService.ExcludeTeam)
                {
                    var changedStartlist = ImportTeams(excelImportService);
                    model.ChangedStartListTeamList = changedStartlist;
                }

                var changedStartlistIndividual = ImportIndividuals(excelImportService);
                model.ChangedStartListIndividualList = changedStartlistIndividual;
            }

            return View("Index", model);
        }



        private Dictionary<int, Changed> ImportTeams(ExcelImportService excelImportService)
        {
            Dictionary<int, Changed> TeamsStartlistChanged = new Dictionary<int, Changed>();
  
            //TeamsStartlistChanged = TeamOneDayTraHast(excelImportService, TeamsStartlistChanged);
            TeamsStartlistChanged = TeamOneDayCompetion(excelImportService, TeamsStartlistChanged);
      //      TeamsStartlistChanged = TeamSMCompetion(excelImportService, TeamsStartlistChanged);

            return TeamsStartlistChanged;
        }

        private Dictionary<int, Changed> TeamOneDayCompetion(ExcelImportService excelImportService, Dictionary<int, Changed> TeamsStartlistChanged)
        {
            var startListTdbClasses = new List<StartListTdbClasses>();

            //Svår klass individuella senior med och utan tekn, Juniorer-Miniorer
            var startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441642 },             //senior lag
                StepMoment = new StepMoment[]
                {
                    new StepMoment { StartListClassStepId = 18, TestNumber = 1 },
                    new StepMoment { StartListClassStepId = 7, TestNumber = 2 }
                }
            };
            startListTdbClasses.Add(startListTdbClass);

            startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441643 },            //Junior lag
                StepMoment = new StepMoment[]
                {
                    new StepMoment { StartListClassStepId = 16, TestNumber = 1 },
                    new StepMoment { StartListClassStepId = 10, TestNumber = 2 }
                }
            };
            startListTdbClasses.Add(startListTdbClass);

            startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441651 },           //Mixklass lag
                StepMoment = new StepMoment[]
                {
                    new StepMoment { StartListClassStepId = 52, TestNumber = 1 },
                    new StepMoment { StartListClassStepId = 52, TestNumber = 2 }
                }
            };
            startListTdbClasses.Add(startListTdbClass);

            startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441653},          //Lätt klass Lag galopp och skritt  
                StepMoment = new StepMoment[]
                {
                    new StepMoment { StartListClassStepId = 43, TestNumber = 1 },
                    new StepMoment { StartListClassStepId = 43, TestNumber = 2 }
                }
            };
            startListTdbClasses.Add(startListTdbClass);




            TeamsStartlistChanged = ImportTeam(excelImportService, TeamsStartlistChanged, startListTdbClasses);
      

            //StartListClassStepId = 1063; //Svår klass lag seniorer klass 1 – Kür
            //testNumber = 3;
            //changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //TeamsStartlistChanged.Add(StartListClassStepId, changed);


            //Mellanklass lag
            //competionClassesTdbIds = new[] {388234};
            //StartListClassStepId = 55; // Mellanklass lag klass 9 - Grund 
            //testNumber = 1;
            //changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            //StartListClassStepId = 56; //Mellanklass klass 9 - Kür
            //testNumber = 2;
            //changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            //horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, StartListClassStepId, testNumber);
            //_updateService.UpdateTeamHorseOrders(horseOrders);

            //changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);
            //horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, StartListClassStepId, testNumber);
            //_updateService.UpdateTeamHorseOrders(horseOrders);

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


        private Dictionary<int, Changed> TeamSMCompetion(ExcelImportService excelImportService, Dictionary<int, Changed> TeamsStartlistChanged)
        {
            int[] competionClassesTdbIds;
            int StartListClassStepId;
            int testNumber;
            Changed changed;
            //senior lag
            competionClassesTdbIds = new[] { 425383};
            StartListClassStepId = 1059; // Svår klass lag seniorer klass 1 -Grund 
            testNumber = 1;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            StartListClassStepId = 1061; //Svår klass lag seniorer klass 1 – Kür
            testNumber = 2;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            StartListClassStepId = 6; //Svår klass lag seniorer klass 1 – Kür
            testNumber = 3;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);


            //Junior lag
            competionClassesTdbIds = new[] { 425384};
            StartListClassStepId = 5; // Svår klass juniorlag klass 2 -Grund  
            testNumber = 1;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            StartListClassStepId = 1062; //Svår klass lag junior – Kür
            testNumber = 2;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            StartListClassStepId = 1063; //Svår klass lag junior – Kür
            testNumber = 3;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            //Mixklass lag
            //competionClassesTdbIds = new[] { 404706 };
            //StartListClassStepId = 52; //Grund och kür 
            //testNumber = 1;
            //changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            //StartListClassStepId = 52; //Grund och kür 
            //testNumber = 2;
            //changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);


            //Mellanklass lag
            //competionClassesTdbIds = new[] {388234};
            //StartListClassStepId = 55; // Mellanklass lag klass 9 - Grund 
            //testNumber = 1;
            //changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            //StartListClassStepId = 56; //Mellanklass klass 9 - Kür
            //testNumber = 2;
            //changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

           
            return TeamsStartlistChanged;
        }
        private Dictionary<int, Changed> TeamOneDayTraHast(ExcelImportService excelImportService, Dictionary<int, Changed> TeamsStartlistChanged)
        {
            int[] competionClassesTdbIds;
            int StartListClassStepId;
            int testNumber;
            Changed changed;
            //Senior lag
            competionClassesTdbIds = new[] { 1 };
            StartListClassStepId = 18; // Svår klass lag seniorer klass 1 -Grund 
            testNumber = 1;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            StartListClassStepId = 18; //Svår klass lag seniorer klass 1 – Kür
            testNumber = 2;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);


            //Junior lag
            competionClassesTdbIds = new[] { 2 };
            StartListClassStepId = 16; // Svår klass juniorlag klass 2 -Grund  
            testNumber = 1;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            StartListClassStepId = 16; //Svår klass lag junior – Kür
            testNumber = 2;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            //Pas de deux
            competionClassesTdbIds = new[] { 4 };
            StartListClassStepId = 1081; // Svår klass juniorlag klass 2 -Grund  
            testNumber = 1;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            //Lättklass lag
            competionClassesTdbIds = new[] { 10 };
            StartListClassStepId = 43; // SLättklass lag -Grund  
            testNumber = 1;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);

            StartListClassStepId = 43; //Lättklass lag – Kür
            testNumber = 2;
            changed = ImportTeam(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            TeamsStartlistChanged = AddToChangedLogg(TeamsStartlistChanged, StartListClassStepId, changed);


            return TeamsStartlistChanged;
        }

        private static Dictionary<int, Changed> AddToChangedLogg(Dictionary<int, Changed> TeamsStartlistChanged, int startListClassStepId, Changed changed)
        {
            Changed currentTeamsStartlistChanged;
            if (TeamsStartlistChanged.TryGetValue(startListClassStepId, out currentTeamsStartlistChanged))
            {
                currentTeamsStartlistChanged.Updated += changed.Updated;
                currentTeamsStartlistChanged.New += changed.New;
                TeamsStartlistChanged[startListClassStepId] = currentTeamsStartlistChanged;
                return TeamsStartlistChanged;
            }

            TeamsStartlistChanged.Add(startListClassStepId, changed);
            return TeamsStartlistChanged;
        }

        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividuals(ExcelImportService excelImportService)
        {
            var individualStartlistChanged = new Dictionary<int, UpdateService.NewHordeorders>();

            individualStartlistChanged = ImportIndividualOnedayCompetition(excelImportService, individualStartlistChanged);
//            individualStartlistChanged = ImportIndividualSmCompetition(excelImportService, individualStartlistChanged);

            return individualStartlistChanged;
        }

        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividualOnedayCompetition(ExcelImportService excelImportService,
            Dictionary<int, UpdateService.NewHordeorders> individualStartlistChanged)
        {
            

            var startListTdbClasses = new List<StartListTdbClasses>();

            //Svår klass individuella senior med och utan tekn, Juniorer-Miniorer
            var startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441644, 441645, 441646, 441647 },
                StepMoment = new StepMoment[] {new StepMoment {StartListClassStepId = 1, TestNumber = 1}}
            };

            startListTdbClasses.Add(startListTdbClass);

            startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441644 }, //Svår klass senio individuella senior med  tekn
                StepMoment = new StepMoment[] { new StepMoment { StartListClassStepId = 1, TestNumber = 2 } }
            };

            startListTdbClasses.Add(startListTdbClass);

            startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441644 }, //Svår klass senio individuella senior med  tekn
                StepMoment = new StepMoment[] { new StepMoment { StartListClassStepId = 2, TestNumber = 3 } }
            };
            startListTdbClasses.Add(startListTdbClass);

            startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441645, 441646, 441647 }, //Svår klass individuella senior med och utan tekn, Juniorer-Miniorer
                StepMoment = new StepMoment[] { new StepMoment { StartListClassStepId = 2, TestNumber = 2} }
            };

            startListTdbClasses.Add(startListTdbClass);

            startListTdbClass = new StartListTdbClasses
            {
                CompetitionClassesTdbIds = new[] { 441655, 441656 }, // Lätt klass individuell galopp coh skritt  klass 7,8
                StepMoment = new StepMoment[]
                {
                    new StepMoment { StartListClassStepId = 46, TestNumber = 1 },
                    new StepMoment { StartListClassStepId = 46, TestNumber = 2 }
                }
            };

            startListTdbClasses.Add(startListTdbClass);

            individualStartlistChanged = ImportIndividual(excelImportService, individualStartlistChanged, startListTdbClasses);



            //StartListClassStepId = 1065; // Svår klass individuella Juniorer-Miniorer – Kür
            //testNumber = 4;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged.Add(StartListClassStepId, newHordeorders);


            //competionClassesTdbIds = new [] { 367769, 367770, 367781 }; // senior individuell 

            //StartListClassStepId = 19; // senior   Grund
            //testNumber = 1;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged.Add(StartListClassStepId, newHordeorders);

            //StartListClassStepId = 1060; // Svår klass individuella senior– teknKür
            //testNumber = 2; //tekn kür
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged.Add(StartListClassStepId, newHordeorders);

            //StartListClassStepId = 1064; // Svår klass individuella seniorklass Kür
            //testNumber = 3;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged.Add(StartListClassStepId, newHordeorders);



 
            //competionClassesTdbIds = new [] { 350455 };
            //StartListClassStepId = 39; // Lätt klass individuell galopp klass 8
            //testNumber = 1;
            //ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);

            //StartListClassStepId = 39; // Lätt klass individuell galopp klass 8
            //testNumber = 2;
            //ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            return individualStartlistChanged;
        }

        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividual(ExcelImportService excelImportService, Dictionary<int, UpdateService.NewHordeorders> individualStartlistChanged,
            List<StartListTdbClasses> startListTdbClasses)
        {
            foreach (var startListTdbClassItem in startListTdbClasses)
            {
                foreach (var stepMoment in startListTdbClassItem.StepMoment)
                {
                    var newHordeorders = ImportIndividual(excelImportService, startListTdbClassItem.CompetitionClassesTdbIds,
                        stepMoment.StartListClassStepId, stepMoment.TestNumber);
                    individualStartlistChanged = UpdateChangeList(individualStartlistChanged, stepMoment.StartListClassStepId,
                        newHordeorders);
                }
            }

            return individualStartlistChanged;
        }

        private Dictionary<int, Changed> ImportTeam(ExcelImportService excelImportService, Dictionary<int, Changed> teamsStartlistChanged,
            List<StartListTdbClasses> startListTdbClasses)
        {
            foreach (var startListTdbClassItem in startListTdbClasses)
            {
                foreach (var stepMoment in startListTdbClassItem.StepMoment)
                {
                    var changed = ImportTeam(excelImportService, startListTdbClassItem.CompetitionClassesTdbIds,
                        stepMoment.StartListClassStepId, stepMoment.TestNumber);
                    teamsStartlistChanged = AddToChangedLogg(teamsStartlistChanged, stepMoment.StartListClassStepId, changed);
                }
            }

            return teamsStartlistChanged;
        }


        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividualSmCompetition(ExcelImportService excelImportService,
        Dictionary<int, UpdateService.NewHordeorders> individualStartlistChanged)
        {
            int[] competionClassesTdbIds;
            int StartListClassStepId;
            int testNumber;
            UpdateService.NewHordeorders newHordeorders;


            competionClassesTdbIds =
                new[] { 425385, 425386,
                    425387, 425388,
                    425393, 425394}; //individuella 

            StartListClassStepId = 1066;
            testNumber = 1;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            StartListClassStepId = 9; // Svår klass individuella senior – Kür
            testNumber = 2;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            competionClassesTdbIds =
                new[] { 425385, 425386,
                    425387, 425388}; //individuella exlusive nationella


            StartListClassStepId = 1064; // Svår klass individuella senior – tekn Kür och grund för ej tekn
            testNumber = 3;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);


            StartListClassStepId = 1065; // Svår klass individuella senior – Kür final
            testNumber = 4;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);






            //// individuella
            ////Svår klass alla klasser
            //competionClassesTdbIds =
            //    new[] { 425385, 425386}; //Svår klass 3,4 individuella senior med och utan tekn

            //StartListClassStepId = 19;
            //testNumber = 1;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            //StartListClassStepId = 1060; // Svår klass individuella senior – Kür
            //testNumber = 2;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);


            //StartListClassStepId = 1064; // Svår klass individuella senior – tekn Kür och grund för ej tekn
            //testNumber = 3;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);


            //StartListClassStepId = 1080; // Svår klass individuella senior – Kür final
            //testNumber = 4;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);


            //competionClassesTdbIds =
            //    new[] { 425387, 425388}; //Svår klass 5,6 individuella  Juniorer-Miniorer

            //StartListClassStepId = 1066; // Svår klass individuella Sen utan tekn Juniorer-Miniorer – Grund
            //testNumber = 1;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            //StartListClassStepId = 9; // Svår klass individuella Sen utan tekn Juniorer-Miniorer – Kür
            //testNumber = 2;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            //StartListClassStepId = 8; // Svår klass individuella Sen utan tekn Juniorer-Miniorer – Grund final
            //testNumber = 3;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            //StartListClassStepId = 1065; // Svår klass individuella Sen utan tekn Juniorer-Miniorer – Kür final
            //testNumber = 4;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            //competionClassesTdbIds =
            //    new[] { 425393, 425394 }; //Svår klass 12,13 individuella  Juniorer-Miniorer Nationell

            //StartListClassStepId = 1075; // Svår klass individuella Sen utan tekn Juniorer-Miniorer – Grund
            //testNumber = 1;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            //StartListClassStepId = 1077; // Svår klass individuella Sen utan tekn Juniorer-Miniorer – Grund
            //testNumber = 2;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);


            return individualStartlistChanged;
        }



        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividualTraHastCompetition(ExcelImportService excelImportService,
            Dictionary<int, UpdateService.NewHordeorders> individualStartlistChanged)
        {
            int[] competionClassesTdbIds;
            int StartListClassStepId;
            int testNumber;
            UpdateService.NewHordeorders newHordeorders;
            // individuella
            //Svår klass alla klasser
            competionClassesTdbIds =
                new[] { 5, 6, 7, 8 ,9  }; //Svår klass individuella senior med och utan tekn, Juniorer-Miniorer

            StartListClassStepId = 1;
            testNumber = 1;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);


            StartListClassStepId = 1; 
            testNumber = 2;
            newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);



            //competionClassesTdbIds =
            //    new[] { 9 }; //Lättklass, Individuell


            //StartListClassStepId = 39; // Svår klass individuella Sen utan tekn Juniorer-Miniorer – Kür
            //testNumber = 1;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            //testNumber = 2;
            //newHordeorders = ImportIndividual(excelImportService, competionClassesTdbIds, StartListClassStepId, testNumber);
            //individualStartlistChanged = UpdateChangeList(individualStartlistChanged, StartListClassStepId, newHordeorders);

            return individualStartlistChanged;
        }



        private static Dictionary<int, UpdateService.NewHordeorders> UpdateChangeList(Dictionary<int, UpdateService.NewHordeorders> individualStartlistChanged, int StartListClassStepId,
            UpdateService.NewHordeorders newHordeorders)
        {
            UpdateService.NewHordeorders currentNewHordeorders;
            if (individualStartlistChanged.TryGetValue(StartListClassStepId, out currentNewHordeorders))
            {
                currentNewHordeorders.NewHorseOrders += newHordeorders.NewHorseOrders;
                currentNewHordeorders.NewVaulterOrders += newHordeorders.NewVaulterOrders;
                individualStartlistChanged[StartListClassStepId] = currentNewHordeorders;
                return individualStartlistChanged;
            }

            individualStartlistChanged.Add(StartListClassStepId, newHordeorders);

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
