namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_StepsToScoreSheet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Steps", "CompetitionClass_CompetitionClassId", "dbo.CompetitionClasses");
            DropIndex("dbo.Steps", new[] { "CompetitionClass_CompetitionClassId" });
            DropColumn("dbo.Steps", "CompetitionClass_CompetitionClassId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Steps", "CompetitionClass_CompetitionClassId", c => c.Int());
            CreateIndex("dbo.Steps", "CompetitionClass_CompetitionClassId");
            AddForeignKey("dbo.Steps", "CompetitionClass_CompetitionClassId", "dbo.CompetitionClasses", "CompetitionClassId");
        }
    }
}
