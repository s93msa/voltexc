using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Migrations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class EditVaulterHorseViewModel
    {
        public int StartListClassStepId { get; set; }

        public SelectList StartListClassStepsSelectList { get; set; }

        public int HorseOrderId { get; set; }

        public SelectList HorseOrderSelectList { get; set; }

    }
}