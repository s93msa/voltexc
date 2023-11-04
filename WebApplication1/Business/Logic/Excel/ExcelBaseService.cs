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

        public void SetValuesInWorkSheet<T>(string worksheetName, int startRow, Row<T>[] rows)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);

            int rowIndex = startRow;
            foreach (var row in rows)
            {
                SetRowValuesInWorksheet(worksheet, rowIndex, 1, row.RowValues);
                rowIndex++;
            }
        }

        public void SetRowValuesInWorksheet<T>(IXLWorksheet worksheet, int row, int startColumn, T[] rowValues)
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
        public void SetValueInWorksheet<T>(IXLWorksheet worksheet, int row, int column, T value)
        {
            worksheet.Cell(row, column).Style.NumberFormat.Format = "@"; //format text för att klara när klassen har punkt och nollor tex 1.40 det får inte ändras till 1,4
            worksheet.Cell(row, column).Value = value;
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
    }
}