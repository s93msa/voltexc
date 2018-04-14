using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Classes;
using WebApplication1.Migrations;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Import
{
    public class UpdateService
    {
        //public Dictionary<string, Horse> GetHorses()
        //{
        //    return new Dictionary<string, Horse>();
        //}
        //public Dictionary<string, Lunger> GetLungers()
        //{
        //    return new Horse[0];
        //}

        

        public void UpdateLungers(Lunger[] lungers)
        {
            var newLungers = new List<Lunger>();
            var updatedLungers = new List<Lunger>();
            foreach (var lunger in lungers)
            {
                var existingLunger = GetExistingLunger(lunger);
                if (existingLunger != null)
                {
                    if (NotEqual(lunger, existingLunger))
                    {
                        existingLunger.LungerName = lunger.LungerName;
                        existingLunger.LungerTdbId = lunger.LungerTdbId;
                        updatedLungers.Add(existingLunger);
                    }
                }
                else
                {
                    newLungers.Add(lunger);
                }
            }

            ContestService.UpdateLungers(updatedLungers.ToArray());
            ContestService.AddLungers(newLungers.ToArray());
        }

        public void UpdateClubs(Club[] clubs)
        {
            var newClubs = new List<Club>();
            var updatedClubs = new List<Club>();
            foreach (var club in clubs)
            {
                var existingClub = GetExistingClub(club);
                if (existingClub != null)
                {
                    if (NotEqual(club, existingClub))
                    {
                        existingClub.ClubName = club.ClubName;
                        existingClub.ClubTdbId = club.ClubTdbId;
                        updatedClubs.Add(existingClub);
                    }
                }
                else
                {
                    newClubs.Add(club);
                }
            }

            ContestService.UpdateClubs(updatedClubs.ToArray());
            ContestService.AddClubs(newClubs.ToArray());
        }

        public void UpdateClasses(CompetitionClass[] classes)
        {
            var newClasses = new List<CompetitionClass>();
            var updatedClasses = new List<CompetitionClass>();
            foreach (var competitionClass in classes)
            {
                var existingClass = GetExistingClass(competitionClass);
                if (existingClass != null)
                {
                    if (NotEqual(competitionClass, existingClass))
                    {
                        existingClass.ClassName = competitionClass.ClassName;
                        existingClass.ClassNr = competitionClass.ClassNr;
                        existingClass.ClassTdbId = competitionClass.ClassTdbId;
                        updatedClasses.Add(existingClass);
                    }
                }
                else
                {
                    newClasses.Add(competitionClass);
                }
            }

            ContestService.UpdateClasses(updatedClasses.ToArray());
            ContestService.AddClasses(newClasses.ToArray());
        }

        public void UpdateVaulters(Vaulter[] vaulters)
        {
            var newVaulters = new List<Vaulter>();
            var updatetdVaulters = new List<Vaulter>();
            foreach (var vaulter in vaulters)
            {
                var existingVaulter = GetExistingVaulter(vaulter);
                var vaultingClassId = GetExistingClass(vaulter.VaultingClass)?.CompetitionClassId;
                var vaultingClubId = GetExistingClub(vaulter.VaultingClub)?.ClubId;
                if (existingVaulter != null)
                {
                    if (NotEqual(vaulter, existingVaulter))
                    {
                        existingVaulter.Name = vaulter.Name.Trim();
                        existingVaulter.VaulterTdbId = vaulter.VaulterTdbId;
                        if (vaultingClassId != null)
                        {
                            existingVaulter.VaultingClassId = vaultingClassId;
                        }
                        if (vaulter.VaultingClubId != null) 
                        {
                            existingVaulter.VaultingClubId = vaultingClubId; 
                        }
                        updatetdVaulters.Add(existingVaulter);
                    }
                }
                else
                {
                    vaulter.VaultingClassId = vaultingClassId;
                    vaulter.VaultingClass = null;
                    vaulter.VaultingClubId = vaultingClubId;
                    vaulter.VaultingClub = null;
                    newVaulters.Add(vaulter);
                }
            }

            //updatetdVaulters = SetClassDatabaseId(updatetdVaulters);
            //updatetdVaulters = SetClubDatabaseId(updatetdVaulters);
            ContestService.UpdateVaulters(updatetdVaulters.ToArray());
            //newVaulters = SetClassDatabaseId(newVaulters);
            //newVaulters = SetClubDatabaseId(newVaulters);
            ContestService.AddVaulters(newVaulters.ToArray());

        }

        public void UpdateTeamMembers(TeamMember[] teamMembers)
        {
            var newMember = new List<TeamList>();
            var updatedMember = new List<TeamList>();

            foreach (var member in teamMembers)
            {
                if (member == null)
                {
                    continue;
                }

                var existingTeam = GetExistingTeam(member.TeamName);
                if (existingTeam == null)
                    continue;
                var existingVaulter = GetExistingVaulter(member.VaulterTdbId,member.VaulterName);
                if(existingVaulter == null)
                    continue;

                var existingTeamMember = GetExistingTeamMember(existingTeam.TeamId, existingVaulter.VaulterId);
                if (existingTeamMember == null)
                {
                    var newTeamMember = new TeamList
                    {
                        ParticipantId = existingVaulter.VaulterId,
                        StartNumber = member.StartNumber,
                        TeamId = existingTeam.TeamId
                    };
                    newMember.Add(newTeamMember);
                    continue;
                }

                if (NotEqual(member, existingTeamMember))
                {
                    existingTeamMember.StartNumber = member.StartNumber;
                    updatedMember.Add(existingTeamMember);

                }


            }
            ContestService.UpdateTeamMembers(updatedMember.ToArray());

            ContestService.AddTeamMembers(newMember.ToArray());
        }

        public void UpdateTeams(Team[] teams)
        {
            var newTeams = new List<Team>();
            var updatedTeams = new List<Team>();
            foreach (var team in teams)
            {
//                var existingTeamMembers = GetExistingTeamMembers(team);
                var existingTeam = GetExistingTeam(team);
                if (existingTeam != null)
                {
                    var competitionClassId = GetExistingClass(team.VaultingClass)?.CompetitionClassId;
                    var competitionClubId = GetExistingClub(team.VaultingClub)?.ClubId;

                    team.VaultingClassId = competitionClassId;
                    team.VaultingClubId = competitionClubId;

                    //foreach (var vaulter in team.TeamList)
                    //{
                    //    vaulter.
                    //}

                    if (NotEqual(team, existingTeam))
                    {

                        if (competitionClassId != null)
                        {
                            existingTeam.VaultingClassId = competitionClassId;
                        }
                        if (competitionClubId != null)
                        {
                            existingTeam.VaultingClubId = competitionClubId;
                        }
                    
                        updatedTeams.Add(existingTeam);
                    }
                }
                else
                {

                    newTeams.Add(team);
                }
            }

            //updatedHorses = SetLungerDatabaseId(updatedHorses);
            ContestService.UpdateTeams(updatedTeams.ToArray());

            //newHorses = SetLungerDatabaseId(newHorses);
            ContestService.AddTeams(newTeams.ToArray());

        }

        public void UpdateIndividualHorseOrders(HorseOrder[] horseOrders)
        {
            var newHorseOrders = GetMissingHorseOrders(horseOrders);

            ContestService.AddHorseOrders(newHorseOrders.ToArray());


            var newVaulterOrders = GetMissingVaulterOrders(horseOrders);
            ContestService.AddVaulterOrders(newVaulterOrders.ToArray());

           // ContestService.UpdateHorseOrder(updatedHorseOrders.ToArray());

        }

        private static List<VaulterOrder> GetMissingVaulterOrders(HorseOrder[] horseOrders)
        {
            var newVaulterOrders = new List<VaulterOrder>();
            foreach (var horseOrder in horseOrders)
            {
                var existingHorseOrders = GetExistingHorseOrderIndividual(horseOrder);
//                var existingHorse = GetExistingHorse(horseOrder.HorseInformation);

                // kolla om horseOrder Finns. Är den förändrad?

                //var existingHorse = GetExistingHorse(horse);
                var horseOrderIds = existingHorseOrders.Select(x => x.HorseOrderId).ToArray();

                foreach (var importedVaulterOrder in horseOrder.Vaulters)
                {
                    var existingVaulterOrder = GetExistingVaulterOrder(horseOrderIds, importedVaulterOrder);
                    if (existingVaulterOrder == null)
                    {
                        var vaulterTdbId = importedVaulterOrder.Participant.VaulterTdbId;
                        var vaulterName = importedVaulterOrder.Participant.Name;
                        var vaulterId = GetExistingVaulter(vaulterTdbId, vaulterName).VaulterId;
                        var testNumber = importedVaulterOrder.Testnumber;

                        var vaulterOrder = new VaulterOrder()
                        {
                            HorseOrderId = horseOrderIds.FirstOrDefault(),
                            VaulterId = vaulterId,
                            IsActive = true,
                            StartOrder = 1,
                            Testnumber = testNumber
                        };
                        newVaulterOrders.Add(vaulterOrder);
                    }
                }
            }
            return newVaulterOrders;
        }

        private static List<HorseOrder> GetMissingHorseOrders(HorseOrder[] horseOrders)
        {
            var newHorseOrders = new List<HorseOrder>();
            foreach (var horseOrder in horseOrders)
            {
                var existingHorseOrders = GetExistingHorseOrderIndividual(horseOrder);
                var horseId = GetExistingHorse(horseOrder.HorseInformation).HorseId;
                if (existingHorseOrders == null || existingHorseOrders.Length == 0)
                {
                    var newHorseOrder = new HorseOrder()
                    {
                        HorseId = horseId,
                        IsActive = true,
                        IsTeam = false,
                        StartListClassStepId = horseOrder.StartListClassStepId,
                        StartNumber = horseOrder.StartNumber
                    };
                    newHorseOrders.Add(newHorseOrder);
                }
            }

            return newHorseOrders;
        }

        public void UpdateTeamHorseOrders(HorseOrder[] horseOrders)
        {
            var newHorseOrders = new List<HorseOrder>();
            var updatedHorseOrders = new List<HorseOrder>();
            foreach (var horseOrder in horseOrders)
            {
                var existingHorseOrder = GetExistingHorseOrderTeam(horseOrder);

                var existingTeam = GetExistingTeam(horseOrder.VaultingTeam.Name);

                var existingHorse = GetExistingHorse(horseOrder.HorseInformation);

                // kolla om horseOrder Finns. Är den förändrad?

                //var existingHorse = GetExistingHorse(horse);
                if (existingHorseOrder != null)
                {
                    if (NotEqual(horseOrder, existingHorseOrder))
                    {
                        existingHorseOrder.StartNumber = horseOrder.StartNumber;
                        existingHorseOrder.HorseId = existingHorse.HorseId;
                        existingHorseOrder.VaultingTeamId = existingTeam.TeamId;
                        updatedHorseOrders.Add(existingHorseOrder);
                    }
                }
                else
                {
                    horseOrder.HorseId = existingHorse.HorseId;
                    horseOrder.HorseInformation = null;
                    horseOrder.VaultingTeamId = existingTeam.TeamId;
                    horseOrder.VaultingTeam = null;
                    newHorseOrders.Add(horseOrder);
                }
            }

            ContestService.UpdateHorseOrder(updatedHorseOrders.ToArray());
            ContestService.AddHorseOrders(newHorseOrders.ToArray());

        }

        public void UpdateHorses(Horse[] horses)
        {
            var newHorses = new List<Horse>();
            var updatedHorses = new List<Horse>();
            foreach (var horse in horses)
            {
                var lungerId = GetExistingLunger(horse.Lunger)?.LungerId;

                var existingHorse = GetExistingHorse(horse);
                if (existingHorse != null)
                {
                    if (NotEqual(horse, existingHorse))
                    {
                        existingHorse.HorseName = horse.HorseName;
                        existingHorse.HorseTdbId = horse.HorseTdbId;
                        existingHorse.LungerId = lungerId; 
                        updatedHorses.Add(existingHorse);
                    }
                }
                else
                {
                    horse.LungerId = lungerId;
                    horse.Lunger = null;
                    newHorses.Add(horse);
                }
            }

            //updatedHorses = SetLungerDatabaseId(updatedHorses);
            ContestService.UpdateHorses(updatedHorses.ToArray());

            //newHorses = SetLungerDatabaseId(newHorses);
            ContestService.AddHorses(newHorses.ToArray());

        }

        //private static List<Horse> SetLungerDatabaseId(List<Horse> newHorses)
        //{
        //    foreach (var newHorse in newHorses)
        //    {
        //        var lungerTdbId = newHorse.Lunger.LungerTdbId;
        //        var lunger = ContestService.GetLunger(lungerTdbId);
        //        newHorse.Lunger.LungerId = lunger.LungerId;
        //    }

        //    return newHorses;
        //}

        //private static List<Vaulter> SetClassDatabaseId(List<Vaulter> newVaulters)
        //{
        //    foreach (var newVaulter in newVaulters)
        //    {
        //        if (newVaulter.VaultingClass == null)
        //        {
        //            continue;
        //        }
        //        var classTdbId = newVaulter.VaultingClass.ClassTdbId;
        //        var competitionClass = ContestService.GetClass(classTdbId);
        //        newVaulter.VaultingClass.CompetitionClassId = competitionClass.CompetitionClassId;
        //    }

        //    return newVaulters;
        //}

        //private static List<Vaulter> SetClubDatabaseId(List<Vaulter> newVaulters)
        //{
        //    foreach (var newVaulter in newVaulters)
        //    {
        //        if (newVaulter.VaultingClub == null)
        //        {
        //            continue;
        //        }
        //        var clubTdbId = newVaulter.VaultingClub.ClubTdbId;
        //        var club = ContestService.GetClub(clubTdbId);
        //        newVaulter.VaultingClub.ClubId = club.ClubId;
        //    }

        //    return newVaulters;
        //}

        private static bool NotEqual(Lunger lunger, Lunger existingLunger)
        {
            return existingLunger.LungerName != lunger.LungerName ||
                                    existingLunger.LungerTdbId != lunger.LungerTdbId;
        }

        private static bool NotEqual(TeamMember teamMember, TeamList existingTeamMember)
        {
           // var teamMemberVaulterInfo = GetExistingVaulter(teamMember.VaulterTdbId, teamMember.VaulterName);

            return teamMember.StartNumber != existingTeamMember.StartNumber;

            //     ||                   teamMemberVaulterInfo.VaulterId != existingTeamMember.ParticipantId;
        }

        private static bool NotEqual(Team team, Team existingTeam)
        {
            var vaulterListNotEqual = false;
            //var vaulterListNotEqual = team.TeamList[0].ParticipantId != existingTeam.TeamList[0].ParticipantId;
            //vaulterListNotEqual = vaulterListNotEqual || team.TeamList[1].ParticipantId != existingTeam.TeamList[1].ParticipantId;
            //vaulterListNotEqual = vaulterListNotEqual || team.TeamList[2].ParticipantId != existingTeam.TeamList[2].ParticipantId;
            //vaulterListNotEqual = vaulterListNotEqual || team.TeamList[3].ParticipantId != existingTeam.TeamList[3].ParticipantId;
            //vaulterListNotEqual = vaulterListNotEqual || team.TeamList[4].ParticipantId != existingTeam.TeamList[4].ParticipantId;
            //vaulterListNotEqual = vaulterListNotEqual || team.TeamList[5].ParticipantId != existingTeam.TeamList[5].ParticipantId;


            return vaulterListNotEqual || (team.VaultingClubId != null && existingTeam.VaultingClubId != team.VaultingClubId) ||
                   team.VaultingClassId != existingTeam.VaultingClassId;
        }

        private static bool NotEqual(CompetitionClass competitionClass, CompetitionClass existingClass)
        {
            return existingClass.ClassName != competitionClass.ClassName ||
                   existingClass.ClassTdbId != competitionClass.ClassTdbId;
        }

        private static bool NotEqual(Vaulter vaulter, Vaulter existingVaulter)
        {
            return vaulter.Name != existingVaulter.Name ||
                   vaulter.VaulterTdbId != existingVaulter.VaulterTdbId|| (
                   vaulter.VaultingClassId != existingVaulter.VaultingClassId)||
                   (vaulter.VaultingClub != null && vaulter.VaultingClub?.ClubTdbId != existingVaulter.VaultingClub?.ClubTdbId);
        }

        private static bool NotEqual(Club club, Club existingClub)
        {
            return existingClub.ClubName != club.ClubName ||
                   existingClub.ClubTdbId != club.ClubTdbId;
        }

        private static bool NotEqual(Horse horse, Horse existingHorse)
        {
            return horse.HorseName != existingHorse.HorseName ||
                   horse.HorseTdbId != existingHorse.HorseTdbId||
                   horse.Lunger.LungerTdbId != existingHorse.Lunger.LungerTdbId;
        }

        private static bool NotEqual(HorseOrder horseOrder, HorseOrder existingHorseOrder)
        {
            return horseOrder.StartNumber != existingHorseOrder.StartNumber;
        }

        private static TeamList GetExistingTeamMember(int teamId, int vaulterId)
        {
            //var vaulter = ContestService.GetVaulter(vaulterTdbId);
            //if(vaulter == null)
            //    return null;

            //var vaulterId = vaulter.VaulterId;
            var existingMember = ContestService.GetTeamMember(teamId, vaulterId);

            return existingMember;
        }
        private static Team GetExistingTeam(Team team)
        {

            var teamName = team.Name?.Trim();
            return GetExistingTeam(teamName);
        }

        private static Team GetExistingTeam(string teamName)
        {
            return ContestService.GetTeam(teamName);
        }

        //private static List<TeamList> GetExistingTeamMembers(Team team)
        //{
        //    var teamId = team.TeamId;
        //    return ContestService.GetTeamMembers(teamId);
        //}

        private static Horse GetExistingHorse(Horse horse)
        {

            var lungerTdbId = horse.Lunger.LungerTdbId;
            var existingHorse = ContestService.GetHorse(horse.HorseTdbId, lungerTdbId);
            if (existingHorse != null)
            {
                return existingHorse;
            }
            var horseName = horse.HorseName?.Trim();
            existingHorse = ContestService.GetHorse(horseName, lungerTdbId);

            return existingHorse;
        }

        private static VaulterOrder GetExistingVaulterOrder( int[] horseOrderIds, VaulterOrder importedVaulterOrder) 
        {
            var vaulterId = importedVaulterOrder.VaulterId;
            if (vaulterId == null)
                return null;

            var testNumber = importedVaulterOrder.Testnumber;

            return ContestService.GetVaulterOrder(horseOrderIds, (int) vaulterId, testNumber);
        }

        private static Lunger GetExistingLunger(Lunger lunger)
        {
            var existingLunger = ContestService.GetLunger(lunger.LungerTdbId);
            if (existingLunger != null)
            {
                return existingLunger;
            }
            var lungerName = lunger.LungerName;
            existingLunger = ContestService.GetLunger(lungerName);
                //if (existingLunger != null)
                //    if (lungerName != existingLunger.LungerName ||
                //        lunger.LungerTdbId != existingLunger.LungerTdbId)
                //    {
                //        var updatedLunger = new Lunger() {LungerName = lungerName, LungerTdbId = lunger.LungerTdbId};

                //    }
            return existingLunger;
        }

        private static HorseOrder GetExistingHorseOrderTeam(HorseOrder horseOrder)
        {
            var startlistClassStepId = horseOrder.StartListClassStepId;
            var horseId = GetExistingHorse(horseOrder.HorseInformation)?.HorseId;
            var vaultingTeamId = GetExistingTeam(horseOrder.VaultingTeam.Name)?.TeamId;
            var existingHorseOrder = ContestService.GetHorseOrder(startlistClassStepId, horseId, vaultingTeamId);
           
            return existingHorseOrder;
           
        }

        private static HorseOrder[] GetExistingHorseOrderIndividual(HorseOrder horseOrder)
        {
            var startlistClassStepId = horseOrder.StartListClassStepId;
            var horseId = GetExistingHorse(horseOrder.HorseInformation)?.HorseId;
//            var vaultingTeamId = GetExistingHorseOrderIndivual(horseOrder.VaultingTeam.Name)?.TeamId;
            var existingHorseOrders = ContestService.GetHorseOrder(startlistClassStepId, horseId);

            return existingHorseOrders;
        }

        private static Club GetExistingClub(Club club)
        {
            if (club == null)
            {
                return null;
            }
            var existingClub = ContestService.GetClub(club.ClubTdbId);
            if (existingClub != null)
            {
                return existingClub;
            }
            var clubName = club.ClubName;
            existingClub = ContestService.GetClub(clubName);
            //if (existingLunger != null)
            //    if (lungerName != existingLunger.LungerName ||
            //        lunger.LungerTdbId != existingLunger.LungerTdbId)
            //    {
            //        var updatedLunger = new Lunger() {LungerName = lungerName, LungerTdbId = lunger.LungerTdbId};

            //    }
            return existingClub;
        }

        private static CompetitionClass GetExistingClass(CompetitionClass competitionClass)
        {
            if (competitionClass == null)
            {
                return null;
            }

            var existingClass = ContestService.GetClass(competitionClass.ClassTdbId);
            if (existingClass != null)
            {
                return existingClass;
            }
            var className = competitionClass.ClassName;
            existingClass = ContestService.GetClass(className);
           
            return existingClass;
        }

        private static Vaulter GetExistingVaulter(Vaulter vaulter)
        {
            var vaulterTdbId = vaulter.VaulterTdbId;
            var vaulterName = vaulter.Name;

            return GetExistingVaulter(vaulterTdbId, vaulterName);
        }

        private static Vaulter GetExistingVaulter( int vaulterTdbId, string vaulterName)
        {
            var existingVaulter = ContestService.GetVaulter(vaulterTdbId);
            if (existingVaulter != null)
            {
                return existingVaulter;
            }
            existingVaulter = ContestService.GetVaulter(vaulterName.Trim());

            if (existingVaulter == null || existingVaulter.VaulterTdbId > 0) // en annan voltigör med samma namn
            {
                return null;
            }

            return existingVaulter;
        }

       

       
      
    }
}