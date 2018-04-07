namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClubForeignKeyFieldInTeam : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Teams", name: "VaultingClub_ClubId", newName: "VaultingClubId");
            RenameIndex(table: "dbo.Teams", name: "IX_VaultingClub_ClubId", newName: "IX_VaultingClubId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Teams", name: "IX_VaultingClubId", newName: "IX_VaultingClub_ClubId");
            RenameColumn(table: "dbo.Teams", name: "VaultingClubId", newName: "VaultingClub_ClubId");
        }
    }
}
