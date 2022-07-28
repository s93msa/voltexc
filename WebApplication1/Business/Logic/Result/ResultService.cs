using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Business.Logic.Excel.Results;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Result
{
    public class ResultService
    {
        public List<Participant> GetParticipants()
        {
            var excelPartitionsList = new List<Participant>();

            var contest = ContestService.GetContestInstance();
            foreach (var startListClassStep in contest.StartListClassStep.OrderBy(x => x.StartOrder))
            {
                foreach (var startListItem in startListClassStep.GetActiveStartList().OrderBy(x => x.StartNumber))
                {
                    var horseName = startListItem.HorseInformation?.HorseName;
                    var horseId = startListItem.HorseInformation?.HorseId ?? 0;
                    var lungerName = startListItem.HorseInformation?.Lunger?.LungerName;

                    if (startListItem.IsTeam)
                    {
                        var testnumber = startListItem.TeamTestnumber;
                        if (testnumber > 1) continue;
                        var vaultingClass = startListItem.VaultingTeam.VaultingClass;
                        var vaultingClassNr = vaultingClass?.ClassNr.ToString();
                        var teamName = startListItem.VaultingTeam.Name;
                        var clubName = startListItem.VaultingTeam.VaultingClub.ClubName;
                        clubName += " (" + startListItem.VaultingTeam.VaultingClub.Country + ")";
                        var teamId = ContestService.GetTeamExcelId(startListItem.VaultingTeam, horseId);
                        var excelParticipant = new Participant()
                        {
                            classNo = vaultingClassNr,
                            participantsName = teamName,
                            lungerName = lungerName,
                            clubName = clubName,
                            horseName = horseName,
                            participantsId = teamId
                        };
                        excelPartitionsList.Add(excelParticipant);
                    }
                    else
                    {
                        foreach (var participant in startListItem.GetActiveVaulters().OrderBy(x => x.StartOrder))
                        {
                            var testnumber = participant.Testnumber;
                            if (testnumber > 1) continue;

                            var vaultingClass = participant.Participant.VaultingClass;
                            var vaultingClassNr = vaultingClass?.ClassNr;
                            var vaulterName = participant.Participant.Name;
                            var clubName = participant.Participant.VaultingClub?.ClubName;
                            clubName += " (" + participant.Participant.VaultingClub?.Country + ")";

                            string vaulterId = ContestService.GetVaulterExcelId(participant.Participant, horseId);
                            var excelParticipant = new Participant()
                            {
                                classNo = vaultingClassNr,
                                participantsName = vaulterName,
                                lungerName = lungerName,
                                clubName = clubName,
                                horseName = horseName,
                                participantsId = vaulterId
                            };
                            excelPartitionsList.Add(excelParticipant);
                        }
                    }
                }

            }

            return excelPartitionsList;
        }

        public List<Excel.Results.CompetitionClass> GetClasses()
        {
            var judges = ContestService.GetJudgesPerStep();

            var excelCompetitionClasses = new List<Excel.Results.CompetitionClass>();
            using (var db = new VaultingContext())
            {
                var contest = ContestService.GetContestInstance();
                var competitionClasses = db.CompetitionClasses.OrderBy(x => x.ClassNr);
                var startListClassSteps = db.StartListClassSteps.ToDictionary(x => x.StartListClassStepId, x => x);
                var classesList = ContestService.GetAllClassesWithAtleastOneParticipant(db);

                foreach (var competitionClass in competitionClasses.ToList())
                {

                    if (competitionClass.ClassNr == "0")
                        continue;
                    if (competitionClass.GetCompetitionSteps(contest.TypeOfContest).Count == 0)
                        continue;
                    if (!classesList.Contains(competitionClass.CompetitionClassId))
                    { continue; }

                    var classNumber = competitionClass.ClassNr;
                    var className = competitionClass.ClassName;
                    var numberOfJudges = "0";
                    var steps = new string[4];
                    var momentText = new string[4];
                    var judgesString1 = GetJudgesString(judges, startListClassSteps, classNumber, 1);
                    var judgesString2 = GetJudgesString(judges, startListClassSteps, classNumber, 2);
                    var judgesString3 = GetJudgesString(judges, startListClassSteps, classNumber, 3);
                    var judgesString4 = GetJudgesString(judges, startListClassSteps, classNumber, 4);

                    var stepsList = GetStepsForThisTypeOfCompetition(competitionClass);

                    foreach (var step in stepsList)
                    {

                        if (step.TestNumber < 1) continue;

                        steps[step.TestNumber - 1] = step.Name;
                        momentText[step.TestNumber - 1] = step.ResultMomentText;

                    }

                    var excelCompetitionClass = new Business.Logic.Excel.Results.CompetitionClass()
                    {
                        ClassNumber = classNumber,
                        ClassName = className,
                        NumberOfJudges = numberOfJudges,
                        Moment1 = steps[0],
                        Moment2 = steps[1],
                        Moment3 = steps[2],
                        Moment4 = steps[3],
                        Moment1Header = momentText[0],
                        Moment2Header = momentText[1],
                        Moment3Header = momentText[2],
                        Moment4Header = momentText[3],
                        JudgesMoment1 = judgesString1,
                        JudgesMoment2 = judgesString2,
                        JudgesMoment3 = judgesString3,
                        JudgesMoment4 = judgesString4
                    };
                    excelCompetitionClasses.Add(excelCompetitionClass);                    
                }

            }

            return excelCompetitionClasses;
        }

        private static string GetJudgesString(Dictionary<string, int> judges, Dictionary<int, StartListClassStep> startListClassSteps, string classNumber, int testNumber)
        {
            string judgesString = GetJudgeName(judges, classNumber, startListClassSteps, JudgeTableNames.A, testNumber);
            judgesString += GetJudgeName(judges, classNumber, startListClassSteps, JudgeTableNames.B, testNumber);
            judgesString += GetJudgeName(judges, classNumber, startListClassSteps, JudgeTableNames.C, testNumber);
            judgesString += GetJudgeName(judges, classNumber, startListClassSteps, JudgeTableNames.D, testNumber);
            return judgesString.TrimStart(',').TrimStart();
        }

        private static string GetJudgeName(Dictionary<string, int> stepsrelation, string classNumber, Dictionary<int, StartListClassStep> startListClassSteps,
           JudgeTableNames judgeTableName, int testNumber)
        {

            int startliststep;
            StartListClassStep startListClassStep = null;
            if (stepsrelation.TryGetValue(classNumber + "_" + testNumber, out startliststep))
                startListClassSteps.TryGetValue(startliststep, out startListClassStep);
            var judgeName = startListClassStep?.GetJudgeName(judgeTableName);
            if (string.IsNullOrWhiteSpace(judgeName))
                return "";

            return ", " + judgeName;
        }

        private static List<Step> GetStepsForThisTypeOfCompetition(Models.CompetitionClass competitionClass)
        {
            var contest = ContestService.GetContestInstance();
            return competitionClass.GetCompetitionSteps(contest.TypeOfContest);
        }
    }
}