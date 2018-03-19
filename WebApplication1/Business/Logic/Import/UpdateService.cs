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
                        updatedHorses.Add(existingHorse);
                    }
                }
                else
                {

                    newHorses.Add(horse);
                }
            }

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

        private static bool NotEqual(Lunger lunger, Lunger existingLunger)
        {
            return existingLunger.LungerName != lunger.LungerName ||
                                    existingLunger.LungerTdbId != lunger.LungerTdbId;
        }

        private static bool NotEqual(Club club, Club existingClub)
        {
            return existingClub.ClubName != club.ClubName ||
                   existingClub.ClubTdbId != club.ClubTdbId;
        }

        private static bool NotEqual(Horse horse, Horse existingHorse)
        {
            return horse.HorseName != existingHorse.HorseName ||
                   horse.HorseTdbId != existingHorse.HorseTdbId;
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


        void AddClubs(string[] clubs)
        {
            
        }

        void UpdateClasses(CompetitionClass[] competitionClasses)
        {

        }
        void UpdateVaulters(Vaulter[] vaulters)
        {

        }
    }
}