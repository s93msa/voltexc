namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompetitionClassToVaulter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vaulters", "VaultingClass_CompetitionClassId", c => c.Int());
            CreateIndex("dbo.Vaulters", "VaultingClass_CompetitionClassId");
            AddForeignKey("dbo.Vaulters", "VaultingClass_CompetitionClassId", "dbo.CompetitionClasses", "CompetitionClassId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaulters", "VaultingClass_CompetitionClassId", "dbo.CompetitionClasses");
            DropIndex("dbo.Vaulters", new[] { "VaultingClass_CompetitionClassId" });
            DropColumn("dbo.Vaulters", "VaultingClass_CompetitionClassId");
        }
    }
}
