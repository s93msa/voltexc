using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class VaulterOrder
    {
        public int VaulterOrderID { get; set; }


        public int StartOrder { get; set; }
        public virtual Vaulter Participant { get; set; }
        public int Testnumber { get; set; }

        public bool IsActive { get; set; }

        public int? HorseOrderId { get; set; }
        //[ForeignKey("HorseOrderId")]
        //public HorseOrder HorseOrder { get; set; }

    }
}