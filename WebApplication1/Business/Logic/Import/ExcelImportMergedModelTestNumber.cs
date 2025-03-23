using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Business.Logic.Import
{
    public class ExcelImportMergedModelTestNumber
    {
        public int testNumber;
        public ExcelImportMergedModel excelImportMergedModel;

        public ExcelImportMergedModelTestNumber(ExcelImportMergedModel excelImportMergedModel, int testNumber)
        {
            this.testNumber = testNumber;
            this.excelImportMergedModel = excelImportMergedModel;
        }
    }
}