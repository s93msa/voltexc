namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyFieldInVaulter : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vaulters", "VaultingClass_CompetitionClassId", "dbo.CompetitionClasses");
            DropIndex("dbo.Vaulters", new[] { "VaultingClass_CompetitionClassId" });
            RenameColumn(table: "dbo.Vaulters", name: "VaultingClass_CompetitionClassId", newName: "VaultingClassId");
            AlterColumn("dbo.Vaulters", "VaultingClassId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vaulters", "VaultingClassId");
            AddForeignKey("dbo.Vaulters", "VaultingClassId", "dbo.CompetitionClasses", "CompetitionClassId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaulters", "VaultingClassId", "dbo.CompetitionClasses");
            DropIndex("dbo.Vaulters", new[] { "VaultingClassId" });
            AlterColumn("dbo.Vaulters", "VaultingClassId", c => c.Int());
            RenameColumn(table: "dbo.Vaulters", name: "VaultingClassId", newName: "VaultingClass_CompetitionClassId");
            CreateIndex("dbo.Vaulters", "VaultingClass_CompetitionClassId");
            AddForeignKey("dbo.Vaulters", "VaultingClass_CompetitionClassId", "dbo.CompetitionClasses", "CompetitionClassId");
        }
    }
}
