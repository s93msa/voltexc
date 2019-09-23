using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Controllers.DTO
{
    public class StartListTdbClasses
    {
        public int[] CompetitionClassesTdbIds;

        public StepMoment[] StepMoment;
    }

    public struct StepMoment
    {
        public int StartListClassStepId;
        public int TestNumber;
    };


    
}