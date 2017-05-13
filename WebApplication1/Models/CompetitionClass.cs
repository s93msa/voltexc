using System.Collections.Generic;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class CompetitionClass
    {
        public int CompetitionClassId { get; set; }
        public int ClassNr { get; set; }
        public string ClassName { get; set; }

        public string Excelfile { get; set; }

        public virtual List<Step> Steps { get; set; }
       
        //public CompetitionClass(int classNr, string className, List<Step> steps)
        //{
        //    ClassNr = classNr;
        //    ClassName = className;
        //    Steps = steps;
        //}

    }
}