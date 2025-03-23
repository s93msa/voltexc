using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Business.Logic.Excel
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