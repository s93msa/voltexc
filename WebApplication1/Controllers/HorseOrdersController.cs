using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Classes;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HorseOrdersController : Controller
    {
        //TODO: skapa UI för att rediger horsorder 

        private VaultingContext db = new VaultingContext();

        // GET: HorseOrders
        public ActionResult Index()
        {
            //var s = db.StartListClassSteps.
            //     Select(startListClassStep =>
            //         new 
            //         {
            //             s = startListClassStep,
            //             startlist = startListClassStep.StartList.OrderBy(order => order.StartNumber)
            //         }).AsEnumerable();

            //return View(s);



            //return View(db.StartListClassSteps.Include(x => x.StartList.Select(startList => startList.StartNumber)).OrderBy(x => x.StartOrder).ThenBy(startList => startList.StartOrder).ToList());
            return View(db.StartListClassSteps.Find(1)?.StartList.First());
        }

        // GET: HorseOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorseOrder horseOrder = db.HorseOrders.Find(id);
            if (horseOrder == null)
            {
                return HttpNotFound();
            }
            return View(horseOrder);
        }

        // GET: HorseOrders/Create
        public ActionResult Create()
        {
            var club = db.Clubs.Find(1);
            //var club = new Club {ClubId = 1};
            var b = new Vaulter {Name = "Testname"};

            b.VaultingClub = club;
            db.Vaulters.Add(b);
            db.SaveChanges();
            return View();
        }

        // POST: HorseOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HorseOrderId,StartNumber,IsTeam,TeamTestnumber")] HorseOrder horseOrder)
        {
            if (ModelState.IsValid)
            {
                db.HorseOrders.Add(horseOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(horseOrder);
        }

        // GET: HorseOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorseOrder horseOrder = db.HorseOrders.Find(id);
            if (horseOrder == null)
            {
                return HttpNotFound();
            }

            var horseOrderSelectList = GetHorsesSelectList(horseOrder);

            var stepsSelectList = GetStepsSelectList(horseOrder);

            var contest = ContestService.GetContestInstance();

            var startListClassesSteps = contest?.StartListClassStep ?? new List<StartListClassStep>();

            var startListClassStepOrdered = startListClassesSteps.OrderBy(x => x.StartOrder);
            var startListClassStepsSelectList = new SelectList(startListClassStepOrdered, "StartListClassStepId", "Name", horseOrder.StartListClassStepId);

            var teams = ContestService.GetTeams().OrderBy(x => x.Name);
            var teamsSelectList = new SelectList(teams, "TeamId", "Name", horseOrder.VaultingTeamId);


            var horseOrderViewModel =
                new HorseOrderViewModel
                {
                    HorseOrder = horseOrder,
                    HorsesLungersSelectList = horseOrderSelectList,
                    StepsSelectList = stepsSelectList,
                    StartListClassStepsSelectList = startListClassStepsSelectList,
                    TeamsSelectList = teamsSelectList
                };

            return View(horseOrderViewModel);
        }

        // GET: HorseOrders/Edit/5
        public ActionResult EditVaulterOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaulterOrder vaulterOrder = db.VaulterOrders.Find(id);
            if (vaulterOrder == null)
            {
                return HttpNotFound();
            }



            //var horseOrderSelectList = GetHorsesSelectList(horseOrder);

            var stepsSelectList = GetStepsSelectList(vaulterOrder);

            //var contest = ContestService.GetContestInstance();

            //var startListClassesSteps = contest?.StartListClassStep ?? new List<StartListClassStep>();

            //var startListClassStepOrdered = startListClassesSteps.OrderBy(x => x.StartOrder);
            //var startListClassStepsSelectList = new SelectList(startListClassStepOrdered, "StartListClassStepId", "Name", horseOrder.StartListClassStepId);

            //var teams = ContestService.GetTeams().OrderBy(x => x.Name);
            //var teamsSelectList = new SelectList(teams, "TeamId", "Name", horseOrder.VaultingTeamId);


            var vaultersOrderViewModel =
                new VaultersOrderViewModel
                {
                    StepsSelectList = stepsSelectList,
                    VaulterOrder = vaulterOrder,
                    VaulterSelectList = GetVaultersSelectList(vaulterOrder),
                    //    HorseOrder = horseOrder,
                    //    HorsesLungersSelectList = horseOrderSelectList,
                    //    StepsSelectList = stepsSelectList,
                    //    StartListClassStepsSelectList = startListClassStepsSelectList,
                    //    TeamsSelectList = teamsSelectList
                };
            return View(vaultersOrderViewModel);
        }

        private SelectList GetStepsSelectList(HorseOrder horseOrder)
        {
            var teamClass = horseOrder.VaultingTeam.VaultingClass;
            var contest = ContestService.GetContestInstance();

            var classSteps = teamClass.GetCompetitionSteps(contest.TypeOfContest)?.OrderBy(x => x.TestNumber);

            var stepsSelectList = new SelectList(classSteps, "TestNumber", "Name", horseOrder.TeamTestnumber);
            return stepsSelectList;
        }

        private SelectList GetStepsSelectList(VaulterOrder vaulterOrder)
        {
            var vaulterClass = vaulterOrder.Participant.VaultingClass;
            var contest = ContestService.GetContestInstance();

            var classSteps = vaulterClass.GetCompetitionSteps(contest.TypeOfContest)?.OrderBy(x => x.TestNumber);

            var stepsSelectList = new SelectList(classSteps, "TestNumber", "Name", vaulterOrder.Testnumber);
            return stepsSelectList;
        }


    private SelectList GetVaultersSelectList(VaulterOrder vaulterOrder)
        {
            var vaulters = ContestService.GetVaulters().OrderBy(x => x.Name);
            var vaulterId = vaulterOrder.VaulterId;
            var vaultersSelectList = new List<KeyValuePair<int, string>>();
            foreach (var vaulterPair in vaulters)
            {
                if(vaulterPair == null)
                    continue;

                var key = vaulterPair.VaulterId;
                var value = vaulterPair.Name + " (Klubb: " + vaulterPair.VaultingClub?.ClubName + " klass: " + vaulterPair.VaultingClass?.ClassName +")";
                var pair = new KeyValuePair<int, string>(key, value);
                vaultersSelectList.Add(pair);

            }

            var stepsSelectList = new SelectList(vaultersSelectList, "key", "value", vaulterId);
            return stepsSelectList;
        }

        private SelectList GetHorsesSelectList(HorseOrder horseOrder)
        {
            var horses = ContestService.GetHorses().OrderBy(x => x.HorseName);
            var horsesSelectList = new List<KeyValuePair<int, string>>();
            foreach (var horse in horses)
            {
                if (horse == null)
                    continue;
                var key = horse.HorseId;
                var value = horse.HorseName + " " + horse.Lunger?.LungerName;
                var pair = new KeyValuePair<int, string>(key, value);
                horsesSelectList.Add(pair);
            }

            var horseOrderSelectList = new SelectList(horsesSelectList, "key", "value", horseOrder.HorseId);
            return horseOrderSelectList;
        }

        // POST: HorseOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "HorsesLungersSelectList")]  HorseOrderViewModel horseOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                var horseOrder = horseOrderViewModel.HorseOrder;
                var existingHorseOrder = ContestService.GetHorseOrder(horseOrder.HorseOrderId);
                existingHorseOrder.StartNumber = horseOrder.StartNumber;
                existingHorseOrder.HorseId = horseOrder.HorseId;
                existingHorseOrder.VaultingTeamId = horseOrder.VaultingTeamId;
                existingHorseOrder.TeamTestnumber = horseOrder.TeamTestnumber;
                existingHorseOrder.IsActive = horseOrder.IsActive;
                existingHorseOrder.StartListClassStepId = horseOrder.StartListClassStepId;

                ContestService.UpdateHorseOrder(existingHorseOrder);
                ContestService.GetNewDataFromDatabase();
                return RedirectToAction("StartList", "Home");
            }
            return View(horseOrderViewModel);
        }

        // GET: HorseOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorseOrder horseOrder = db.HorseOrders.Find(id);
            if (horseOrder == null)
            {
                return HttpNotFound();
            }
            return View(horseOrder);
        }

        // POST: HorseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HorseOrder horseOrder = db.HorseOrders.Find(id);
            db.HorseOrders.Remove(horseOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
