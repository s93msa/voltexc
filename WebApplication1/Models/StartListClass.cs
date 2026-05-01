using System.Collections.Generic;

namespace VoltigeCore.Models
{
    public class StartListClass
    {
        public int StartListClassId { get; set; }
        public int StartOrder { get; set; }
        public string Name { get; set; }
        public virtual List<StartListClassStep> StartListClassStep { get; set; }
    }
}
