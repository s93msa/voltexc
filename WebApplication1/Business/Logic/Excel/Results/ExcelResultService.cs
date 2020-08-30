using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Business.Logic.Excel;

namespace WebApplication1.Business.Logic.Excel.Results
{
    public class ExcelResultService
    {
        private const string ExcelFileExtension = ".xlsx";
        private ExcelBaseService _excelBaseService;
        private string _ExcelPathAndName;
        public ExcelResultService()
        {
            var workingdirectory = System.Web.Hosting.HostingEnvironment.MapPath("~");
            _ExcelPathAndName = workingdirectory + @"..\output\MagnusL";
            var workbook = new XLWorkbook(_ExcelPathAndName + ExcelFileExtension);

            _excelBaseService = new ExcelBaseService(workbook);
        }

        public void Save()
        {
            _excelBaseService.SaveExcelFile();
        }
        public void SaveWithTimeStamp()
        {
            _excelBaseService.SaveExcelFile(_ExcelPathAndName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ExcelFileExtension);
        }
        public void SetCompetitionClasses(CompetitionClass[] competetionClasses)
        {
            const string worksheetName = "Klasser";

            var rows = ConvertClassListToArraryList(competetionClasses);

            _excelBaseService.SetValuesInWorkSheet(worksheetName, 2, rows);
        }

        public void SetVaulterList(Participant[] participants)
        {
            const string worksheetName = "Deltagare";

            var rows = ConvertClassListToArraryList(participants);

            _excelBaseService.SetValuesInWorkSheet(worksheetName, 2, rows);
        }

        private Row<string>[] ConvertClassListToArraryList<T>(T[] classes)
        {
            List<Row<string>> rows = new List<Row<string>>();
            foreach (T classIntance in classes)
            {
                string[] row = ConvertClassToArray(classIntance);
                rows.Add(new Row<string>(row));
            }

            return rows.ToArray();
        }

        private string[] ConvertClassToArray<T>(T classToConvert)
        {
            return classToConvert.GetType()
                    .GetProperties()
                    .Select(p =>
                        {
                            object value = p.GetValue(classToConvert, null);
                            return value == null ? null : value.ToString();
                        })
                    .ToArray();
        }
    }
}