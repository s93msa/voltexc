namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScoreSheetId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompetitionClasses", "ScoreSheet_ScoreSheetsId", "dbo.ScoreSheets");
            DropIndex("dbo.CompetitionClasses", new[] { "ScoreSheet_ScoreSheetsId" });
            RenameColumn(table: "dbo.CompetitionClasses", name: "ScoreSheet_ScoreSheetsId", newName: "ScoreSheetId");
            AlterColumn("dbo.CompetitionClasses", "ScoreSheetId", c => c.Int(nullable: false));
            CreateIndex("dbo.CompetitionClasses", "ScoreSheetId");
            AddForeignKey("dbo.CompetitionClasses", "ScoreSheetId", "dbo.ScoreSheets", "ScoreSheetsId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompetitionClasses", "ScoreSheetId", "dbo.ScoreSheets");
            DropIndex("dbo.CompetitionClasses", new[] { "ScoreSheetId" });
            AlterColumn("dbo.CompetitionClasses", "ScoreSheetId", c => c.Int());
            RenameColumn(table: "dbo.CompetitionClasses", name: "ScoreSheetId", newName: "ScoreSheet_ScoreSheetsId");
            CreateIndex("dbo.CompetitionClasses", "ScoreSheet_ScoreSheetsId");
            AddForeignKey("dbo.CompetitionClasses", "ScoreSheet_ScoreSheetsId", "dbo.ScoreSheets", "ScoreSheetsId");
        }
    }
}
