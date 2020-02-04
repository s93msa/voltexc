using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using WebApplication1.Migrations;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Import
{
    public class ExcelImportService
    {
        private ExcelImportRepository _excelImportRepository;

        public ExcelImportService(XLWorkbook workbook)
        {
            _excelImportRepository = new ExcelImportRepository(workbook);
        }

        public HorseOrder[] GetHorseOrderIndividual(int[] competionClassesTdbIds,
            int startListClassStepId, int vaulterTestnumber)
        {
            var horseOrderList = GetIndividualHorseOrders(competionClassesTdbIds);
            foreach (var horseOrder in horseOrderList)
            {
                horseOrder.StartListClassStepId = startListClassStepId;
                horseOrder.Vaulters.ForEach(x => x.Testnumber = vaulterTestnumber);
            }

            return horseOrderList;
        }

        public HorseOrder[] GetHorseOrdersTeam(int[] competionClassesTdbIds,
            int startListClassStepId, int teamTestnumber)
        {
            var horseOrderList = GetTeamHorseOrders(competionClassesTdbIds);
            foreach (var horseOrder in horseOrderList)
            {
                horseOrder.StartListClassStepId = startListClassStepId;
                horseOrder.TeamTestnumber = teamTestnumber;
            }

            return horseOrderList;
        }

        private HorseOrder[] GetIndividualHorseOrders(int[] competionClassesTdbIds)
        {
            var rows = GetRows(competionClassesTdbIds);
            var individualRows = rows.Where(x => x.IsTeam == false).ToArray();
            var listofDistinctHorseTdbIds = individualRows.GroupBy(x => new { x.HorseTdbId, x.LungerTdbId}).Select(h => h.First());
            var horseOrders = new List<HorseOrder>();
            foreach (var row in listofDistinctHorseTdbIds)
            {
                var vaultersRows = individualRows.Where(x => x.HorseTdbId == row.HorseTdbId && x.LungerTdbId == row.LungerTdbId);
                var lunger = new Lunger() { LungerTdbId = row.LungerTdbId };

                var vaultersOrder = new List<VaulterOrder>();
                int vaulterStartOrder = 1;
                foreach (var vaultersRow in vaultersRows)
                {
                    var vaulter = new Vaulter() {VaulterTdbId = vaultersRow.VaulterId1, Name = vaultersRow.VaulterName1};
                    var vaulterOrder = new VaulterOrder() {StartOrder = vaulterStartOrder++, Participant = vaulter, IsActive = true};
                    vaultersOrder.Add(vaulterOrder);
                }

                int startNumber = 1;
                var horseOrder = new HorseOrder
                {
                    StartNumber = startNumber++,
                    HorseInformation = new Horse() { HorseTdbId = row.HorseTdbId, Lunger = lunger },
                    IsActive = true,
                    IsTeam = false,
                    Vaulters = vaultersOrder
                };
                horseOrders.Add(horseOrder);

            }

            return horseOrders.ToArray();


        }

    private HorseOrder[] GetTeamHorseOrders(int[] competionClassesTdbIds)
        {
            var rows = GetRows(competionClassesTdbIds);

            var teamRows = rows.Where(x => x.IsTeam).ToArray();
            List<HorseOrder> horseOrders = new List<HorseOrder>();
            int startNumber = 1;
            foreach (var teamRow in teamRows)
            {
                var lunger = new Lunger() {LungerTdbId = teamRow.LungerTdbId};

                var horseOrder = new HorseOrder
                {
                    StartNumber = startNumber++,
                    HorseInformation = new Horse() {HorseTdbId = teamRow.HorseTdbId, Lunger = lunger},
                    IsActive = true,
                    IsTeam = true,
                    VaultingTeam = new Team() {Name = teamRow.TeamName}
                };

                horseOrders.Add(horseOrder);
            }

            return horseOrders.ToArray();
        }

       

        public ExcelImportMergedModel[] GetRows(int[] classNumbers)
        {
            var mergedInfo = GetMergedInfo();
            IEnumerable<ExcelImportMergedModel> rowQuery = mergedInfo;
            //foreach (var classTdbId in classNumbers)
            //{
            //}
            rowQuery = rowQuery.Where(x => classNumbers.Contains(x.ClassTdbId));
            return rowQuery.ToArray();
        }
        //public ExcelImportMergedModel[] GetRows(int[] classTdbIds, int horseTdbId)
        //{
        //    var mergedInfo = GetMergedInfo();
        //    var rowQuery = mergedInfo.Where(x => x.HorseTdbId == horseTdbId);
        //    foreach (var classTdbId in classTdbIds)
        //    {
        //        rowQuery = rowQuery.Where(x => x.ClassTdbId == classTdbId);
        //    }
        //    return rowQuery.ToArray();
        //    //_excelImportRepository.GetVaulters(classTdbIds)
        //}


        public Lunger[] GetLungers()
        {
            return _excelImportRepository.GetLungers();          
        }

        public Horse[] GetHorses()
        {
            //TODO: cache
            var horsesArray = _excelImportRepository.GetHorses();
            var horses = new List<Horse>(horsesArray);
            horses = SetLungers(horses);
            return horses.ToArray();
        }

        public Vaulter[] GetVaulters()
        {
            var individualVaulters = GetIndividualVaulters();
            var vaulters = AddTeamVaulters(individualVaulters);

            return vaulters.ToArray();
        }


        public TeamMember[] GetTeamMembers()
        {
            var mergedInfo = GetMergedInfo();
            var teamRows = mergedInfo.Where(x => x.IsTeam).ToArray();
            var teamMembersList = new List<TeamMember>();
            foreach (var teamRow in teamRows)
            {

                var member1 = GetTeamMember(teamRow.VaulterName1, teamRow.VaulterId1, 1, teamRow.TeamName);
                var member2 = GetTeamMember(teamRow.VaulterName2, teamRow.VaulterId2, 2, teamRow.TeamName);
                var member3 = GetTeamMember(teamRow.VaulterName3, teamRow.VaulterId3, 3, teamRow.TeamName);
                var member4 = GetTeamMember(teamRow.VaulterName4, teamRow.VaulterId4, 4, teamRow.TeamName);
                var member5 = GetTeamMember(teamRow.VaulterName5, teamRow.VaulterId5, 5, teamRow.TeamName);
                var member6 = GetTeamMember(teamRow.VaulterName6, teamRow.VaulterId6, 6, teamRow.TeamName);
                var member7 = GetTeamMember(teamRow.VaulterName7, teamRow.VaulterId7, 7, teamRow.TeamName);
                var member8 = GetTeamMember(teamRow.VaulterName8, teamRow.VaulterId8, 8, teamRow.TeamName);

                var teamMembers = new List<TeamMember> { member1, member2, member3, member4, member5, member6, member7, member8 };
                teamMembersList.AddRange(teamMembers);
            }
            return teamMembersList.ToArray();
        }

            public Team[] GetTeams()
        {
            var mergedInfo = GetMergedInfo();
            var teamRows = mergedInfo.Where(x => x.IsTeam).ToArray();

            var teamList = new List<Team>();

            foreach (var teamRow in teamRows)
            {
                Club club = CreateClubInstance(teamRow.ClubTdbId, teamRow.ClubName);

                var competitionClass = new CompetitionClass
                {
                    ClassTdbId = teamRow.ClassTdbId,
                    ClassNr = teamRow.ClassNr,
                    ClassName = teamRow.ClassName
                };

             

                var newTeam = new Team
                {
                    Name = teamRow.TeamName,
                    VaultingClub = club,
                    VaultingClass = competitionClass,
                };

                teamList.Add(newTeam);

            }
            return teamList.ToArray();

        }

        private static Club CreateClubInstance(int clubTdbId, string clubName)
        {
            return new Club
            {
                ClubTdbId = clubTdbId,
                ClubName = clubName
            };
        }

        private static TeamMember GetTeamMember(string vaulterName, int vaulterTdbId, int startNumber, string teamName)
        {
            if (vaulterTdbId == 0)
                return null;

            var teamMember = new TeamMember()
            {
                VaulterName = vaulterName,
                VaulterTdbId = vaulterTdbId,
                StartNumber = startNumber,
                TeamName = teamName
            };
            return teamMember;
        }

        private List<Vaulter> AddTeamVaulters(Vaulter[] individualVaulters)
        {
            var vaulters = new List<Vaulter>(individualVaulters);

            var teamVaulters = GetTeamVaulters();
            foreach (var teamVaulter in teamVaulters)
            {
                if (!vaulters.Exists(x => x.VaulterTdbId == teamVaulter.VaulterTdbId))
                {
                    vaulters.Add(teamVaulter);
                }
            }
            return vaulters;
        }

        public Vaulter[] GetIndividualVaulters()
        {
            //TODO: cache
            var vaultersArray = _excelImportRepository.GetVaulters();
            var vaulters = new List<Vaulter>(vaultersArray);
            vaulters = SetClub(vaulters);
            vaulters = SetClass(vaulters);
            return vaulters.ToArray();
        }

        public Vaulter[] GetTeamVaulters()
        {
            //TODO: cache
            var vaulters = _excelImportRepository.GetTeamVaulters();
            return vaulters;
        }

        public Club[] GetClubs()
        {
            //TODO: cache
            return _excelImportRepository.GetClubs();
        }

        public CompetitionClass[] GetClasses()
        {
            //TODO: cache
            return _excelImportRepository.GetClasses();
        }

        private List<Horse> SetLungers(List<Horse> horses)
        {
            var moreHorses = new List<Horse>();
            foreach (var horse in horses)
            {
                var lungers = GetLungers(horse.HorseTdbId);
                horse.Lunger = lungers.FirstOrDefault();
                moreHorses = ExtendHorsesWithMoreLungers(horse, lungers, moreHorses);
            }
            horses.AddRange(moreHorses);

            return horses;
        }

        private List<Vaulter> SetClub(List<Vaulter> vaulters)
        {
//            var moreVaulters = new List<Vaulter>();
            foreach (var vaulter in vaulters)
            {
                var clubs = GetClubs(vaulter.VaulterTdbId);
                vaulter.VaultingClub = clubs.FirstOrDefault();
                //moreVaulters = ExtendHorsesWithMoreClubs(horse, lungers, moreHorses); //Lägg till om voltigörer kan tillhöra flera klubbar i samma tävling 
            }
            //vaulters.AddRange(moreVaulters);

            return vaulters;
        }

        private List<Vaulter> SetClass(List<Vaulter> vaulters)
        {
            //            var moreVaulters = new List<Vaulter>();
            foreach (var vaulter in vaulters)
            {
                var competitionClasses = GetClasses(vaulter.VaulterTdbId);
                vaulter.VaultingClass = competitionClasses.FirstOrDefault();
                //moreVaulters = ExtendHorsesWithMoreClubs(horse, lungers, moreHorses); //Lägg till om voltigörer kan tillhöra flera klasser i samma tävling 
            }
            //vaulters.AddRange(moreVaulters);

            return vaulters;
        }

        private static List<Horse> ExtendHorsesWithMoreLungers(Horse horse, Lunger[] lungers, List<Horse> newHorseLungers)
        {
            int noOfLungers = lungers.Length;
            if (noOfLungers > 1)
            {
                for (int i = 1; i < noOfLungers; i++)
                {
                    var newHorseLunger = Mapper.Map<Horse>(horse);
                    newHorseLunger.Lunger = lungers[i];
                    newHorseLungers.Add(newHorseLunger);
                }
            }

            return newHorseLungers;
        }

        public Lunger[] GetLungers(int horseTdbId)
        {
            var mergedInfo  = GetMergedInfo();
            var filteredRows = mergedInfo.Where(x => x.HorseTdbId == horseTdbId).ToArray();

            var lungers = new List<Lunger>();

            foreach (var row in filteredRows)
            {
                if (lungers.Exists(x => x.LungerTdbId == row.LungerTdbId))
                {
                    continue;
                }

                var lunger = new Lunger
                {
                    LungerTdbId = row.LungerTdbId,
                    LungerName = row.LungerName
                };
                lungers.Add(lunger);                    ;
            }
            return lungers.ToArray();
        }
        public Club[] GetClubs(int vaulterTdbId)
        {
            var mergedInfo = GetMergedInfo();
            var filteredRows = mergedInfo.Where(x => !x.IsTeam && x.VaulterId1 == vaulterTdbId).ToArray();

            var clubs = new List<Club>();

            foreach (var row in filteredRows)
            {
                if (clubs.Exists(x => x.ClubTdbId == row.ClubTdbId))
                {
                    continue;
                }

                var club = new Club()
                {
                    ClubTdbId = row.ClubTdbId,
                    ClubName = row.ClubName
                };
                clubs.Add(club); ;
            }
            return clubs.ToArray();
        }

        public CompetitionClass[] GetClasses(int vaulterTdbId)
        {
            var mergedInfo = GetMergedInfo();
            var filteredRows = mergedInfo.Where(x => !x.IsTeam && x.VaulterId1 == vaulterTdbId).ToArray();

            var classes = new List<CompetitionClass>();

            foreach (var row in filteredRows)
            {
                if (classes.Exists(x => x.ClassTdbId == row.ClassTdbId))
                {
                    continue;
                }

                var competitionClass = new CompetitionClass()
                {
                    ClassTdbId = row.ClassTdbId,
                    ClassName = row.ClassName,
                    ClassNr = row.ClassNr
                   
                };
                classes.Add(competitionClass); ;
            }
            return classes.ToArray();
        }


        public ExcelImportMergedModel[] GetMergedInfo()
        {
            //TODO: cache
            return _excelImportRepository.GetMergedInfo();
        }


        
    }
}


