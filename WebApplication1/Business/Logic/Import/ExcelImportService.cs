using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ClosedXML.Excel;
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
            var horsesArray = _excelImportRepository.GetHorsesWorksheet();
            List<Horse> horses = new List<Horse>(horsesArray);
            horses = SetLungers(horses);
            return horses.ToArray();
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

        public ExcelImportMergedModel[] GetMergedInfo()
        {
            //TODO: cache
            return _excelImportRepository.GetMergedInfo();
        }


        
    }
}


