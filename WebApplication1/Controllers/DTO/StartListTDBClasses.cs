namespace VoltigeCore.Controllers.DTO
{
    public class StartListTdbClasses
    {
        public int[] CompetitionClassesTdbIds;
        public StepMoment[] StepMoment;
    }

    public struct StepMoment
    {
        public int StartListClassStepId;
        public int TestNumber;
    }
}
