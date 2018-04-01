using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Classes;
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
                if (existingVaulter != null)
                {
                    if (NotEqual(vaulter, existingVaulter))
                    {
                        existingVaulter.Name = vaulter.Name.Trim();
                        existingVaulter.VaulterTdbId = vaulter.VaulterTdbId;

                        if (vaulter.VaultingClass != null && existingVaulter.VaultingClass?.ClassTdbId != vaulter.VaultingClass?.ClassTdbId)
                        {
                            existingVaulter.VaultingClass = GetExistingClass(vaulter.VaultingClass);
                        }
                        if (vaulter.VaultingClub != null && existingVaulter.VaultingClub?.ClubTdbId != vaulter.VaultingClub?.ClubTdbId)
                        {
                            existingVaulter.VaultingClub =  GetExistingClub(vaulter.VaultingClub);
                        }
                        updatetdVaulters.Add(existingVaulter);
                    }
                }
                else
                {
                    newVaulters.Add(vaulter);
                }
            }

            //updatetdVaulters = SetClassDatabaseId(updatetdVaulters);
            //updatetdVaulters = SetClubDatabaseId(updatetdVaulters);
            ContestService.UpdateVaulters(updatetdVaulters.ToArray());
            newVaulters = SetClassDatabaseId(newVaulters);
            newVaulters = SetClubDatabaseId(newVaulters);
            ContestService.AddVaulters(newVaulters.ToArray());

        }

        public void UpdateHorses(Horse[] horses)
        {
            var newHorses = new List<Horse>();
            var updatedHorses = new List<Horse>();
            foreach (var horse in horses)
            {
                var existingHorse = GetExistingHorse(horse);
                if (existingHorse != null)
                {
                    if (NotEqual(horse, existingHorse))
                    {
                        existingHorse.HorseName = horse.HorseName;
                        existingHorse.HorseTdbId = horse.HorseTdbId;
                        existingHorse.Lunger = GetExistingLunger(horse.Lunger); 
                        updatedHorses.Add(existingHorse);
                    }
                }
                else
                {

                    newHorses.Add(horse);
                }
            }

            //updatedHorses = SetLungerDatabaseId(updatedHorses);
            ContestService.UpdateHorses(updatedHorses.ToArray());

            newHorses = SetLungerDatabaseId(newHorses);
            ContestService.AddHorses(newHorses.ToArray());

        }

        private static List<Horse> SetLungerDatabaseId(List<Horse> newHorses)
        {
            foreach (var newHorse in newHorses)
            {
                var lungerTdbId = newHorse.Lunger.LungerTdbId;
                var lunger = ContestService.GetLunger(lungerTdbId);
                newHorse.Lunger.LungerId = lunger.LungerId;
            }

            return newHorses;
        }

        private static List<Vaulter> SetClassDatabaseId(List<Vaulter> newVaulters)
        {
            foreach (var newVaulter in newVaulters)
            {
                if (newVaulter.VaultingClass == null)
                {
                    continue;
                }
                var classTdbId = newVaulter.VaultingClass.ClassTdbId;
                var competitionClass = ContestService.GetClass(classTdbId);
                newVaulter.VaultingClass.CompetitionClassId = competitionClass.CompetitionClassId;
            }

            return newVaulters;
        }

        private static List<Vaulter> SetClubDatabaseId(List<Vaulter> newVaulters)
        {
            foreach (var newVaulter in newVaulters)
            {
                if (newVaulter.VaultingClub == null)
                {
                    continue;
                }
                var clubTdbId = newVaulter.VaultingClub.ClubTdbId;
                var club = ContestService.GetClub(clubTdbId);
                newVaulter.VaultingClub.ClubId = club.ClubId;
            }

            return newVaulters;
        }

        private static bool NotEqual(Lunger lunger, Lunger existingLunger)
        {
            return existingLunger.LungerName != lunger.LungerName ||
                                    existingLunger.LungerTdbId != lunger.LungerTdbId;
        }

        private static bool NotEqual(CompetitionClass competitionClass, CompetitionClass existingClass)
        {
            return existingClass.ClassName != competitionClass.ClassName ||
                   existingClass.ClassTdbId != competitionClass.ClassTdbId;
        }

        private static bool NotEqual(Vaulter vaulter, Vaulter existingVaulter)
        {
            return vaulter.Name != existingVaulter.Name ||
                   vaulter.VaulterTdbId != existingVaulter.VaulterTdbId|| (vaulter.VaultingClass != null &&
                   vaulter.VaultingClass?.ClassTdbId != existingVaulter.VaultingClass?.ClassTdbId)||
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

            //if (existingLunger != null)
            //    if (lungerName != existingLunger.LungerName ||
            //        lunger.LungerTdbId != existingLunger.LungerTdbId)
            //    {
            //        var updatedLunger = new Lunger() {LungerName = lungerName, LungerTdbId = lunger.LungerTdbId};

            //    }
            return existingHorse;
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

            var existingClub = ContestService.GetClass(competitionClass.ClassTdbId);
            if (existingClub != null)
            {
                return existingClub;
            }
            var className = competitionClass.ClassName;
            existingClub = ContestService.GetClass(className);
            //if (existingLunger != null)
            //    if (lungerName != existingLunger.LungerName ||
            //        lunger.LungerTdbId != existingLunger.LungerTdbId)
            //    {
            //        var updatedLunger = new Lunger() {LungerName = lungerName, LungerTdbId = lunger.LungerTdbId};

            //    }
            return existingClub;
        }

        private static Vaulter GetExistingVaulter(Vaulter vaulter)
        {
            var existingVaulter = ContestService.GetVaulter(vaulter.VaulterTdbId);
            if (existingVaulter != null)
            {
                return existingVaulter;
            }
            var vaulterName = vaulter.Name.Trim();
            existingVaulter = ContestService.GetVaulter(vaulterName);

            if (existingVaulter == null || existingVaulter.VaulterTdbId > 0) // en annan voltigör med samma namn
            {
                return null;
            }

            return existingVaulter;
        }

       

       
      
    }
}