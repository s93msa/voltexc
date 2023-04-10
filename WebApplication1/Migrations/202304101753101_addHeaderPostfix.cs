namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHeaderPostfix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScoreSheets", "ScoreSheetBaseId", c => c.Int());
            AddColumn("dbo.ScoreSheets", "HeaderPostfix", c => c.String());
            CreateIndex("dbo.ScoreSheets", "ScoreSheetBaseId");
            AddForeignKey("dbo.ScoreSheets", "ScoreSheetBaseId", "dbo.ScoreSheets", "ScoreSheetsId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScoreSheets", "ScoreSheetBaseId", "dbo.ScoreSheets");
            DropIndex("dbo.ScoreSheets", new[] { "ScoreSheetBaseId" });
            DropColumn("dbo.ScoreSheets", "HeaderPostfix");
            DropColumn("dbo.ScoreSheets", "ScoreSheetBaseId");
        }
    }
}
