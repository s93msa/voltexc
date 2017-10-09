namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overrideexcelfilename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Steps", "OverrideExcelfileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Steps", "OverrideExcelfileName");
        }
    }
}
