namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScoreSheets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScoreSheets",
                c => new
                    {
                        ScoreSheetsId = c.Int(nullable: false, identity: true),
                        NameOfType = c.String(),
                        Excelfile = c.String(),
                    })
                .PrimaryKey(t => t.ScoreSheetsId);
            
            AddColumn("dbo.CompetitionClasses", "ScoreSheet_ScoreSheetsId", c => c.Int());
            AddColumn("dbo.Steps", "ScoreSheets_ScoreSheetsId", c => c.Int());
            CreateIndex("dbo.CompetitionClasses", "ScoreSheet_ScoreSheetsId");
            CreateIndex("dbo.Steps", "ScoreSheets_ScoreSheetsId");
            AddForeignKey("dbo.Steps", "ScoreSheets_ScoreSheetsId", "dbo.ScoreSheets", "ScoreSheetsId");
            AddForeignKey("dbo.CompetitionClasses", "ScoreSheet_ScoreSheetsId", "dbo.ScoreSheets", "ScoreSheetsId");
            DropColumn("dbo.CompetitionClasses", "Excelfile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompetitionClasses", "Excelfile", c => c.String());
            DropForeignKey("dbo.CompetitionClasses", "ScoreSheet_ScoreSheetsId", "dbo.ScoreSheets");
            DropForeignKey("dbo.Steps", "ScoreSheets_ScoreSheetsId", "dbo.ScoreSheets");
            DropIndex("dbo.Steps", new[] { "ScoreSheets_ScoreSheetsId" });
            DropIndex("dbo.CompetitionClasses", new[] { "ScoreSheet_ScoreSheetsId" });
            DropColumn("dbo.Steps", "ScoreSheets_ScoreSheetsId");
            DropColumn("dbo.CompetitionClasses", "ScoreSheet_ScoreSheetsId");
            DropTable("dbo.ScoreSheets");
        }
    }
}
