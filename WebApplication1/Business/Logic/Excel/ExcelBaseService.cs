using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
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


        public void ShowOnlyWorksheetOpenXml(WorkbookPart workbookPart, Worksheet worksheetToShow)
        {
            if (workbookPart == null || worksheetToShow == null) return;

            // Find the WorksheetPart that contains this Worksheet instance
            var worksheetPart = workbookPart.WorksheetParts.FirstOrDefault(wp => wp.Worksheet == worksheetToShow);
            if (worksheetPart == null) return;

            // Relationship id used on the Sheet element
            string relId = workbookPart.GetIdOfPart(worksheetPart);

            // Collect sheets and determine index of the sheet to show
            var sheets = workbookPart.Workbook.Sheets.Cast<Sheet>().ToList();
            int showIndex = -1;
            for (int i = 0; i < sheets.Count; i++)
            {
                var s = sheets[i];
                if (s.Id != null && s.Id.Value == relId)
                {
                    // Make target sheet visible
                    s.State = new EnumValue<SheetStateValues>(SheetStateValues.Visible);
                    showIndex = i;
                }
                else
                {
                    // Hide non-target sheets
                    s.State = new EnumValue<SheetStateValues>(SheetStateValues.Hidden);
                }
            }

            // Ensure BookViews / WorkbookView exists and set ActiveTab to the target sheet index (zero-based)
            var bookViews = workbookPart.Workbook.GetFirstChild<BookViews>() ?? workbookPart.Workbook.AppendChild(new BookViews());
            var workbookView = bookViews.Elements<WorkbookView>().FirstOrDefault();
            //if (workbookView == null)
            //{
            //    workbookView = new WorkbookView();
            //    bookViews.Append(workbookView);
            //}

            if (showIndex >= 0)
            {
                workbookView.ActiveTab = new UInt32Value((uint)showIndex);
            }

            //workbookPart.Workbook.Save();
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

        public void SetTimeConstants(string worksheetName)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            worksheet.Cell("l1").Value = "Häst";
            worksheet.Cell(StartlistExportService.TEAMCOMPULSORY_CELL).Value = 7.5;
        }
        public void ConvertTextToFormula(string worksheetName, string column, int endRow)
        {
            var worksheet = _workbook.Worksheets.Worksheet(worksheetName);
            for (int row = 1; row<= endRow; row++)
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
            worksheet.Cell(row, column).Value = XLCellValue.FromObject(cell.CellValue);
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

        public string  GetCellRef(WorkbookPart workbookPart, string worksheetName, string cellName)
        {
            var definedNames = workbookPart.Workbook.DefinedNames.
                Elements<DocumentFormat.OpenXml.Spreadsheet.DefinedName>().Where(n => n.Name.Value == cellName);

            var definedName = definedNames.FirstOrDefault(d => d.InnerText.Split('!')[0].Trim('\'') == worksheetName);

            string fullRef = definedName.Text;
            return fullRef.Split('!')[1].Replace("$", "");
        }

        public void SetValueInWorksheetOpenXML(Worksheet worksheet, string cellRef, string value)
        {


            // Parse cell reference (e.g., "A1" -> row 1, column 1)
            Cell cell = GetOrCreateCell(worksheet, cellRef);

                if (cell != null)
                {
                    // Clear any existing cell data
                    cell.CellValue = new CellValue(value);
                    cell.DataType = new EnumValue<CellValues>(CellValues.String);
                }
        }


        /// <summary>
        /// Gets or creates a cell in the worksheet at the specified address.
        /// </summary>
        private Cell GetOrCreateCell(Worksheet worksheet, string cellAddress)
        {
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            if (sheetData == null)
                return null;

            // Parse cell address (e.g., "A1")
            uint rowNum = uint.Parse(new string(cellAddress.Where(char.IsDigit).ToArray()));
            string colLetter = new string(cellAddress.Where(char.IsLetter).ToArray());
            uint colNum = ConvertColumnLetterToNumber(colLetter);

            // Get or create the row
            Row row = sheetData.Elements<Row>()
                .FirstOrDefault(r => r.RowIndex == rowNum);

            if (row == null)
            {
                row = new Row { RowIndex = rowNum };
                sheetData.AppendChild(row);
            }

            // Get or create the cell
            Cell cell = row.Elements<Cell>()
                .FirstOrDefault(c => c.CellReference == cellAddress);

            if (cell == null)
            {
                cell = new Cell { CellReference = cellAddress };
                row.AppendChild(cell);
            }

            return cell;
        }

        /// <summary>
        /// Converts column letter (A, B, AA, etc.) to numeric index.
        /// </summary>
        private uint ConvertColumnLetterToNumber(string columnLetter)
        {
            uint columnNumber = 0;
            foreach (char c in columnLetter.ToUpper())
            {
                columnNumber = columnNumber * 26 + (uint)(c - 'A' + 1);
            }
            return columnNumber;
        }

        public void HideColumnOpenXml(WorkbookPart workbookPart, Worksheet worksheet, string cellAddress)
        {
            if (workbookPart == null || worksheet == null || string.IsNullOrWhiteSpace(cellAddress))
                return;

            // Extract column letters from address (e.g. "A1" -> "A")
            string colLetters = new string(cellAddress.Where(char.IsLetter).ToArray()).Replace("$", "");
            if (string.IsNullOrEmpty(colLetters))
                return;

            uint colIndex = ConvertColumnLetterToNumber(colLetters);

            // Ensure Columns element exists (insert before SheetData)
            Columns columns = worksheet.GetFirstChild<Columns>();
            if (columns == null)
            {
                columns = new Columns();
                var sheetData = worksheet.GetFirstChild<SheetData>();
                if (sheetData != null)
                    worksheet.InsertBefore(columns, sheetData);
                else
                    worksheet.Append(columns);
            }

            // Try to find an existing Column element that covers this column
            var existing = columns.Elements<Column>()
                .FirstOrDefault(c => c.Min != null && c.Max != null && c.Min.Value <= colIndex && c.Max.Value >= colIndex);

            if (existing != null && existing.Min.Value == colIndex && existing.Max.Value == colIndex)
            {
                // Exact single-column definition found: mark hidden
                existing.Hidden = new BooleanValue(true);
            }
            else
            {
                // Append a single-column definition to hide only this column.
                // If an existing range covers it, adding a single-column Column element will override that column.
                var newCol = new Column()
                {
                    Min = new UInt32Value(colIndex),
                    Max = new UInt32Value(colIndex),
                    Hidden = new BooleanValue(true),
                    CustomWidth = new BooleanValue(true),
                    Width = new DoubleValue(0.0)
                };

                // Place new column element. Keeping document order simple: append.
                columns.Append(newCol);
            }
        }

        //private uint ConvertColumnLetterToNumber(string columnLetter)
        //{
        //    if (string.IsNullOrWhiteSpace(columnLetter))
        //        throw new ArgumentException("columnLetter is null or empty", nameof(columnLetter));

        //    // Keep only letters and normalize
        //    var letters = new string(columnLetter.Where(char.IsLetter).ToArray()).ToUpper();
        //    if (letters.Length == 0)
        //        throw new ArgumentException("columnLetter contains no letters", nameof(columnLetter));

        //    uint columnNumber = 0;
        //    foreach (char c in letters)
        //    {
        //        columnNumber = columnNumber * 26 + (uint)(c - 'A' + 1);
        //    }
        //    return columnNumber;
        //}

        public IXLCell GetNamedCell(IXLWorksheet worksheet, string cellName)
        {
            var linkTocell = worksheet.DefinedNames.FirstOrDefault( x => x.Name == cellName);
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

            foreach (var cell in column.CellsUsed(c => c.Address.RowNumber > 1)) // // Skip header row 1
            {
                var text = cell.GetValue<string>()?.Trim();
                if (string.IsNullOrEmpty(text)) continue;
                // Remove Excel-leading apostrophe if present
                if (text.Length > 0 && text[0] == '\'') text = text.Substring(1);

                double value;
                if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out value)
               || double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    {
                    cell.SetValue(value);
                }
            }
        }
    }
}