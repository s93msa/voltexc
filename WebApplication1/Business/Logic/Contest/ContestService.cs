using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VoltigeCore.Classes;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Contest
{
    public class ContestService
    {
        private static Models.Contest _contest;
        private static Dictionary<string, int> _stepsJudges = null;
        private static Dictionary<string, Lunger> _lungers = null;
        private static List<Club> _clubs = null;
        private static List<Horse> _horses = null;
        private static List<CompetitionClass> _classes;
        private static List<Vaulter> _vaulters;
        private static List<Team> _teams;
        private static List<TeamList> _teamMembers;
        private static List<HorseOrder> _horseOrders;
        private static List<VaulterOrder> _vaulterOrders;
        private static List<StartListClassStep> _startListSteps;

        public static Models.Contest GetContestInstance()
        {
            if (_contest != null)
                return _contest;
            return GetAllDataFromDataBase();
        }

        public static Dictionary<string, int> GetJudgesPerStep()
        {
            if (_stepsJudges != null)
                return _stepsJudges;

            _stepsJudges = new Dictionary<string, int>();

            foreach (var startListClassStep in GetContestInstance().StartListClassStep)
            {
                foreach (var carriage in startListClassStep.GetActiveStartList())
                {
                    if (carriage.IsTeam && carriage.VaultingTeam != null)
                    {
                        var vaultingClass = carriage.VaultingTeam.VaultingClass;
                        if (vaultingClass == null) continue;
                        var testNumber = carriage.TeamTestnumber;
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

        public static int GetContestTypeId() => AppConfig.ContestId;

        public static bool IsTraHastTavling() => AppConfig.IsTraHastTavling;

        public static float HorsePointTraHastTavling() => AppConfig.HorsePointTraHastTavling;

        public static Lunger GetLunger(string lungerName)
        {
            var lungers = GetLungers();
            if (lungers != null && lungers.TryGetValue(lungerName.Trim(), out var lunger))
                return lunger;
            return null;
        }

        public static HorseOrder GetHorseOrder(int horseOrderId)
        {
            return GetHorseOrders().FindAll(x => x.HorseOrderId == horseOrderId).FirstOrDefault();
        }

        public static HorseOrder[] GetHorseOrders(int startListClassStepId)
        {
            return GetHorseOrders().FindAll(x => x.StartListClassStepId == startListClassStepId).ToArray();
        }

        public static HorseOrder[] GetHorseOrders(int? startListClassStepId, int? horseId, int startNumber)
        {
            return GetHorseOrders().FindAll(x => x.StartListClassStepId == startListClassStepId &&
                                                  x.HorseId == horseId && x.StartNumber == startNumber).ToArray();
        }

        public static HorseOrder GetTeamHorseOrder(int? startListClassStepId, int? vaultingTeamId, int testnumber)
        {
            return GetHorseOrders().FirstOrDefault(x => x.StartListClassStepId == startListClassStepId &&
                                                         x.VaultingTeamId == vaultingTeamId && x.TeamTestnumber == testnumber);
        }

        public static Lunger GetLunger(int lungerTdbId)
        {
            var lunger = GetLungers().FirstOrDefault(x => x.Value.LungerTdbId == lungerTdbId);
            return lunger.Value;
        }

        public static Club GetClub(int clubTdbId)
        {
            return GetClubs().FirstOrDefault(x => x.ClubTdbId == clubTdbId);
        }

        public static Club GetClub(string clubName)
        {
            return GetClubs().FirstOrDefault(x => x.ClubName.Trim() == clubName.Trim());
        }

        public static StartListClassStep GetStartListStep(int id)
        {
            return GetStartListSteps().FirstOrDefault(x => x.StartListClassStepId == id);
        }

        public static CompetitionClass GetClass(int classTdbId)
        {
            return GetClasses().FirstOrDefault(x => x.ClassTdbId == classTdbId);
        }

        public static CompetitionClass GetClass(string className)
        {
            return GetClasses().FirstOrDefault(x => x.ClassName == className);
        }

        public static Vaulter GetVaulter(int vaulterTdbId)
        {
            return GetVaulters().FirstOrDefault(x => x.VaulterTdbId == vaulterTdbId);
        }

        public static Vaulter GetVaulter(string vaulterName)
        {
            return GetVaulters().FirstOrDefault(x => x.Name.Trim() == vaulterName);
        }

        public static Team GetTeam(string teamName)
        {
            return GetTeams().FirstOrDefault(x => x.Name.Trim() == teamName);
        }

        public static TeamList GetTeamMember(int teamId, int vaulterId)
        {
            return GetTeamMembers().FirstOrDefault(x => x.TeamId == teamId && x.ParticipantId == vaulterId);
        }

        public static Horse GetHorse(int horseTdbId, int lungerTdbId)
        {
            return GetHorses().FirstOrDefault(x => x.HorseTdbId == horseTdbId && x.Lunger.LungerTdbId == lungerTdbId);
        }

        public static VaulterOrder GetVaulterOrder(int vaulterOrderId)
        {
            return GetVaulterOrders().FirstOrDefault(x => x.VaulterOrderID == vaulterOrderId);
        }

        public static VaulterOrder GetVaulterOrder(int[] horseOrderIds, int vaulterId, int testNumber)
        {
            return GetVaulterOrders().FirstOrDefault(x => horseOrderIds.Contains(x.HorseOrderId ?? -1) &&
                                                           x.VaulterId == vaulterId && x.Testnumber == testNumber);
        }

        public static Horse GetHorse(string horseName, int lungerTdbId)
        {
            return GetHorses().FirstOrDefault(x => x.HorseName.Trim() == horseName && x.Lunger.LungerTdbId == lungerTdbId);
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
            _clubs = null;
        }

        public static void AddStartListSteps(ICollection<StartListClassStep> startListClassSteps)
        {
            using (var db = new VaultingContext())
            {
                db.StartListClassSteps.AddRange(startListClassSteps);
                db.SaveChanges();
            }
            _startListSteps = null;
        }

        public static void AddClasses(CompetitionClass[] classes)
        {
            using (var db = new VaultingContext())
            {
                db.CompetitionClasses.AddRange(classes);
                db.SaveChanges();
            }
            _classes = null;
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

        public static void AddHorseOrders(HorseOrder[] horseOrders)
        {
            using (var db = new VaultingContext())
            {
                db.HorseOrders.AddRange(horseOrders);
                db.SaveChanges();
            }
            _horseOrders = null;
        }

        public static void AddVaulterOrders(VaulterOrder[] vaulterOrders)
        {
            using (var db = new VaultingContext())
            {
                db.VaulterOrders.AddRange(vaulterOrders);
                db.SaveChanges();
            }
            _vaulterOrders = null;
        }

        public static void AddTeams(Team[] teams)
        {
            using (var db = new VaultingContext())
            {
                db.Teams.AddRange(teams);
                db.SaveChanges();
            }
            _teams = null;
        }

        public static void AddTeamMembers(TeamList[] teamMembers)
        {
            using (var db = new VaultingContext())
            {
                db.TeamMembers.AddRange(teamMembers);
                db.SaveChanges();
            }
            _teamMembers = null;
        }

        public static void UpdateLungers(Lunger[] lungers)
        {
            using (var db = new VaultingContext())
            {
                foreach (var lunger in lungers)
                    db.Entry(lunger).State = EntityState.Modified;
                db.SaveChanges();
            }
            _lungers = null;
        }

        public static void UpdateClubs(Club[] clubs)
        {
            using (var db = new VaultingContext())
            {
                foreach (var club in clubs)
                    db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
            }
            _clubs = null;
        }

        public static void UpdateStartListSteps(ICollection<StartListClassStep> startListSteps)
        {
            using (var db = new VaultingContext())
            {
                foreach (var step in startListSteps)
                    db.Entry(step).State = EntityState.Modified;
                db.SaveChanges();
            }
            _startListSteps = null;
        }

        public static void UpdateClasses(CompetitionClass[] competitionClasses)
        {
            using (var db = new VaultingContext())
            {
                foreach (var c in competitionClasses)
                    db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
            }
            _classes = null;
        }

        public static void UpdateHorses(Horse[] horses)
        {
            using (var db = new VaultingContext())
            {
                foreach (var horse in horses)
                    db.Entry(horse).State = EntityState.Modified;
                db.SaveChanges();
            }
            _horses = null;
        }

        public static void UpdateVaulterOrder(VaulterOrder vaulterOrder) => UpdateVaulterOrders(new[] { vaulterOrder });

        public static void UpdateVaulterOrders(VaulterOrder[] vaulterOrders)
        {
            using (var db = new VaultingContext())
            {
                foreach (var vo in vaulterOrders)
                    db.Entry(vo).State = EntityState.Modified;
                db.SaveChanges();
            }
            _vaulterOrders = null;
        }

        public static int AddHorseOrder(HorseOrder horseOrder)
        {
            using (var db = new VaultingContext())
            {
                db.HorseOrders.Add(horseOrder);
                db.SaveChanges();
            }
            _horseOrders = null;
            return horseOrder.HorseOrderId;
        }

        public static void UpdateHorseOrder(HorseOrder horseOrder) => UpdateHorseOrder(new[] { horseOrder });

        public static void UpdateHorseOrder(HorseOrder[] horseOrders)
        {
            using (var db = new VaultingContext())
            {
                foreach (var ho in horseOrders)
                    db.Entry(ho).State = EntityState.Modified;
                db.SaveChanges();
            }
            _horseOrders = null;
        }

        public static void UpdateTeams(Team[] teams)
        {
            using (var db = new VaultingContext())
            {
                foreach (var team in teams)
                    db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
            }
            _teams = null;
        }

        public static void UpdateTeamMembers(TeamList[] teamMembers)
        {
            using (var db = new VaultingContext())
            {
                foreach (var tm in teamMembers)
                    db.Entry(tm).State = EntityState.Modified;
                db.SaveChanges();
            }
            _teamMembers = null;
        }

        public static void UpdateVaulters(Vaulter[] vaulters)
        {
            using (var db = new VaultingContext())
            {
                foreach (var vaulter in vaulters)
                    db.Entry(vaulter).State = EntityState.Modified;
                db.SaveChanges();
            }
            _vaulters = null;
        }

        private static Dictionary<string, Lunger> GetLungers()
        {
            if (_lungers == null)
            {
                using (var db = new VaultingContext())
                    _lungers = db.Lungers.ToDictionary(x => x.LungerName?.Trim());
            }
            return _lungers;
        }

        private static List<HorseOrder> GetHorseOrders()
        {
            if (_horseOrders == null)
            {
                using (var db = new VaultingContext())
                {
                    _horseOrders = db.HorseOrders.ToList();
                    foreach (var ho in _horseOrders)
                    {
                        var dummy1 = ho.HorseInformation.Lunger;
                        var dummy2 = ho.Vaulters;
                    }
                }
            }
            return _horseOrders;
        }

        private static List<Club> GetClubs()
        {
            if (_clubs == null)
            {
                using (var db = new VaultingContext())
                    _clubs = db.Clubs.ToList();
            }
            return _clubs;
        }

        private static ICollection<StartListClassStep> GetStartListSteps()
        {
            if (_startListSteps == null)
            {
                using (var db = new VaultingContext())
                    _startListSteps = db.StartListClassSteps.ToList();
            }
            return _startListSteps;
        }

        private static List<CompetitionClass> GetClasses()
        {
            if (_classes == null)
            {
                using (var db = new VaultingContext())
                    _classes = db.CompetitionClasses.ToList();
            }
            return _classes;
        }

        public static List<Vaulter> GetVaulters()
        {
            if (_vaulters == null)
            {
                using (var db = new VaultingContext())
                {
                    _vaulters = db.Vaulters.ToList();
                    foreach (var v in _vaulters)
                    {
                        var dummy1 = v.VaultingClass;
                        var dummy2 = v.VaultingClub;
                    }
                }
            }
            return _vaulters;
        }

        public static List<Team> GetTeams()
        {
            if (_teams == null)
            {
                using (var db = new VaultingContext())
                    _teams = db.Teams.ToList();
            }
            return _teams;
        }

        private static List<TeamList> GetTeamMembers()
        {
            if (_teamMembers == null)
            {
                using (var db = new VaultingContext())
                    _teamMembers = db.TeamMembers.ToList();
            }
            return _teamMembers;
        }

        public static List<Horse> GetHorses(bool forceReadFromDb = false)
        {
            if (forceReadFromDb || _horses == null)
            {
                using (var db = new VaultingContext())
                {
                    var horses = db.Horses.ToList();
                    _horses = DeepCopy(horses);
                }
            }
            return _horses;
        }

        private static List<VaulterOrder> GetVaulterOrders(bool forceReadFromDb = false)
        {
            if (forceReadFromDb || _vaulterOrders == null)
            {
                using (var db = new VaultingContext())
                    _vaulterOrders = db.VaulterOrders.ToList();
            }
            return _vaulterOrders;
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
                var currentContestId = GetContestTypeId();
                var contest = db.Contests.Find(currentContestId);
                _contest = DeepCopy(contest);
            }
            return _contest;
        }

        private static T DeepCopy<T>(T obj)
        {
                var settings = new JsonSerializerSettings
                {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                var json = JsonConvert.SerializeObject(obj, settings);
            return JsonConvert.DeserializeObject<T>(json);
         }


        public static Models.Contest GetNewDataFromDatabase()
        {
            _contest = null;
            return GetAllDataFromDataBase();
        }

        public static string GetVaulterExcelId(Vaulter participant, int horseId, int testNumber = 0, JudgeTable judgeTable = null)
        {
            var returnStrings = new List<string>();
            var vaulterId = participant.VaulterId;
            var classnrArray = participant.VaultingClass?.ClassNr.Split('.');
            foreach (var classNr in classnrArray)
            {
                var returnString = "id_" + vaulterId + "_" + classNr + "_" + horseId;
                if (testNumber > 0 && judgeTable != null)
                    returnString = returnString + "_" + testNumber + "_" + judgeTable.JudgeTableName;
                returnStrings.Add(returnString);
            }
            return string.Join(",", returnStrings);
        }

        public static string GetTeamExcelId(Team team, int horseId, int testNumber = 0, JudgeTable judgeTable = null)
        {
            var returnStrings = new List<string>();
            var teamId = team.TeamId;
            var classnrArray = team.VaultingClass?.ClassNr.Split('.');
            foreach (var classNr in classnrArray)
            {
                var returnString = "id_" + teamId + "_" + classNr + "_" + horseId;
                if (testNumber > 0 && judgeTable != null)
                    returnString = returnString + "_" + testNumber + "_" + judgeTable.JudgeTableName;
                returnStrings.Add(returnString);
            }
            return string.Join(",", returnStrings);
        }

        public static List<int?> GetAllClassesWithAtleastOneParticipant(VaultingContext db)
        {
            var classesList = GetAllClassesWithAtLeastOneVaulter(db);
            classesList.AddRange(GetAllClassesWithAtLeastOneTeam(db));
            return classesList;
        }

        private static List<int?> GetAllClassesWithAtLeastOneVaulter(VaultingContext db)
        {
            return db.Vaulters.GroupBy(x => x.VaultingClassId).Select(grp => grp.FirstOrDefault().VaultingClassId).ToList();
        }

        private static List<int?> GetAllClassesWithAtLeastOneTeam(VaultingContext db)
        {
            return db.Teams.GroupBy(x => x.VaultingClassId).Select(grp => grp.FirstOrDefault().VaultingClassId).ToList();
        }
    }
}
