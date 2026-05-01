using System.Collections.Generic;

namespace VoltigeCore.Business.Logic.Excel
{
    public struct Row<T>
    {
        public ICollection<T> RowValues;

        public Row(ICollection<T> rowValues)
        {
            RowValues = rowValues;
        }
    }
}
