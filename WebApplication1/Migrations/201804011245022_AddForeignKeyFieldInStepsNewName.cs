namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyFieldInStepsNewName : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Steps", "TypeOfContest_ContestTypeId", "dbo.ContestTypes");
            DropIndex("dbo.Steps", new[] { "TypeOfContest_ContestTypeId" });
            DropColumn("dbo.Steps", "TypeOfContestId");
            RenameColumn(table: "dbo.Steps", name: "TypeOfContest_ContestTypeId", newName: "TypeOfContestId");
            AlterColumn("dbo.Steps", "TypeOfContestId", c => c.Int(nullable: false));
            CreateIndex("dbo.Steps", "TypeOfContestId");
            AddForeignKey("dbo.Steps", "TypeOfContestId", "dbo.ContestTypes", "ContestTypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Steps", "TypeOfContestId", "dbo.ContestTypes");
            DropIndex("dbo.Steps", new[] { "TypeOfContestId" });
            AlterColumn("dbo.Steps", "TypeOfContestId", c => c.Int());
            RenameColumn(table: "dbo.Steps", name: "TypeOfContestId", newName: "TypeOfContest_ContestTypeId");
            AddColumn("dbo.Steps", "TypeOfContestId", c => c.Int(nullable: false));
            CreateIndex("dbo.Steps", "TypeOfContest_ContestTypeId");
            AddForeignKey("dbo.Steps", "TypeOfContest_ContestTypeId", "dbo.ContestTypes", "ContestTypeId");
        }
    }
}
