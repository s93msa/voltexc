using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ClosedXML.Excel;
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
            var filteredRows = mergedInfo.Where(x => !x.isTeam && x.VaulterId1 == vaulterTdbId).ToArray();

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
            var filteredRows = mergedInfo.Where(x => !x.isTeam && x.VaulterId1 == vaulterTdbId).ToArray();

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


