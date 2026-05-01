using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace VoltigeCore.Business.Logic.Excel
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
            if (workbookWorksheets == null) return;

            foreach (var currrentWorksheet in workbookWorksheets)
            {
                if (currrentWorksheet == worksheet)
                {
                    currrentWorksheet.Visibility = XLWorksheetVisibility.Visible;
                    currrentWorksheet.TabActive = true;
                }
                else
                    currrentWorksheet.Hide();
            }
        }

        public void SaveExcelFile() => _workbook.Save();

        public void SaveExcelFile(string outputPathAndName)
        {
            if (outputPathAndName == null) return;
            outputPathAndName = outputPathAndName.Replace("&", "och").Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("*", string.Empty);
            _workbook.SaveAs(outputPathAndName);
        }

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

        public void SetTimeConstants(string worksheetName)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            worksheet.Cell("l1").Value = "Häst";
            worksheet.Cell(StartlistExportService.TEAMCOMPULSORY_CELL).Value = 7.5;
        }

        public void ConvertTextToFormula(string worksheetName, string column, int endRow)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            for (int row = 1; row <= endRow; row++)
            {
                var formulaAsString = worksheet.Cell(row, column).Value.ToString();
                worksheet.Cell(row, column).FormulaA1 = formulaAsString;
            }
            worksheet.Cell(StartlistExportService.TEAMCOMPULSORY_CELL).Value = 7.5;
            worksheet.Column(1).InsertColumnsBefore(1);
        }

        public void SetFormulaInWorkSheet(string worksheetName, int endRow)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            worksheet.Column(1).InsertColumnsBefore(1);
            worksheet.Column(1).InsertColumnsBefore(1);

            worksheet.Cell("A1").Value = new DateTime(2023, 9, 10, 9, 0, 0);
            worksheet.Column("A").Style.DateFormat.Format = "HH:mm";

            for (var currentRow = 2; currentRow <= endRow; currentRow++)
            {
                var lastValue = $"LOOKUP(2, 1/(A$1:A{currentRow - 1} <>\"\"), A$1:A{currentRow - 2})";
                var newTime = $@"INDIRECT(ADDRESS(MAX(ROW(A1:A{currentRow - 1})*(A1:A{currentRow - 1}<>"""")), COLUMN(C1)))";
                var formula = $"IF(C{currentRow}=0, \"\", {lastValue}+ TIME(0,0,SUMPRODUCT(VALUE({newTime}:C{currentRow - 1}))*60))";
                worksheet.Cell(currentRow, 2).FormulaA1 = formula;
            }
        }

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

        public void SetValueInWorksheet<T>(IXLWorksheet worksheet, int row, int column, Cell<T> cell)
        {
            if (cell == null) return;
            worksheet.Cell(row, column).Style.NumberFormat.Format = "@";
            if (cell.FontStyle == ExcelFontStyle.Bold)
                worksheet.Cell(row, column).Style.Font.Bold = true;
            worksheet.Cell(row, column).Value = Convert.ToString(cell.CellValue) ?? "";
        }

        public IXLCell SetValueInWorksheet(IXLWorksheet worksheet, string cellName, string value)
        {
            var currentCell = GetNamedCell(worksheet, cellName);
            if (currentCell == null) return null;
            currentCell.Value = value;
            return currentCell;
        }

        public IXLCell GetNamedCell(IXLWorksheet worksheet, string cellName)
        {
            var linkTocell = worksheet.NamedRange(cellName);
            if (linkTocell == null) return null;
            var currentCell = worksheet.Cell(linkTocell.RefersTo.Split('!')[1]);
            return currentCell;
        }

        public void SetAutoRowHeight(string worksheetName)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            worksheet.Rows().AdjustToContents();
        }

        public void ConvertToNumber(string worksheetName, string columnName, string format)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            worksheet.Column(columnName).Style.NumberFormat.Format = format;
            foreach (var cell in worksheet.Column(columnName).CellsUsed(c => c.Address.RowNumber > 1))
            {
                double result;
                if (double.TryParse(cell.GetValue<string>(), out result))
                {
                    cell.Value = result;
                }
            }
        }
    }
}
