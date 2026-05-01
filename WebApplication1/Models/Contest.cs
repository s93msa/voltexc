using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltigeCore.Models
{
    public class Contest
    {
        public int ContestId { get; set; }
        [Column("TypeOfContest_ContestTypeId")]
        public int? TypeOfContestContestTypeId { get; set; }
        public virtual ContestType TypeOfContest { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public virtual List<StartListClassStep> StartListClassStep { get; set; }

        public List<StartListClassStep> GetActiveStartListClassStep()
        {
            return StartListClassStep.FindAll(x => x.StartOrder > 0);
        }
    }
}
