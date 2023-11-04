using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Controllers.DTO
{
    
    public class StepIdWithClasses
    {
        public int StartListClassStepId;

        public ClassesTdbIdDictionary CompetitionClassesTdbIds;

        public StepIdWithClasses(int startListClassStepId, ClassesTdbIdDictionary competitionClassesTdbIds)
        {
            StartListClassStepId = startListClassStepId;
            CompetitionClassesTdbIds = competitionClassesTdbIds;
        }
    }

    public class ClassesTdbIdDictionary : Dictionary<int, int>
    {
        public new void Add(int tdbId, int testNumber)
        {
            //if (base.TryGetValue(tdbId, out var existingTestNumbers))
            //{
            //    existingTestNumbers.Add(testNumber);
            //    base[tdbId] = existingTestNumbers;
            //}
            //else
            //{
                base.Add(tdbId, testNumber);
            //}
        }

        public new int this[int tdbId]
        {
            get { return base[tdbId]; }
            set { base[tdbId] = value; }
        }
    }



}