using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Classes;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class EditVaulterHorseController : Controller
    {
        private VaultingContext db = new VaultingContext();

        // GET: EditVaulterHorse
        public ActionResult Index(int id)
        {
            HorseOrder horseOrder = db.HorseOrders.Find(id);
            if (horseOrder == null)
            {
                return HttpNotFound();
            }

            //var horseOrderSelectList = GetHorsesSelectList(horseOrder);

            var startListClassStepsSelectList = GetStartListClassStepsSelectList(horseOrder);
            var horseOrderSelectList = GetHorsesSelectList(horseOrder.StartListClassStepId??0, horseOrder.HorseOrderId);

            var editVaulterHorseViewModel =
                new EditVaulterHorseViewModel
                {
                    StartListClassStepId = horseOrder.StartListClassStepId??0,
                    StartListClassStepsSelectList = startListClassStepsSelectList,
                    HorseOrderId = horseOrder.HorseOrderId,
                    HorseOrderSelectList = horseOrderSelectList
                };
            return View(editVaulterHorseViewModel);
        }

      

        // GET: EditVaulterHorse/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EditVaulterHorse/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EditVaulterHorse/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EditVaulterHorse/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EditVaulterHorse/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EditVaulterHorse/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EditVaulterHorse/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private static SelectList GetStartListClassStepsSelectList(HorseOrder horseOrder)
        {
            //var stepsSelectList = GetStepsSelectList(horseOrder);
            var contest = ContestService.GetContestInstance();

            var startListClassesSteps = contest?.StartListClassStep ?? new List<StartListClassStep>();

            var startListClassStepOrdered = startListClassesSteps.OrderBy(x => x.StartOrder);
            var startListClassStepsSelectList = new SelectList(startListClassStepOrdered, "StartListClassStepId", "Name",
                horseOrder.StartListClassStepId);
            return startListClassStepsSelectList;
        }

        //kopia av koden i HorseOrdersController
        private SelectList GetHorsesSelectList(int startListClassStepId, int selectedHorseOrder)
        {
            var horseOrders = ContestService.GetHorseOrders(startListClassStepId).OrderBy(x => x.HorseInformation.HorseName);
            var horseOrdersSelectList = new List<KeyValuePair<int, string>>();
            foreach (var horseOrder in horseOrders)
            {
                var key = horseOrder.HorseOrderId;
                var value = horseOrder.HorseInformation.HorseName + " " + horseOrder.HorseInformation.Lunger.LungerName;
                var pair = new KeyValuePair<int, string>(key, value);
                horseOrdersSelectList.Add(pair);
            }

            var horseOrderSelectList = new SelectList(horseOrdersSelectList, "key", "value", selectedHorseOrder);
            return horseOrderSelectList;
        }

       

        
    }
}
