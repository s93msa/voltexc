using System;
using System.Collections.Generic;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class StartListClassStep
    {
        public int StartListClassStepId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<JudgeTable> JudgeTables { get; set; }

        public bool IsTeam { get; set; }

        public SortedList<int, Vaulter> Vaulters { get; set; }
        public SortedList<int, Team> Team { get; set; }

        public List<Step> Step { get; set; }
    }
}