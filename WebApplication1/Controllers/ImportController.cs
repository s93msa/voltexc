using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using WebApplication1.Business.Logic.Import;

namespace WebApplication1.Controllers
{
    public class ImportController : Controller
    {


        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            var requestService = new RequestService(Request);
            var workbook = requestService.GetWorkbook();
            var excelImportService = new ExcelImportService(workbook);
            var updateservice = new UpdateService();
            var lungers = excelImportService.GetLungers();
            updateservice.UpdateLungers(lungers);

            var horses = excelImportService.GetHorses();
            updateservice.UpdateHorses(horses);

            var clubs = excelImportService.GetClubs();
            updateservice.UpdateClubs(clubs);

            var classes = excelImportService.GetClasses();
            updateservice.UpdateClasses(classes);


            var vaulters = excelImportService.GetVaulters();
            updateservice.UpdateVaulters(vaulters);

            return RedirectToAction("Index");
        }
    }
}
