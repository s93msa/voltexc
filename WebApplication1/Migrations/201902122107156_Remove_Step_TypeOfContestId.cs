namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Step_TypeOfContestId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Steps", "TypeOfContestId", "dbo.ContestTypes");
            DropIndex("dbo.Steps", new[] { "TypeOfContestId" });
            DropColumn("dbo.Steps", "TypeOfContestId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Steps", "TypeOfContestId", c => c.Int(nullable: false));
            CreateIndex("dbo.Steps", "TypeOfContestId");
            AddForeignKey("dbo.Steps", "TypeOfContestId", "dbo.ContestTypes", "ContestTypeId", cascadeDelete: true);
        }
    }
}
