using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace VoltigeCore.Business.Logic.Excel.Results
{
    public class ExcelResultService
    {
        private const string ExcelFileExtension = ".xlsx";
        private ExcelBaseService _excelBaseService;
        private string _ExcelPathAndName;

        public ExcelResultService()
        {
            _ExcelPathAndName = AppConfig.StartlistOutputPath + "MagnusL";
            var workbook = new XLWorkbook(_ExcelPathAndName + ExcelFileExtension);
            _excelBaseService = new ExcelBaseService(workbook);
        }

        public void Save() => _excelBaseService.SaveExcelFile();

        public void SaveWithTimeStamp()
        {
            _excelBaseService.SaveExcelFile(_ExcelPathAndName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ExcelFileExtension);
        }

        public void SetCompetitionClasses(CompetitionClass[] competetionClasses)
        {
            var rows = ConvertClassListToArraryList(competetionClasses);
            _excelBaseService.SetValuesInWorkSheet("Klasser", 2, rows);
        }

        public void SetVaulterList(Participant[] participants)
        {
            var rows = ConvertClassListToArraryList(participants);
            _excelBaseService.SetValuesInWorkSheet("Deltagare", 2, rows);
        }

        private ICollection<Row<Cell<string>>> ConvertClassListToArraryList<T>(T[] classes)
        {
            var rows = new List<Row<Cell<string>>>();
            foreach (T classIntance in classes)
            {
                var row = ConvertClassToArray(classIntance);
                rows.Add(new Row<Cell<string>>(row));
            }
            return rows;
        }

        private ICollection<Cell<string>> ConvertClassToArray<T>(T classToConvert)
        {
            return classToConvert.GetType()
                .GetProperties()
                .Select(p =>
                {
                    object value = p.GetValue(classToConvert, null);
                    return value == null ? null : new Cell<string>(value.ToString());
                })
                .ToList();
        }
    }
}
