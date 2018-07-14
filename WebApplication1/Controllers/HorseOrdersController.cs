using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Classes;
using WebApplication1.Models;

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
            return View(horseOrder);
        }

        // POST: HorseOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HorseOrderId,StartNumber,IsTeam,TeamTestnumber")] HorseOrder horseOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(horseOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(horseOrder);
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
