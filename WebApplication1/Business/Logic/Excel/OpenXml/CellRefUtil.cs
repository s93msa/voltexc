using System.Text;

namespace VoltigeCore.Business.Logic.Excel.OpenXml
{
    public static class CellRefUtil
    {
        public static bool TryParse(string reference, out uint column, out uint row)
        {
            column = 0;
            row = 0;
            if (string.IsNullOrEmpty(reference)) return false;

            int i = 0;
            uint c = 0;
            while (i < reference.Length && char.IsLetter(reference[i]))
            {
                c = c * 26u + (uint)(char.ToUpperInvariant(reference[i]) - 'A' + 1);
                i++;
            }
            if (i == 0 || i >= reference.Length) return false;
            if (!uint.TryParse(reference.AsSpan(i), out var r)) return false;

            column = c;
            row = r;
            return true;
        }

        public static string Format(uint column, uint row) => ColumnName(column) + row.ToString();

        public static string ColumnName(uint column)
        {
            var sb = new StringBuilder();
            uint c = column;
            while (c > 0)
            {
                uint rem = (c - 1) % 26u;
                sb.Insert(0, (char)('A' + rem));
                c = (c - 1) / 26u;
            }
            return sb.ToString();
        }

        public static bool TryParseColumn(string? reference, out uint column)
        {
            column = 0;
            if (string.IsNullOrEmpty(reference)) return false;
            uint c = 0;
            int i = 0;
            while (i < reference.Length && char.IsLetter(reference[i]))
            {
                c = c * 26u + (uint)(char.ToUpperInvariant(reference[i]) - 'A' + 1);
                i++;
            }
            if (i == 0) return false;
            column = c;
            return true;
        }
    }
}
