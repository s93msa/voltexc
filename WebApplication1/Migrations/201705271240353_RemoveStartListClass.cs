namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStartListClass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StartListClasses", "Contest_ContestId", "dbo.Contests");
            DropIndex("dbo.StartListClasses", new[] { "Contest_ContestId" });
            AddColumn("dbo.StartListClassSteps", "Contest_ContestId", c => c.Int());
            CreateIndex("dbo.StartListClassSteps", "Contest_ContestId");
            AddForeignKey("dbo.StartListClassSteps", "Contest_ContestId", "dbo.Contests", "ContestId");
            DropColumn("dbo.StartListClasses", "Contest_ContestId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StartListClasses", "Contest_ContestId", c => c.Int());
            DropForeignKey("dbo.StartListClassSteps", "Contest_ContestId", "dbo.Contests");
            DropIndex("dbo.StartListClassSteps", new[] { "Contest_ContestId" });
            DropColumn("dbo.StartListClassSteps", "Contest_ContestId");
            CreateIndex("dbo.StartListClasses", "Contest_ContestId");
            AddForeignKey("dbo.StartListClasses", "Contest_ContestId", "dbo.Contests", "ContestId");
        }
    }
}
