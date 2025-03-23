using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Controllers.DTO
{
    
    public class StepIdWithClasses
    {
        public int StartListClassStepId;

        public List<ClassesTdb> CompetitionClassesTdbIds;

        public StepIdWithClasses(int startListClassStepId, List<ClassesTdb> competitionClassesTdbIds)
        {
            StartListClassStepId = startListClassStepId;
            CompetitionClassesTdbIds = competitionClassesTdbIds;
        }
    }

    public class startListClassStep
    {
        public int startListClassId { get; set; }
        public string stepName { get; set; }
        public int startOrder { get; set; }
        public DateTime stepDate { get; set; }
    }

    public class ClassesTdb
    {
        public int ClassTdbId;
        public int testnumber;

        public ClassesTdb(int classTdbId, int testnumber)
        {
            ClassTdbId = classTdbId;
            this.testnumber = testnumber;
        }
        
    }





}