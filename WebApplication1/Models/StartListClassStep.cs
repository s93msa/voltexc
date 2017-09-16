using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class StartListClassStep
    {
        public int StartListClassStepId { get; set; }

        public int StartOrder { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public virtual List<JudgeTable> JudgeTables { get; set; }

        public virtual List<HorseOrder> StartList { get; set; }

        //public virtual List<Step> IncludedSteps { get; set; }

        public string GetJudgeName(JudgeTableNames judgeTableName)
        {
            var selectedJudgeTable =  JudgeTables?.FirstOrDefault(judgeTable => judgeTable.JudgeTableName == judgeTableName);
            
            return selectedJudgeTable?.JudgeName;            
        }
        //public bool IsTeam { get; set; }

        //public virtual SortedList<int, Vaulter> Vaulters { get; set; }
        //public virtual SortedList<int, Team> Team { get; set; }

        //public List<Step> Step { get; set; }
    }
}