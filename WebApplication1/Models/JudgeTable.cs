namespace WebApplication1.Models
{
    public enum JudgeTableNames { A = 1,B,C,D };
    public class JudgeTable
    {
        public int JudgeTableId { get; set; }
        public virtual JudgeTableNames JudgeTableName { get; set; }
        public string JudgeName { get; set; }

        //private JudgeTableNames _judge;

        //private const string OutputFolder = @"C:\Temp\Test_Voligemallar\output\";

        //public JudgeTable(JudgeTableNames judge)
        //{
        //    _judge = judge;
        //}

        //public void CreateFolder(string competitionClass)
        //{
        //    //TODO: Kontrollera om competitionClass foldern finns. Om inte skapa den. Skapa o så fall även Folder för domaren
        //    return;
        //}

        //public void CreateProtocolFor(Vaulter person)
        //{

        //}


    }
}