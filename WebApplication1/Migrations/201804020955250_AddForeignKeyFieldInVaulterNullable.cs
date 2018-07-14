namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyFieldInVaulterNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vaulters", "VaultingClassId", "dbo.CompetitionClasses");
            DropIndex("dbo.Vaulters", new[] { "VaultingClassId" });
            AlterColumn("dbo.Vaulters", "VaultingClassId", c => c.Int());
            CreateIndex("dbo.Vaulters", "VaultingClassId");
            AddForeignKey("dbo.Vaulters", "VaultingClassId", "dbo.CompetitionClasses", "CompetitionClassId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaulters", "VaultingClassId", "dbo.CompetitionClasses");
            DropIndex("dbo.Vaulters", new[] { "VaultingClassId" });
            AlterColumn("dbo.Vaulters", "VaultingClassId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vaulters", "VaultingClassId");
            AddForeignKey("dbo.Vaulters", "VaultingClassId", "dbo.CompetitionClasses", "CompetitionClassId", cascadeDelete: true);
        }
    }
}
