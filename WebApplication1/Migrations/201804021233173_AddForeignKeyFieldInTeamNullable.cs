namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyFieldInTeamNullable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Teams", name: "VaultingClass_CompetitionClassId", newName: "VaultingClassId");
            RenameIndex(table: "dbo.Teams", name: "IX_VaultingClass_CompetitionClassId", newName: "IX_VaultingClassId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Teams", name: "IX_VaultingClassId", newName: "IX_VaultingClass_CompetitionClassId");
            RenameColumn(table: "dbo.Teams", name: "VaultingClassId", newName: "VaultingClass_CompetitionClassId");
        }
    }
}
