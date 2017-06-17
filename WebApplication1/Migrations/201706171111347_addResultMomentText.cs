namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addResultMomentText : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Steps", "ResultMomentText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Steps", "ResultMomentText");
        }
    }
}
