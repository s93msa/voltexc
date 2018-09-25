using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class HorseOrderViewModel 
    {
        public HorseOrderViewModel()
        {
        }

        public HorseOrder HorseOrder { get; set; }

        public SelectList HorsesLungersSelectList { get; set; }

        public SelectList StepsSelectList { get; set; }

        public SelectList StartListClassStepsSelectList { get; set; }

        public SelectList TeamsSelectList { get; set; }
    }
}