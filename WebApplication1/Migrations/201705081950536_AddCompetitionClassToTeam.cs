namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompetitionClassToTeam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "VaultingClass_CompetitionClassId", c => c.Int());
            CreateIndex("dbo.Teams", "VaultingClass_CompetitionClassId");
            AddForeignKey("dbo.Teams", "VaultingClass_CompetitionClassId", "dbo.CompetitionClasses", "CompetitionClassId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "VaultingClass_CompetitionClassId", "dbo.CompetitionClasses");
            DropIndex("dbo.Teams", new[] { "VaultingClass_CompetitionClassId" });
            DropColumn("dbo.Teams", "VaultingClass_CompetitionClassId");
        }
    }
}
