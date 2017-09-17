namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContestType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContestTypes",
                c => new
                    {
                        ContestTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ContestTypeId);
            
            AddColumn("dbo.Steps", "TypeOfContest_ContestTypeId", c => c.Int());
            AddColumn("dbo.Contests", "TypeOfContest_ContestTypeId", c => c.Int());
            CreateIndex("dbo.Steps", "TypeOfContest_ContestTypeId");
            CreateIndex("dbo.Contests", "TypeOfContest_ContestTypeId");
            AddForeignKey("dbo.Steps", "TypeOfContest_ContestTypeId", "dbo.ContestTypes", "ContestTypeId");
            AddForeignKey("dbo.Contests", "TypeOfContest_ContestTypeId", "dbo.ContestTypes", "ContestTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contests", "TypeOfContest_ContestTypeId", "dbo.ContestTypes");
            DropForeignKey("dbo.Steps", "TypeOfContest_ContestTypeId", "dbo.ContestTypes");
            DropIndex("dbo.Contests", new[] { "TypeOfContest_ContestTypeId" });
            DropIndex("dbo.Steps", new[] { "TypeOfContest_ContestTypeId" });
            DropColumn("dbo.Contests", "TypeOfContest_ContestTypeId");
            DropColumn("dbo.Steps", "TypeOfContest_ContestTypeId");
            DropTable("dbo.ContestTypes");
        }
    }
}
