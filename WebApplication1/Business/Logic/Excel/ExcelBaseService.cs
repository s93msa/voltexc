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
        public void SetFormulaInWorkSheet(string worksheetName, int endRow)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            worksheet.Column(1).InsertColumnsBefore(1);
            
            worksheet.Cell("A1").Value = new DateTime(2023, 9, 10, 9, 0, 0); ;
            worksheet.Column("A").Style.DateFormat.Format = "HH:mm";
            //= OM(D1 = 0; ""; B1 + KLOCKSLAG(0; 0; PRODUKTSUMMA(D1: D1) * 60))

            for (var currentRow=2; currentRow <= endRow; currentRow++)
            {
                worksheet.Cell(currentRow, 1).FormulaA1 = "IF(B" + (currentRow) + "=0,\"\", A1+TIME(0,0,SUMPRODUCT(VALUE(B1:B" + (currentRow-1) + "))*60))";
            }
        }

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

        public void ConvertToNumber(string worksheetName, string columnName)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            var column = worksheet.Column(columnName);
            worksheet.Column(columnName).Style.NumberFormat.Format = "0.0";

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