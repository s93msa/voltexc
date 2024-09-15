using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Business.Logic.Excel
{
    public class ExcelBaseService
    {
        private XLWorkbook _workbook;

        public ExcelBaseService(XLWorkbook workbook)
        {
            _workbook = workbook;
        }

        public void ShowOnlyWorksheet(IXLWorksheet worksheet)
        {
            IXLWorksheets workbookWorksheets = _workbook?.Worksheets;
            if (workbookWorksheets == null)
                return;


            foreach (var currrentWorksheet in workbookWorksheets)
            {
                if (currrentWorksheet == worksheet)
                {
                    currrentWorksheet.Visibility = XLWorksheetVisibility.Visible;
                    currrentWorksheet.TabActive = true;
                }
                else
                    currrentWorksheet.Hide(); //.Visibility = XLWorksheetVisibility.Hidden;
            }
        }

        public void SaveExcelFile()
        {
            _workbook.Save();
        }
            public void SaveExcelFile(string outputPathAndName)
        {
            if (outputPathAndName == null)
                return;
            outputPathAndName = outputPathAndName.Replace("&", "och").Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("*", string.Empty);
            _workbook.SaveAs(outputPathAndName);
        }

        //public void SetValuesInWorkSheet<T>(string worksheetName, int startRow, Row<T>[] rows)
        //{
        //    var worksheet = _workbook.Worksheets.Worksheet(worksheetName);

        //    int rowIndex = startRow;
        //    foreach (var row in rows)
        //    {
        //        SetRowValuesInWorksheet(worksheet, rowIndex, startColumn: 1, rowValues: row.RowValues);
        //        rowIndex++;
        //    }
        //}

        public void SetValuesInWorkSheet<T>(string worksheetName, int startRow, ICollection<Row<Cell<T>>> rows)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);

            int rowIndex = startRow;
            foreach (var row in rows)
            {
                SetRowValuesInWorksheet(worksheet, rowIndex, startColumn: 1, rowValues: row.RowValues.ToList());
                rowIndex++;
            }
        }

        //public void SetFormulaInWorkSheet(string worksheetName, int endRow)
        //{
        //    var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
        //    worksheet.Column(1).InsertColumnsBefore(1);
        //    worksheet.Column(1).InsertColumnsBefore(1);

        //    worksheet.Cell("A1").Value = new DateTime(2023, 9, 10, 9, 0, 0); ;
        //    worksheet.Column("A").Style.DateFormat.Format = "HH:mm";
        //    //= OM(D1 = 0; ""; B1 + KLOCKSLAG(0; 0; PRODUKTSUMMA(D1: D1) * 60))

        //    for (var currentRow = 2; currentRow <= endRow; currentRow++)
        //    {
        //        //var f = @"INDIRECT(ADDRESS(MAX((A1:A{currentRow}<>"""")*ROW(A1:A{currentRow})), COLUMN(C1)))";

        //        worksheet.Cell(currentRow, 2).FormulaA1 = "IF(C" + (currentRow) + "=0,\"\", A1+TIME(0,0,SUMPRODUCT(VALUE(C1:C" + (currentRow - 1) + "))*60))";
        //        //@"IF(C13=0, """", A1 + TIME(0, 0, SUMPRODUCT(VALUE(INDIRECT(ADDRESS(MAX((A1:A15<>"""")*ROW(A1:A15)), COLUMN(C1))):C15))*60)))";
        //    }
        //}


        public void SetFormulaInWorkSheet(string worksheetName, int endRow)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            worksheet.Column(1).InsertColumnsBefore(1);
            worksheet.Column(1).InsertColumnsBefore(1);

            worksheet.Cell("A1").Value = new DateTime(2023, 9, 10, 9, 0, 0); ;
            worksheet.Column("A").Style.DateFormat.Format = "HH:mm";
            //= OM(D1 = 0; ""; B1 + KLOCKSLAG(0; 0; PRODUKTSUMMA(D1: D1) * 60))

            for (var currentRow = 2; currentRow <= endRow; currentRow++)
            {
                var lastValue = $"LOOKUP(2, 1/(A$1:A{currentRow-1} <>\"\"), A$1:A{currentRow - 2})";
                // Create the dynamic reference using INDIRECT and ADDRESS
                var newTime = $@"INDIRECT(ADDRESS(MAX(ROW(A1:A{currentRow - 1})*(A1:A{currentRow - 1}<>"""")), COLUMN(C1)))";

                // Create the formula without the @ symbols by handling the array explicitly
                var formula = $"IF(C{currentRow}=0, \"\", {lastValue}+ TIME(0,0,SUMPRODUCT(VALUE({newTime}:C{currentRow - 1}))*60))";
                //var formula = $"IF(OR(C{currentRow}=0:A{currentRow}<>\"\") , \"\", {lastValue}+ TIME(0,0,SUMPRODUCT(VALUE({newTime}:C{currentRow - 1}))*60))";

                // Assign the formula to the worksheet cell
                worksheet.Cell(currentRow, 2).FormulaA1 = formula;
            }


            //for (var currentRow = 2; currentRow <= endRow; currentRow++)
            //{
            //    var f = $@"INDIRECT(ADDRESS(MAX((A1:A{currentRow - 1}<>"""")*ROW(A1:A{currentRow - 1})), COLUMN(C1)))";
            //    var formula = $"IF(C{currentRow}=0,\"\", A1+TIME(0,0,SUMPRODUCT(VALUE({f}:C{currentRow - 1}))*60))";
            //    worksheet.Cell(currentRow, 2).FormulaA1 = formula;                //worksheet.Cell(currentRow, 2).FormulaA1 = "IF(C" + (currentRow) + "=0,\"\", A1+TIME(0,0,SUMPRODUCT(VALUE(" + f + ":C" + (currentRow - 1) + "))*60))";
            //}
        }
        //@"IF(C13=0, """", A1 + TIME(0, 0, SUMPRODUCT(VALUE(INDIRECT(ADDRESS(MAX((A1:A15<>"""")*ROW(A1:A15)), COLUMN(C1))):C15))*60)))";
        //=OM(C13= 0;"";A1+KLOCKSLAG(0;0;PRODUKTSUMMA(TEXTNUM(INDIREKT(ADRESS(MAX((A$1:A$15<>"")*RAD(A$1:A$15)); KOLUMN(C$1))):C15))*60))

        //public void SetFormulaInWorkSheet(string worksheetName, int endRow)
        //{
        //    var worksheet = _workbook.Worksheets.Worksheet(worksheetName);

        //    // Insert some columns if needed
        //    worksheet.Column(1).InsertColumnsBefore(1);
        //    worksheet.Column(1).InsertColumnsBefore(1);

        //    // Set the initial time in A1
        //    worksheet.Cell("A1").Value = new DateTime(2023, 9, 10, 9, 0, 0);
        //    worksheet.Column("A").Style.DateFormat.Format = "HH:mm";

        //    // Set formulas in column B for each row
        //    for (var currentRow = 2; currentRow <= endRow; currentRow++)
        //    {
        //        // Formula to get the last non-empty value in column A and sum values from column C starting at that row
        //        worksheet.Cell(currentRow, 2).FormulaA1 = $@"
        //    IF(C{currentRow}=0, 
        //       """", 
        //       SUMPRODUCT((ROW(A$1:A{endRow})>=LOOKUP(2,1/(A$1:A{currentRow - 1}<>""""),ROW(A$1:A{currentRow - 1}))) * (C$1:C{endRow}))
        //    )";
        //    }
        //}


        //        Breakdown:
        //LETAUPP(2; ...): This is the LOOKUP function, which searches for the value 2. It is used here in a special way to find the last non-empty cell in a range.

        //1/(A$1:A15<>""):

        //This part creates an array of 1s and #DIV/0! errors. For each cell in the range A$1:A15, if the cell is not empty (<>""), it returns 1; otherwise, it returns a #DIV/0! error.
        //The LOOKUP function ignores errors like #DIV/0!, so it will only consider the 1s.
        //A$1:A15:

        //This is the range from which the formula will return the value.Since LOOKUP(2, ...) is trying to find the value 2, and since the array only contains 1s, LOOKUP will return the last non-empty value in the range A$1:A15.
        //What it does:
        //The formula finds the last non-empty value in the range A$1:A15.This is a common trick used to find the most recent entry in a column or range of cells.




        //public void SetRowValuesInWorksheet<T>(IXLWorksheet worksheet, int row, int startColumn, T[] rowValues)
        //{
        //    int columnIndex = startColumn;
        //    foreach (var cellValue in rowValues)
        //    {
        //        SetValueInWorksheet(worksheet, row, columnIndex, cellValue);
        //        columnIndex++;
        //    }
        //}
        public void SetRowValuesInWorksheet<T>(IXLWorksheet worksheet, int row, int startColumn, ICollection<Cell<T>> rowValues)
        {
            int columnIndex = startColumn;
            foreach (var cellValue in rowValues)
            {
                SetValueInWorksheet(worksheet, row, columnIndex, cellValue);
                columnIndex++;
            }
        }



        public void SetValueInWorksheet(IXLWorksheet worksheet, int row, string column, string value)
        {
            worksheet.Cell(row, column).Value = value;
        }
        //public void SetValueInWorksheet<T>(IXLWorksheet worksheet, int row, int column, T value)
        //{
        //    worksheet.Cell(row, column).Style.NumberFormat.Format = "@"; //format text för att klara när klassen har punkt och nollor tex 1.40 det får inte ändras till 1,4
        //    worksheet.Cell(row, column).Value = value;
        //}
        public void SetValueInWorksheet<T>(IXLWorksheet worksheet, int row, int column, Cell<T> cell)
        {
            if(cell == null)
            {
                return;
            }
            worksheet.Cell(row, column).Style.NumberFormat.Format = "@"; //format text för att klara när klassen har punkt och nollor tex 1.40 det får inte ändras till 1,4
            if (cell.FontStyle == ExcelFontStyle.Bold)
            {
                worksheet.Cell(row, column).Style.Font.Bold = true;
            }
            worksheet.Cell(row, column).Value = cell.CellValue;
        }

        public IXLCell SetValueInWorksheet(IXLWorksheet worksheet, string cellName, string value)
        {
            //var linkTocell = worksheet.NamedRange(cellName);

            //var g = GetNamedCell(worksheet, cellName);
            var currentCell = GetNamedCell(worksheet, cellName);
            if (currentCell == null)
                return null;

            currentCell.Value = value;

            return currentCell;
            //          worksheet.Workbook.Range(cellName).Cells();
            //= value;

        }

        public IXLCell GetNamedCell(IXLWorksheet worksheet, string cellName)
        {
            var linkTocell = worksheet.NamedRange(cellName);
            if (linkTocell == null)
                return null;

            //var g = GetNamedCell(worksheet, cellName);
            var currentCell = worksheet.Cell(linkTocell.RefersTo.Split('!')[1]);

            return currentCell;
            //IXLNamedRange xlNamedRange = worksheet.NamedRange(namedCell);
            //if (xlNamedRange == null)
            //    return (IXLCell)null;
            //IXLRange xlRange = xlNamedRange.Ranges.FirstOrDefault<IXLRange>();
            //if (xlRange == null)
            //    return (IXLCell)null;
            //return xlRange.FirstCell();
        }

        public void SetAutoRowHeight(string worksheetName)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            worksheet.Rows().AdjustToContents();
        }

        public void ConvertToNumber(string worksheetName, string columnName, string format)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            var column = worksheet.Column(columnName);
            worksheet.Column(columnName).Style.NumberFormat.Format = format;

            foreach (var cell in column.CellsUsed(c => c.Address.RowNumber > 1)) // Skip the header in D1
            {
                double result;
                if (double.TryParse(cell.GetValue<string>(), out result))
                {
                    cell.Value = result; // Assign the parsed number
                    cell.SetDataType(XLDataType.Number); // Set the data type to number
                }
            }
        }
    }
}