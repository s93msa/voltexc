//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using ClosedXML.Excel;
//using WebApplication1.Classes;
//using WebApplication1.Models;

using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Contest
{
    public class ContestService
    {
        private static Models.Contest _contest;

        //TODO: Ska det verkligen vara dictionaries? Borde göras om till listor istället?
        private static Dictionary<string, int> _stepsJudges = null;
        private static Dictionary<string, Lunger> _lungers = null;
        private static List<Club> _clubs = null;
        private static List<Horse> _horses = null;
        private static List<CompetitionClass> _classes;
        private static List<Vaulter> _vaulters;
        private static List<Team> _teams;
        private static List<TeamList> _teamMembers;


        public static Models.Contest GetContestInstance()
        {
            if (_contest != null)
                return _contest;

            return GetAllDataFromDataBase();
        }

        

        public static Dictionary<string,int> GetJudgesPerStep()
        {

            if (_stepsJudges != null)
                return _stepsJudges;

            _stepsJudges = new Dictionary<string, int>();

            foreach (var startListClassStep in GetContestInstance().StartListClassStep)
            {
                //var judgeTables = startListClassStep.JudgeTables;
                foreach (var carriage in startListClassStep.GetActiveStartList())
                {
                    if (carriage.IsTeam && carriage.VaultingTeam != null)
                    {
                        var vaultingClass = carriage.VaultingTeam.VaultingClass;
                        if (vaultingClass == null)
                            continue;

                        var testNumber = carriage.TeamTestnumber;
                        //var step = ExcelPreCompetitionData.GetCompetitionStep(vaultingClass, testNumber);
                        //var momentName = step?.Name;
                        string classNr = vaultingClass.ClassNr.ToString();
                        string testNumberString = testNumber.ToString();

                        AddToStepsJudgesList(classNr, testNumberString, startListClassStep);
                    }
                    else
                    {
                        foreach (var vaulter in carriage.Vaulters)
                        {
                            string classNr = vaulter.Participant.VaultingClass.ClassNr.ToString();
                            string testNumberString = vaulter.Testnumber.ToString();
                            AddToStepsJudgesList(classNr, testNumberString, startListClassStep);
                        }
                    }
                }
                
            }

            return _stepsJudges;
        }

        public static int GetContestTypeId()
        {
            int contestId;
            if (int.TryParse(ConfigurationManager.AppSettings["ContestId"], out contestId))
                return contestId;

            return 0;
        }

        public static Lunger GetLunger(string lungerName)
        {
            Lunger lunger;
            var lungers = GetLungers();
            if (lungers != null && lungers.TryGetValue(lungerName.Trim(), out lunger))
            {
                return lunger;
            }

            return null;
        }

        public static Lunger GetLunger(int lungerTdbId)
        {
            
            var lungers = GetLungers();
            var lunger = lungers.FirstOrDefault(x => x.Value.LungerTdbId == lungerTdbId); //TODO: refaktorera. Hämta från databasen istället? 

            return lunger.Value;

        }

        public static Club GetClub(int clubTdbId)
        {

            var clubs = GetClubs();
            var club = clubs.FirstOrDefault(x => x.ClubTdbId == clubTdbId); //TODO: refaktorera. Hämta från databasen istället? 

            return club;

        }

        public static Club GetClub(string clubName)
        {
            var clubs = GetClubs();
            var club = clubs.FirstOrDefault(x => x.ClubName.Trim() == clubName);
           

            return club;
        }
        public static CompetitionClass GetClass(int classTdbId)
        {

            var classes = GetClasses();
            var competitionClass = classes.FirstOrDefault(x => x.ClassTdbId == classTdbId); //TODO: refaktorera. Hämta från databasen istället? 

            return competitionClass;
        }

        public static CompetitionClass GetClass(string className)
        {

            var classes = GetClasses();
            var competitionClass = classes.FirstOrDefault(x => x.ClassName == className); //TODO: refaktorera. Hämta från databasen istället? 

            return competitionClass;
        }

        public static Vaulter GetVaulter(int vaulterTdbId)
        {

            var vaulters = GetVaulters();
            var vaulter = vaulters.FirstOrDefault(x => x.VaulterTdbId == vaulterTdbId); //TODO: refaktorera. Hämta från databasen istället? 

            return vaulter;
        }

        public static Vaulter GetVaulter(string vaulterName)
        {

            var vaulters = GetVaulters();
            var vaulter = vaulters.FirstOrDefault(x => (x.Name.Trim() == vaulterName)); //TODO: refaktorera. Hämta från databasen istället? 

            return vaulter;
        }

        public static Team GetTeam(string teamName)
        {

            var teams = GetTeams();
            var team = teams.FirstOrDefault(x => (x.Name.Trim() == teamName)); //TODO: refaktorera. Hämta från databasen istället? 

            return team;
        }

        public static TeamList GetTeamMember(int teamId, int vaulterId)
        {
            var allTeamMembers = GetTeamMembers();
            var  member = allTeamMembers.FirstOrDefault(x => (x.TeamId == teamId && x.ParticipantId == vaulterId)); //TODO: refaktorera. Hämta från databasen istället? 

            return member;
        }


        public static Horse GetHorse(int horseTdbId, int lungerTdbId)
        {

            var horses = GetHorses();
            var horse = horses.FirstOrDefault(x => x.HorseTdbId == horseTdbId && x.Lunger.LungerTdbId == lungerTdbId); //TODO: refaktorera. Hämta från databasen istället? 

            return horse;

        }

        public static Horse GetHorse(string horseName, int lungerTdbId)
        {

            var horses = GetHorses();
            var horse = horses.FirstOrDefault(x => x.HorseName.Trim() == horseName && x.Lunger.LungerTdbId == lungerTdbId); //TODO: refaktorera. Hämta från databasen istället? 

            return horse;

        }



        public static void AddLungers(Lunger[] lungers)
        {
            using (var db = new VaultingContext())
            {
                db.Lungers.AddRange(lungers);
                db.SaveChanges();
            }
            _lungers = null;

        }
        public static void AddClubs(Club[] clubs)
        {
            using (var db = new VaultingContext())
            {
                db.Clubs.AddRange(clubs);
                db.SaveChanges();
            }
            _lungers = null;

        }

        public static void AddClasses(CompetitionClass[] classes)
        {
            using (var db = new VaultingContext())
            {
                db.CompetitionClasses.AddRange(classes);
                db.SaveChanges();
            }
            _lungers = null;

        }

        public static void AddVaulters(Vaulter[] vaulters)
        {
            using (var db = new VaultingContext())
            {
                db.Vaulters.AddRange(vaulters);
                db.SaveChanges();
            }
            _vaulters = null;

        }
        public static void AddHorses(Horse[] horses)
        {
            using (var db = new VaultingContext())
            {
                db.Horses.AddRange(horses);
                db.SaveChanges();
            }
            _horses = null;

        }

        public static void AddTeams(Team[] teams)
        {
            using (var db = new VaultingContext())
            {
                db.Teams.AddRange(teams);
                db.SaveChanges();
            }
            _horses = null;

        }

        public static void AddTeamMembers(TeamList[] teamMembers)
        {
            using (var db = new VaultingContext())
            {
                db.TeamMembers.AddRange(teamMembers);
                db.SaveChanges();
            }
            _horses = null;

        }

        public static void UpdateLungers(Lunger[] lungers)
        {
            using (var db = new VaultingContext())
            {
                foreach (var lunger in lungers)
                {
                    db.Entry(lunger).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            _lungers = null;

        }

        public static void UpdateClubs(Club[] clubs)
        {
            using (var db = new VaultingContext())
            {
                foreach (var lunger in clubs)
                {
                    db.Entry(lunger).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            _lungers = null;

        }

        public static void UpdateClasses(CompetitionClass[] competitionClasses)
        {
            using (var db = new VaultingContext())
            {
                foreach (var competitionClass in competitionClasses)
                {
                    db.Entry(competitionClass).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            _classes = null;

        }

        public static void UpdateHorses(Horse[] horses)
        {
            using (var db = new VaultingContext())
            {
                foreach (var horse in horses)
                {
                    db.Entry(horse).State = EntityState.Modified; //TODO; bara uppdatera förändrade fält
                }
                db.SaveChanges();
            }
            _horses = null;

        }

        public static void UpdateTeams(Team[] teams)
        {
            using (var db = new VaultingContext())
            {
                foreach (var team in teams)
                {
                    db.Entry(team).State = EntityState.Modified; //TODO; bara uppdatera förändrade fält
                }
                db.SaveChanges();
            }
            _teams = null;

        }

        public static void UpdateTeamMembers(TeamList[] teamMembers)
        {
            using (var db = new VaultingContext())
            {
                foreach (var teamMember in teamMembers)
                {
                    db.Entry(teamMember).State = EntityState.Modified; //TODO; bara uppdatera förändrade fält
                }
                db.SaveChanges();
            }
            _teams = null;

        }
        public static void UpdateVaulters(Vaulter[] vaulters)
        {
            using (var db = new VaultingContext())
            {
                //var vaulters1 = db.Vaulters.ToList();

                //var vaulter = vaulters1.FirstOrDefault(x => x.VaulterTdbId == vaulters[0].VaulterTdbId); //TODO: refaktorera. Hämta från databasen istället? 

                //vaulter.Name = vaulter.Name + "test";
                foreach (var vaulter in vaulters)
                {
                    //db.Entry(vaulter).State = EntityState.Added;
                    db.Entry(vaulter).State = EntityState.Modified;
                    //db.Entry(vaulter).Property(x => x.VaultingClass.Steps[0].TypeOfContestId).IsModified = false;
                    //db.Entry(vaulter).Property(x => x.VaultingClub).IsModified = false;
                    // var vaulter = vaulters[0];
                    //db.Vaulters.Attach(existingVaulter);
                    //db.Entry(existingVaulter).State = EntityState.Added; //TODO; bara uppdatera förändrade fält
                    //db.Entry(existingVaulter).State = EntityState.Unchanged;

                    //var existingVaulter = GetVaulter(vaulter.VaulterTdbId);
                    //if (existingVaulter.VaulterTdbId != vaulter.VaulterTdbId)
                    //{
                    //    existingVaulter.VaulterTdbId = vaulter.VaulterTdbId;
                    //}
                    //if (existingVaulter.Name != vaulter.Name)
                    //{
                    //    existingVaulter.Name = vaulter.Name;
                    //}
                    //if (existingVaulter.VaultingClass.ClassTdbId != vaulter.VaultingClass.ClassTdbId)
                    //{
                    //    existingVaulter.VaultingClass = vaulter.VaultingClass;
                    //}
                    //if (existingVaulter.VaultingClub.ClubTdbId != vaulter.VaultingClub.ClubTdbId)
                    //{
                    //    existingVaulter.VaultingClub = vaulter.VaultingClub;
                    //}


                }
                db.SaveChanges();
            }
            _vaulters = null;

        }


        private static Dictionary<string, Lunger> GetLungers()
        {
            if (_lungers == null)
            {
                using (var db = new VaultingContext())
                {
                    _lungers = db.Lungers.ToDictionary(x => x.LungerName?.Trim());
                }
            }

            return _lungers;

        }

        private static List<Club> GetClubs()
        {
            if (_clubs == null)
            {
                using (var db = new VaultingContext())
                {
                    _clubs = db.Clubs.ToList();
                }
            }

            return _clubs;

        }

        private static List<CompetitionClass> GetClasses()
        {
            if (_classes == null)
            {
                using (var db = new VaultingContext())
                {
                    _classes = db.CompetitionClasses.ToList();
                }
            }

            return _classes;

        }

        private static List<Vaulter> GetVaulters()
        {
            if (_vaulters == null)
            {
                using (var db = new VaultingContext())
                {
                     _vaulters = db.Vaulters.ToList();
                    foreach (var vaulter in _vaulters)
                    {
                        //var dummy1 = vaulter.VaultingClass;
                        var dummy2 = vaulter.VaultingClub;
                    }
                    //var dummy = GetAllDataFromDataBase<List<Vaulter>>(_vaulters); // bara för att hämta alla värden när vi är i context dvs inom using
                }
            }


            return _vaulters;

        }

        private static List<Team> GetTeams()
        {
            if (_teams == null)
            {
                using (var db = new VaultingContext())
                {
                    _teams = db.Teams.ToList();
                    //foreach (var team in _teams)
                    //{
                    //    var dummy = team.TeamList.ToArray(); // bara för att hämta alla värden när vi är i context dvs inom using
                    //}
                }
            }


            return _teams;

        }

        private static List<TeamList> GetTeamMembers()
        {
            if (_teamMembers == null)
            {
                using (var db = new VaultingContext())
                {
                    _teamMembers = db.TeamMembers.ToList();
                }
            }


            return _teamMembers;

        }
        private static List<Horse> GetHorses(bool forceReadFromDb = false)
        {
            if (forceReadFromDb || _horses == null)
            {
                using (var db = new VaultingContext())
                {
                     var horses = db.Horses.ToList();

                    _horses = GetAllDataFromDataBase<List<Horse>>(horses);

                }
            }

            return _horses;

        }


        private static void AddToStepsJudgesList(string classNr, string testNumberString, StartListClassStep startListClassStep)
        {
            var key = classNr + "_" + testNumberString;
            if (_stepsJudges.ContainsKey(key) || startListClassStep == null)
                return;

            _stepsJudges[key] = startListClassStep.StartListClassStepId;
        }

        private static Models.Contest GetAllDataFromDataBase()
        {
            using (var db = new VaultingContext())
            {
                var contests = db.Contests;

                var currentContestId = GetContestTypeId();
                var contest = contests.Find(currentContestId);


                _contest = GetAllDataFromDataBase<Models.Contest>(contest);
            }
            return _contest;
        }

        private static T GetAllDataFromDataBase<T>(T contest) 
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var jsonString = jsonSerializer.Serialize(contest);
            // För att ladda alla multipla nestlade entiteter som annars skulle lazy loadas och vara utanför scopet när databas connectionen stängs

            return jsonSerializer.Deserialize<T>(jsonString);
        }


        public static Models.Contest GetNewDataFromDatabase()
        {
            return GetAllDataFromDataBase();
        }

        
        public static string GetVaulterExcelId(Vaulter participant, int testNumber = 0, JudgeTable judgeTable = null)
        {
            var classId = participant.VaultingClass?.CompetitionClassId;
            var vaulterId = participant.VaulterId;
            var classnr = participant.VaultingClass?.ClassNr;
            var returnString = "id_" + classId + "_" + vaulterId + "_" + classnr;

            //var stepTypeString = GetStepTypeString(stepType);
            if (testNumber > 0 && judgeTable != null)
            {
                returnString = returnString + "_" + testNumber + "_" + judgeTable.JudgeTableName;
            }
            return returnString;
            //return "id_" + participant.VaultingClass?.CompetitionClassId + "_" + participant.VaulterId + testNumber;
        }


        public static string GetTeamExcelId(Team team, int testNumber = 0, JudgeTable judgeTable = null)
        {
            // var stepTypeString = GetStepTypeString(stepType);
            var classId = team.VaultingClass?.CompetitionClassId;
            var vaulterId = team.TeamId;
            var classnr = team.VaultingClass?.ClassNr;
            var returnString = "id_" + classId + "_" + vaulterId + "_" + classnr;

            //var stepTypeString = GetStepTypeString(stepType);
            if (testNumber > 0 && judgeTable != null)
            {
                returnString = returnString + "_" + testNumber + "_" + judgeTable.JudgeTableName;
            }

            return returnString;
            //return "id_" + team.VaultingClass?.CompetitionClassId + "_" +
            //      team.TeamId + testNumber;
        }

        private static string GetStepTypeString(StepType stepType)
        {
            var stepTypeString = "";
            if (stepType != null)
                stepTypeString = "_" + stepType.StepTypeId;
            return stepTypeString;
        }
    }
}

