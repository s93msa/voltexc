using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Business.Logic.Excel
{
    public struct Row<T>
    {
        public T[] RowValues;

        public Row(T[] rowValues)
        {
            RowValues = rowValues;
        }
    }
}   