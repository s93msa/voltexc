using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public sealed class CompetitionClassesViewModel
    {
        public CompetitionClassesViewModel()
        {
            CompetitionClassesInformation = new List<CompetitionClassViewModel>();
        }
        public List<CompetitionClassViewModel> CompetitionClassesInformation { get; set; }
    }

    public class CompetitionClassViewModel
    {
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string NumberOfJudges { get; set; }
        public string Moment1 { get; set; }
        public string Moment2 { get; set; }
        public string Moment3 { get; set; }
        public string Moment4 { get; set; }

        public string Moment1Header { get; set; }
        public string Moment2Header { get; set; }
        public string Moment3Header { get; set; }
        public string Moment4Header { get; set; }

        public string JudgesMoment1 { get; set; }
        public string JudgesMoment2 { get; set; }
        public string JudgesMoment3 { get; set; }
        public string JudgesMoment4 { get; set; }


    }
}