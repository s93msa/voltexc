namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VaulterOrderForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.VaulterOrders", name: "HorseOrder_HorseOrderId", newName: "HorseOrderId");
            RenameIndex(table: "dbo.VaulterOrders", name: "IX_HorseOrder_HorseOrderId", newName: "IX_HorseOrderId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.VaulterOrders", name: "IX_HorseOrderId", newName: "IX_HorseOrder_HorseOrderId");
            RenameColumn(table: "dbo.VaulterOrders", name: "HorseOrderId", newName: "HorseOrder_HorseOrderId");
        }
    }
}
