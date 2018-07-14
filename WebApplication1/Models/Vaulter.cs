using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Vaulter
    {
        public int VaulterId { get; set; }
        public string Name { get; set; }

        public int? VaultingClubId { get; set; }
        [ForeignKey("VaultingClubId")]
        public virtual Club VaultingClub { get; set; }

        public string Armband { get; set; }

        public int? VaultingClassId { get; set; }
        [ForeignKey("VaultingClassId")]

        public virtual CompetitionClass VaultingClass { get; set; }

        [Index]
        public int VaulterTdbId { get; set; }


        //public bool Active { get; set; }

        //private readonly CompetitionClass _competitionClass;
        //public Vaulter(CompetitionClass competitionClass)
        //{
        //    _competitionClass = competitionClass;
        //}

        //public CompetitionClass GetCompetitionClassName()
        //{
        //    return _competitionClass;
        //}


    }
}