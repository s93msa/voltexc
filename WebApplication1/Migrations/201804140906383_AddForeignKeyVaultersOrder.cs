namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyVaultersOrder : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.VaulterOrders", name: "Participant_VaulterId", newName: "VaulterId");
            RenameIndex(table: "dbo.VaulterOrders", name: "IX_Participant_VaulterId", newName: "IX_VaulterId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.VaulterOrders", name: "IX_VaulterId", newName: "IX_Participant_VaulterId");
            RenameColumn(table: "dbo.VaulterOrders", name: "VaulterId", newName: "Participant_VaulterId");
        }
    }
}
