using System.Collections.Generic;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class Horse
    {
        public int HorseId { get; set; }
        public string HorseName { get; set; }

        public Lunger Lunger { get; set; }

        public List<Vaulter> Vaulters { get; set; }
    }
}