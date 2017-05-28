namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedStartlistToHorseOrder : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StartLists", newName: "HorseOrders");
            DropForeignKey("dbo.VaulterOrders", "StartList_StartListId", "dbo.StartLists");
            RenameColumn(table: "dbo.VaulterOrders", name: "StartList_StartListId", newName: "HorseOrder_HorseOrderId");
            RenameIndex(table: "dbo.VaulterOrders", name: "IX_StartList_StartListId", newName: "IX_HorseOrder_HorseOrderId");
            DropPrimaryKey("dbo.HorseOrders");
            DropColumn("dbo.HorseOrders", "StartListId");

            AddColumn("dbo.HorseOrders", "HorseOrderId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.HorseOrders", "HorseOrderId");
            AddForeignKey("dbo.VaulterOrders", "HorseOrder_HorseOrderId", "dbo.HorseOrders", "HorseOrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HorseOrders", "StartListId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.VaulterOrders", "HorseOrder_HorseOrderId", "dbo.HorseOrders");
            DropPrimaryKey("dbo.HorseOrders");
            DropColumn("dbo.HorseOrders", "HorseOrderId");
            AddPrimaryKey("dbo.HorseOrders", "StartListId");
            RenameIndex(table: "dbo.VaulterOrders", name: "IX_HorseOrder_HorseOrderId", newName: "IX_StartList_StartListId");
            RenameColumn(table: "dbo.VaulterOrders", name: "HorseOrder_HorseOrderId", newName: "StartList_StartListId");
            AddForeignKey("dbo.VaulterOrders", "StartList_StartListId", "dbo.StartLists", "StartListId");
            RenameTable(name: "dbo.HorseOrders", newName: "StartLists");
        }
    }
}
