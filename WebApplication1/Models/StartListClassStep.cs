using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace VoltigeCore.Models
{
    public class StartListClassStep
    {
        public int StartListClassStepId { get; set; }
        public int StartOrder { get; set; }
        [Column("Contest_ContestId")]
        public int? ContestContestId { get; set; }
        [Column("StartListClass_StartListClassId")]
        public int? StartListClassStartListClassId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public virtual List<JudgeTable> JudgeTables { get; set; }
        public virtual List<HorseOrder> StartList { get; set; }

        public List<HorseOrder> GetActiveStartList()
        {
            return StartList.FindAll(x => x.IsActive);
        }

        public string GetJudgeName(JudgeTableNames judgeTableName)
        {
            var selectedJudgeTable = JudgeTables?.FirstOrDefault(j => j.JudgeTableName == judgeTableName);
            return selectedJudgeTable?.JudgeName;
        }
    }
}
