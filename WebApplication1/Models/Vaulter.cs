namespace WebApplication1.Models
{
    public class Vaulter
    {
        public int VaulterId { get; set; }
        public string Name { get; set; }
        public Club VaultingClub { get; set; }

        public string Armband { get; set; }

        public CompetitionClass VaultingClass { get; set; }


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