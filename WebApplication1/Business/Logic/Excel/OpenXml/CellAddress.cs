namespace VoltigeCore.Business.Logic.Excel.OpenXml
{
    public readonly record struct CellAddress(string SheetName, uint Column, uint Row)
    {
        public string Reference => CellRefUtil.Format(Column, Row);

        public CellAddress Below(uint rows) => this with { Row = Row + rows };
        public CellAddress Above(uint rows) => this with { Row = Row - rows };
    }
}
