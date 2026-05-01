using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoltigeCore.Business.Logic.Import;
using VoltigeCore.Controllers.DTO;
using VoltigeCore.ViewModels;

namespace VoltigeCore.Controllers
{
    public class ImportController : Controller
    {
        private readonly RequestService _requestService;
        private readonly UpdateService _updateService;

        public ImportController()
        {
            _requestService = new RequestService();
            _updateService = new UpdateService();
        }

        // GET: Import
        public IActionResult Index()
        {
            return View(new ImportViewModel());
        }

        [HttpPost]
        public IActionResult Upload(ImportViewModel model, IFormFile file, string addNewCheckbox, string updateCheckbox, string excludeStartlist, string excludeTeam)
        {
            _updateService.AddNew = _requestService.IsCheckboxChecked(addNewCheckbox);
            _updateService.UpdateExisting = _requestService.IsCheckboxChecked(updateCheckbox);
            _updateService.ExcludeStartlist = _requestService.IsCheckboxChecked(excludeStartlist);
            _updateService.ExcludeTeam = _requestService.IsCheckboxChecked(excludeTeam);

            var workbook = _requestService.GetWorkbook(file);
            var excelImportService = new ExcelImportService(workbook);

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

            var startlistSteps = excelImportService.GetStartlistSteps();
            _updateService.UpdateStartOrderSteps(startlistSteps);

            if (!_updateService.ExcludeTeam)
            {
                var teams = excelImportService.GetTeams();
                changedItems = _updateService.UpdateTeams(teams);
                model.NewTeams = changedItems.New;
                model.UpdatedTeams = changedItems.Updated;

                var teamMembers = excelImportService.GetTeamMembers();
                changedItems = _updateService.UpdateTeamMembers(teamMembers);
                model.NewTeamMembers = changedItems.New;
                model.UpdatedTeamMembers = changedItems.Updated;
            }

            var horseOrders = excelImportService.GetTeamsHorseordersFromStartList();
            if (horseOrders != null)
            {
                var horseOrdersTeams = horseOrders.Where(horseOrder => horseOrder.IsTeam).ToArray();
                _updateService.UpdateTeamHorseOrders(horseOrdersTeams);

                var horseOrdersIndividual = horseOrders.Where(horseOrder => !horseOrder.IsTeam).ToArray();
                _updateService.UpdateIndividualHorseOrders(horseOrdersIndividual);

                return View("Index", model);
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
            var teamsStartlistChanged = new Dictionary<int, Changed>();
            teamsStartlistChanged = TeamOneDayCompetion(excelImportService, teamsStartlistChanged);
            return teamsStartlistChanged;
        }

        private Dictionary<int, Changed> TeamOneDayCompetion(ExcelImportService excelImportService, Dictionary<int, Changed> teamsStartlistChanged)
        {
            var stepIdsWithClasses = excelImportService.GetStartlistStepCompetionClasses();
            return ImportTeam(excelImportService, teamsStartlistChanged, stepIdsWithClasses);
        }

        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividuals(ExcelImportService excelImportService)
        {
            var individualStartlistChanged = new Dictionary<int, UpdateService.NewHordeorders>();
            individualStartlistChanged = ImportIndividualOnedayCompetition(excelImportService, individualStartlistChanged);
            return individualStartlistChanged;
        }

        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividualOnedayCompetition(ExcelImportService excelImportService,
            Dictionary<int, UpdateService.NewHordeorders> individualStartlistChanged)
        {
            var stepIdsWithClasses = excelImportService.GetStartlistStepCompetionClasses();
            individualStartlistChanged = ImportIndividuals(excelImportService, individualStartlistChanged, stepIdsWithClasses);
            return individualStartlistChanged;
        }

        private Dictionary<int, UpdateService.NewHordeorders> ImportIndividuals(ExcelImportService excelImportService,
            Dictionary<int, UpdateService.NewHordeorders> individualStartlistChanged, List<StepIdWithClasses> stepIdsWithClasses)
        {
            foreach (var stepIdWithClasses in stepIdsWithClasses)
            {
                var newHordeorders = ImportIndividual(excelImportService, stepIdWithClasses);
                individualStartlistChanged = UpdateChangeList(individualStartlistChanged, stepIdWithClasses.StartListClassStepId, newHordeorders);
            }
            return individualStartlistChanged;
        }

        private Dictionary<int, Changed> ImportTeam(ExcelImportService excelImportService, Dictionary<int, Changed> teamsStartlistChanged,
            List<StepIdWithClasses> stepIdsWithClasses)
        {
            foreach (var stepIdWithClasses in stepIdsWithClasses)
            {
                var changed = ImportTeam(excelImportService, stepIdWithClasses);
                teamsStartlistChanged = AddToChangedLogg(teamsStartlistChanged, stepIdWithClasses.StartListClassStepId, changed);
            }
            return teamsStartlistChanged;
        }

        private UpdateService.NewHordeorders ImportIndividual(ExcelImportService excelImportService, int[] competionClassesTdbIds,
            int startListClassStepId, int testNumber)
        {
            var horseOrders = excelImportService.GetHorseOrderIndividual(competionClassesTdbIds, startListClassStepId, testNumber);
            return _updateService.UpdateIndividualHorseOrders(horseOrders);
        }

        private UpdateService.NewHordeorders ImportIndividual(ExcelImportService excelImportService, StepIdWithClasses startListTdbClasses)
        {
            var horseOrders = excelImportService.GetHorseOrderIndividual(startListTdbClasses.StartListClassStepId, startListTdbClasses.CompetitionClassesTdbIds);
            return _updateService.UpdateIndividualHorseOrders(horseOrders);
        }

        private Changed ImportTeam(ExcelImportService excelImportService, StepIdWithClasses startListTdbClasses)
        {
            var horseOrders = excelImportService.GetHorseOrdersTeam(startListTdbClasses.StartListClassStepId, startListTdbClasses.CompetitionClassesTdbIds);
            return _updateService.UpdateTeamHorseOrders(horseOrders);
        }

        private Changed ImportTeam(ExcelImportService excelImportService, int[] competionClassesTdbIds, int startListClassStepId, int testNumber)
        {
            var horseOrders = excelImportService.GetHorseOrdersTeam(competionClassesTdbIds, startListClassStepId, testNumber);
            return _updateService.UpdateTeamHorseOrders(horseOrders);
        }

        private static Dictionary<int, Changed> AddToChangedLogg(Dictionary<int, Changed> teamsStartlistChanged, int startListClassStepId, Changed changed)
        {
            if (teamsStartlistChanged.TryGetValue(startListClassStepId, out var currentChanged))
            {
                currentChanged.Updated += changed.Updated;
                currentChanged.New += changed.New;
                teamsStartlistChanged[startListClassStepId] = currentChanged;
                return teamsStartlistChanged;
            }
            teamsStartlistChanged.Add(startListClassStepId, changed);
            return teamsStartlistChanged;
        }

        private static Dictionary<int, UpdateService.NewHordeorders> UpdateChangeList(Dictionary<int, UpdateService.NewHordeorders> individualStartlistChanged,
            int startListClassStepId, UpdateService.NewHordeorders newHordeorders)
        {
            if (individualStartlistChanged.TryGetValue(startListClassStepId, out var current))
            {
                current.NewHorseOrders += newHordeorders.NewHorseOrders;
                current.NewVaulterOrders += newHordeorders.NewVaulterOrders;
                individualStartlistChanged[startListClassStepId] = current;
                return individualStartlistChanged;
            }
            individualStartlistChanged.Add(startListClassStepId, newHordeorders);
            return individualStartlistChanged;
        }
    }
}
