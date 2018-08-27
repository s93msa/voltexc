namespace WebApplication1.Models
{
    public enum JudgeTableNames {Okänd = 0, A = 1,B,C,D };
    public class JudgeTable
    {
        public int JudgeTableId { get; set; }
        public virtual JudgeTableNames JudgeTableName { get; set; }
        public string JudgeName { get; set; }

       


    }
}