namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyFieldInSteps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Steps", "TypeOfContestId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Steps", "TypeOfContestId");
        }
    }
}
