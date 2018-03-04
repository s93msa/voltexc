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

        void UpdateHorses(Horse[] horses)
        {
            

        }

        public void AddLungers(Lunger[] lungers)
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

        private static bool NotEqual(Lunger lunger, Lunger existingLunger)
        {
            return existingLunger.LungerName != lunger.LungerName ||
                                    existingLunger.LungerTdbId != lunger.LungerTdbId;
        }

        private static Lunger GetExistingLunger(Lunger lunger)
        {
            Lunger existingLunger;
            existingLunger = ContestService.GetLunger(lunger.LungerTdbId);
            if (existingLunger != null)
            {
                //compare if updated
                //update 
            }
            else
            {
                var lungerName = lunger.LungerName;
                existingLunger = ContestService.GetLunger(lungerName);
                //if (existingLunger != null)
                //    if (lungerName != existingLunger.LungerName ||
                //        lunger.LungerTdbId != existingLunger.LungerTdbId)
                //    {
                //        var updatedLunger = new Lunger() {LungerName = lungerName, LungerTdbId = lunger.LungerTdbId};

                //    }
            }
            return existingLunger;
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