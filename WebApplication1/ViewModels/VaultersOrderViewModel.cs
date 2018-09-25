using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class VaultersOrderViewModel
    {

        public VaultersOrderViewModel()
        {
        }

        public VaulterOrder VaulterOrder { get; set; }

        

        public SelectList VaulterSelectList { get; set; }

    }
}