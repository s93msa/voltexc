namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VaultersAddClubForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Vaulters", name: "VaultingClub_ClubId", newName: "VaultingClubId");
            RenameIndex(table: "dbo.Vaulters", name: "IX_VaultingClub_ClubId", newName: "IX_VaultingClubId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Vaulters", name: "IX_VaultingClubId", newName: "IX_VaultingClub_ClubId");
            RenameColumn(table: "dbo.Vaulters", name: "VaultingClubId", newName: "VaultingClub_ClubId");
        }
    }
}
