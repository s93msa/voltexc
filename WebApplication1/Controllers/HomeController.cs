using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CopyExcel()
        {
            Contest contest;
            List<StartListClass> d;
            using (var db = new VaultingContext())
            {
                contest = (from contestTable in db.Contests
                             select contestTable).FirstOrDefault();
                d = contest?.StartListClass;

                //foreach (var item in query)
                //{
                //    country = item.Country;
                //}
            }
            ////CreateFolders();
            //ViewBag.Message = "CopyExcel.";

            string fileName = @"C:\Temp\Test_Voligemallar\Protokoll 2017 individuell klass.xlsx";

            var workbook = new XLWorkbook(fileName);
            var worksheetHorse = workbook.Worksheet(2);
            worksheetHorse.Cell(7, "c").Value = contest?.Country;

            worksheetHorse.Cell(7, "d").Value = d?.Count;

            workbook.SaveAs(@"C:\Temp\Test_Voligemallar\HelloWorld.xlsx");
            return View();
        }

       
    }
}