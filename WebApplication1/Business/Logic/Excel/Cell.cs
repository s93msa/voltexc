using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Business.Logic.Excel
{
    public class Cell<T>
    {
        public T CellValue { get; set; }
        public ExcelFontStyle FontStyle { get; set; }


        public Cell(T value)
        {
            CellValue = value;
        }

    }

    public enum ExcelFontStyle
    {
        Bold = 1
    }
}