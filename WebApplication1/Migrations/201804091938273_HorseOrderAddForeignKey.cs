namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorseOrderAddForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.HorseOrders", name: "HorseInformation_HorseId", newName: "HorseId");
            RenameColumn(table: "dbo.HorseOrders", name: "VaultingTeam_TeamId", newName: "VaultingTeamId");
            RenameIndex(table: "dbo.HorseOrders", name: "IX_HorseInformation_HorseId", newName: "IX_HorseId");
            RenameIndex(table: "dbo.HorseOrders", name: "IX_VaultingTeam_TeamId", newName: "IX_VaultingTeamId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.HorseOrders", name: "IX_VaultingTeamId", newName: "IX_VaultingTeam_TeamId");
            RenameIndex(table: "dbo.HorseOrders", name: "IX_HorseId", newName: "IX_HorseInformation_HorseId");
            RenameColumn(table: "dbo.HorseOrders", name: "VaultingTeamId", newName: "VaultingTeam_TeamId");
            RenameColumn(table: "dbo.HorseOrders", name: "HorseId", newName: "HorseInformation_HorseId");
        }
    }
}
