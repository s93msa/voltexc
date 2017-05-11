using System.Collections.Generic;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{

    public class Contest
    {
        public int ContestId { get; set; }
        public string Location{ get; set; }
        public string Country { get; set; }
        public virtual List<StartListClass> StartListClass { get; set; }

    }
}