namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExcelInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompetitionClasses", "Excelfile", c => c.String());
            AddColumn("dbo.Steps", "TestNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Steps", "ExcelWorksheetNameJudgesTableA", c => c.String());
            AddColumn("dbo.Steps", "ExcelWorksheetNameJudgesTableB", c => c.String());
            AddColumn("dbo.Steps", "ExcelWorksheetNameJudgesTableC", c => c.String());
            AddColumn("dbo.Steps", "ExcelWorksheetNameJudgesTableD", c => c.String());
            AddColumn("dbo.StartLists", "TestNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StartLists", "TestNumber");
            DropColumn("dbo.Steps", "ExcelWorksheetNameJudgesTableD");
            DropColumn("dbo.Steps", "ExcelWorksheetNameJudgesTableC");
            DropColumn("dbo.Steps", "ExcelWorksheetNameJudgesTableB");
            DropColumn("dbo.Steps", "ExcelWorksheetNameJudgesTableA");
            DropColumn("dbo.Steps", "TestNumber");
            DropColumn("dbo.CompetitionClasses", "Excelfile");
        }
    }
}
