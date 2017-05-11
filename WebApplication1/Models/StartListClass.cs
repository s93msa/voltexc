using System.Collections.Generic;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class StartListClass
    {
        public int StartListClassId { get; set; }

        public string Name { get; set; }
        public List<StartListClassStep> StartListClassStep { get; set; }

        //public StartListClass(List<StartListClassStep> steps)
        //{
        //    _steps = steps;
        //}

    }
}