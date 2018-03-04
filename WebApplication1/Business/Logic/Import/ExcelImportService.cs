using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Import
{
    public class ExcelImportService
    {
        private XLWorkbook _workbook;

        public ExcelImportService(XLWorkbook workbook)
        {
            this._workbook = workbook;
            _workbook = workbook;

           


            //var fileName = Path.GetFileName(file.FileName);
            //var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            //file.SaveAs(path);
        }


        public Lunger[] GetLungers()
        {
            var lungers = new List<Lunger>();
            var worksheet = _workbook.Worksheets?.Worksheet("Voltigörer");
            foreach (var row in worksheet.Rows())
            {
                var lunger = new Lunger();
                lunger.LungerTdbId = Convert.ToInt32(row.Cell("b").Value);
                lunger.LungerName = row.Cell("c").Value.ToString();
                lungers.Add(lunger);
            }

            return lungers.ToArray();

           
        }

        public Horse[] GetHorses()
        {
            var horses = GetHorsesWorksheet();



            return horses;
        }

        private Horse[] GetHorsesWorksheet()
        {
            var horses = new List<Horse>();
            var worksheet = _workbook.Worksheets?.Worksheet("Hästar");
            foreach (var row in worksheet.Rows())
            {
                var horse = new Horse();
                horse.HorseTdbId = Convert.ToInt32(row.Cell("b").Value);
                horse.HorseName = row.Cell("c").Value.ToString();
                horses.Add(horse);
            }

            return horses.ToArray();
        }
    }
}


