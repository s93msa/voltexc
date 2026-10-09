using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace VoltigeCore.Business.Logic.Excel.OpenXml
{
    public sealed class ScorecardWriter : IDisposable
    {
        private readonly SpreadsheetDocument _doc;
        private readonly WorkbookPart _workbookPart;
        private readonly Dictionary<string, WorksheetPart> _sheetsByName;
        private readonly Dictionary<string, uint> _sheetIndexByName;
        private string? _activeSheet;

        public ScorecardWriter(string outputPath, byte[] templateBytes)
        {
            var dir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            File.WriteAllBytes(outputPath, templateBytes);

            _doc = SpreadsheetDocument.Open(outputPath, true);
            _workbookPart = _doc.WorkbookPart
                ?? throw new InvalidOperationException("Template has no WorkbookPart");

            _sheetsByName = new Dictionary<string, WorksheetPart>(StringComparer.Ordinal);
            _sheetIndexByName = new Dictionary<string, uint>(StringComparer.Ordinal);
            uint i = 0;
            foreach (var sheet in _workbookPart.Workbook.Sheets!.Elements<Sheet>())
            {
                if (sheet.Id?.Value != null && sheet.Name?.Value != null)
                {
                    var part = (WorksheetPart)_workbookPart.GetPartById(sheet.Id.Value);
                    _sheetsByName[sheet.Name.Value] = part;
                    _sheetIndexByName[sheet.Name.Value] = i;
                }
                i++;
            }
        }

        public bool SheetExists(string sheetName) => _sheetsByName.ContainsKey(sheetName);

        public void SetActiveSheet(string sheetName) => _activeSheet = sheetName;

        public bool TryResolveDefinedName(string definedName, out CellAddress address)
        {
            address = default;
            var definedNames = _workbookPart.Workbook.DefinedNames;
            if (definedNames == null) return false;

            uint? activeIndex = _activeSheet != null && _sheetIndexByName.TryGetValue(_activeSheet, out var idx)
                ? idx : (uint?)null;

            DefinedName? workbookScoped = null;
            foreach (var dn in definedNames.Elements<DefinedName>())
            {
                if (!string.Equals(dn.Name?.Value, definedName, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (dn.LocalSheetId == null)
                {
                    workbookScoped = dn;
                    continue;
                }

                if (activeIndex.HasValue && dn.LocalSheetId.Value == activeIndex.Value)
                    return TryParseDefinedNameRef(dn.Text ?? "", out address);
            }

            if (workbookScoped != null)
                return TryParseDefinedNameRef(workbookScoped.Text ?? "", out address);

            return false;
        }

        private static bool TryParseDefinedNameRef(string refText, out CellAddress address)
        {
            address = default;
            if (string.IsNullOrEmpty(refText)) return false;

            var bangIdx = refText.LastIndexOf('!');
            if (bangIdx < 0) return false;

            var sheetPart = refText.Substring(0, bangIdx);
            var cellPart = refText.Substring(bangIdx + 1).Replace("$", "");

            if (sheetPart.Length >= 2 && sheetPart[0] == '\'' && sheetPart[sheetPart.Length - 1] == '\'')
                sheetPart = sheetPart.Substring(1, sheetPart.Length - 2).Replace("''", "'");

            if (!CellRefUtil.TryParse(cellPart, out var col, out var row)) return false;

            address = new CellAddress(sheetPart, col, row);
            return true;
        }

        public string GetCellString(CellAddress addr)
        {
            if (!_sheetsByName.TryGetValue(addr.SheetName, out var wsPart)) return "";
            var sheetData = wsPart.Worksheet.GetFirstChild<SheetData>();
            if (sheetData == null) return "";

            var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex?.Value == addr.Row);
            if (row == null) return "";

            var cell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference?.Value == addr.Reference);
            return cell == null ? "" : ResolveCellText(cell);
        }

        private string ResolveCellText(Cell cell)
        {
            var t = cell.DataType?.Value;
            if (t == CellValues.SharedString)
            {
                if (!int.TryParse(cell.CellValue?.Text, out var idx)) return "";
                var sst = _workbookPart.SharedStringTablePart?.SharedStringTable;
                var item = sst?.Elements<SharedStringItem>().ElementAtOrDefault(idx);
                return item?.InnerText ?? "";
            }
            if (t == CellValues.InlineString)
                return cell.InlineString?.InnerText ?? "";
            return cell.CellValue?.Text ?? "";
        }

        public void SetCellString(CellAddress addr, string? value)
        {
            var cell = GetOrCreateCell(addr);
            var styleIndex = cell.StyleIndex;
            cell.RemoveAllChildren();
            cell.DataType = CellValues.InlineString;
            if (styleIndex != null) cell.StyleIndex = styleIndex;
            cell.AppendChild(new InlineString(
                new Text(value ?? "") { Space = SpaceProcessingModeValues.Preserve }));
        }

        public void SetCellNumber(CellAddress addr, double value)
        {
            var cell = GetOrCreateCell(addr);
            var styleIndex = cell.StyleIndex;
            cell.RemoveAllChildren();
            cell.DataType = CellValues.Number;
            if (styleIndex != null) cell.StyleIndex = styleIndex;
            cell.AppendChild(new CellValue(value.ToString("R", CultureInfo.InvariantCulture)));
        }

        public void HideColumn(string sheetName, uint columnIndex)
        {
            if (!_sheetsByName.TryGetValue(sheetName, out var wsPart)) return;
            var worksheet = wsPart.Worksheet;

            var cols = worksheet.GetFirstChild<Columns>();
            if (cols == null)
            {
                cols = new Columns();
                var sheetData = worksheet.GetFirstChild<SheetData>();
                if (sheetData != null) worksheet.InsertBefore(cols, sheetData);
                else worksheet.AppendChild(cols);
            }

            Column? containing = null;
            foreach (var c in cols.Elements<Column>())
            {
                if (c.Min?.Value <= columnIndex && c.Max?.Value >= columnIndex)
                {
                    containing = c;
                    break;
                }
            }

            if (containing == null)
            {
                cols.AppendChild(new Column
                {
                    Min = columnIndex,
                    Max = columnIndex,
                    Hidden = true,
                    CustomWidth = true,
                    Width = 0d
                });
                return;
            }

            uint min = containing.Min!.Value;
            uint max = containing.Max!.Value;

            if (min == columnIndex && max == columnIndex)
            {
                containing.Hidden = true;
                return;
            }

            if (min < columnIndex)
            {
                var leftCol = (Column)containing.CloneNode(true);
                leftCol.Min = min;
                leftCol.Max = columnIndex - 1;
                cols.InsertBefore(leftCol, containing);
            }

            if (max > columnIndex)
            {
                var rightCol = (Column)containing.CloneNode(true);
                rightCol.Min = columnIndex + 1;
                rightCol.Max = max;
                cols.InsertBefore(rightCol, containing);
            }

            containing.Min = columnIndex;
            containing.Max = columnIndex;
            containing.Hidden = true;
        }

        public void ShowOnlySheet(string sheetName)
        {
            var sheets = _workbookPart.Workbook.Sheets!;
            uint activeIndex = 0;
            uint i = 0;
            bool found = false;

            foreach (var sheet in sheets.Elements<Sheet>())
            {
                if (string.Equals(sheet.Name?.Value, sheetName, StringComparison.Ordinal))
                {
                    sheet.State = null;
                    activeIndex = i;
                    found = true;
                }
                else
                {
                    sheet.State = SheetStateValues.Hidden;
                }
                i++;
            }
            if (!found) return;

            var bookViews = _workbookPart.Workbook.BookViews;
            if (bookViews == null)
            {
                bookViews = new BookViews(new WorkbookView { ActiveTab = activeIndex });
                _workbookPart.Workbook.InsertBefore(bookViews, _workbookPart.Workbook.Sheets);
            }
            else
            {
                var view = bookViews.GetFirstChild<WorkbookView>();
                if (view == null) bookViews.AppendChild(new WorkbookView { ActiveTab = activeIndex });
                else view.ActiveTab = activeIndex;
            }

            foreach (var kvp in _sheetsByName)
            {
                bool isActive = string.Equals(kvp.Key, sheetName, StringComparison.Ordinal);
                var sheetViews = kvp.Value.Worksheet.GetFirstChild<SheetViews>();
                if (sheetViews == null) continue;
                foreach (var view in sheetViews.Elements<SheetView>())
                    view.TabSelected = isActive ? true : null;
            }
        }

        public void SetCellBackgroundColor(CellAddress addr, string argbHex)
        {
            var cell = GetOrCreateCell(addr);
            uint baseStyleIndex = cell.StyleIndex?.Value ?? 0;
            cell.StyleIndex = CloneStyleWithFill(baseStyleIndex, argbHex);
        }

        private uint CloneStyleWithFill(uint baseStyleIndex, string argbHex)
        {
            var stylesPart = _workbookPart.WorkbookStylesPart
                ?? throw new InvalidOperationException("Template has no styles part");
            var stylesheet = stylesPart.Stylesheet;

            var fills = stylesheet.Fills
                ?? throw new InvalidOperationException("Template stylesheet has no fills");
            uint fillId = GetOrAddSolidFill(fills, argbHex);

            var cellXfs = stylesheet.CellFormats
                ?? throw new InvalidOperationException("Template stylesheet has no cellXfs");
            var formats = cellXfs.Elements<CellFormat>().ToList();
            var baseFormat = baseStyleIndex < (uint)formats.Count
                ? formats[(int)baseStyleIndex]
                : new CellFormat();

            var newFormat = (CellFormat)baseFormat.CloneNode(true);
            newFormat.FillId = fillId;
            newFormat.ApplyFill = true;
            cellXfs.AppendChild(newFormat);

            uint newIndex = (uint)(cellXfs.Elements<CellFormat>().Count() - 1);
            if (cellXfs.Count != null) cellXfs.Count = (uint)(newIndex + 1);
            return newIndex;
        }

        private static uint GetOrAddSolidFill(Fills fills, string argbHex)
        {
            uint idx = 0;
            foreach (var fill in fills.Elements<Fill>())
            {
                var pf = fill.PatternFill;
                if (pf?.PatternType?.Value == PatternValues.Solid
                    && string.Equals(pf.ForegroundColor?.Rgb?.Value, argbHex, StringComparison.OrdinalIgnoreCase))
                {
                    return idx;
                }
                idx++;
            }

            var newFill = new Fill(new PatternFill(
                new ForegroundColor { Rgb = argbHex },
                new BackgroundColor { Indexed = 64u })
            { PatternType = PatternValues.Solid });
            fills.AppendChild(newFill);
            if (fills.Count != null) fills.Count = (uint)fills.Elements<Fill>().Count();
            return (uint)(fills.Elements<Fill>().Count() - 1);
        }

        private Cell GetOrCreateCell(CellAddress addr)
        {
            if (!_sheetsByName.TryGetValue(addr.SheetName, out var wsPart))
                throw new InvalidOperationException($"Sheet '{addr.SheetName}' not found");

            var sheetData = wsPart.Worksheet.GetFirstChild<SheetData>()
                ?? wsPart.Worksheet.AppendChild(new SheetData());

            var rowEl = GetOrCreateRow(sheetData, addr.Row);
            return GetOrCreateCellInRow(rowEl, addr.Reference, addr.Column);
        }

        private static Row GetOrCreateRow(SheetData sheetData, uint rowIndex)
        {
            Row? insertBefore = null;
            foreach (var r in sheetData.Elements<Row>())
            {
                if (r.RowIndex?.Value == rowIndex) return r;
                if (r.RowIndex?.Value > rowIndex) { insertBefore = r; break; }
            }
            var newRow = new Row { RowIndex = rowIndex };
            if (insertBefore != null) sheetData.InsertBefore(newRow, insertBefore);
            else sheetData.AppendChild(newRow);
            return newRow;
        }

        private static Cell GetOrCreateCellInRow(Row row, string cellRef, uint column)
        {
            Cell? insertBefore = null;
            foreach (var c in row.Elements<Cell>())
            {
                if (c.CellReference?.Value == cellRef) return c;
                if (CellRefUtil.TryParseColumn(c.CellReference?.Value, out var existingCol)
                    && existingCol > column)
                {
                    insertBefore = c;
                    break;
                }
            }
            var newCell = new Cell { CellReference = cellRef };
            if (insertBefore != null) row.InsertBefore(newCell, insertBefore);
            else row.AppendChild(newCell);
            return newCell;
        }

        public void Dispose() => _doc.Dispose();
    }
}
