using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class Horse
    {
        public int HorseId { get; set; }
        public string HorseName { get; set; }

        [Index]
        public int HorseTdbId { get; set; }

        public virtual Lunger Lunger { get; set; }


        //public List<Vaulter> Vaulters { get; set; }
    }
}