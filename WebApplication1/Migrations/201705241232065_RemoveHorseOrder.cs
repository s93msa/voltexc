namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveHorseOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HorseOrders", "HorseInformation_HorseId", "dbo.Horses");
            DropForeignKey("dbo.VaulterOrders", "HorseOrder_HorseOrderId", "dbo.HorseOrders");
            DropForeignKey("dbo.StartLists", "Horse_HorseOrderId", "dbo.HorseOrders");
            DropIndex("dbo.StartLists", new[] { "Horse_HorseOrderId" });
            DropIndex("dbo.HorseOrders", new[] { "HorseInformation_HorseId" });
            DropIndex("dbo.VaulterOrders", new[] { "HorseOrder_HorseOrderId" });
            AddColumn("dbo.StartLists", "IsTeam", c => c.Boolean(nullable: false));
            AddColumn("dbo.StartLists", "HorseInformation_HorseId", c => c.Int());
            AddColumn("dbo.StartLists", "VaultingTeam_TeamId", c => c.Int());
            AddColumn("dbo.VaulterOrders", "StartList_StartListId", c => c.Int());
            CreateIndex("dbo.StartLists", "HorseInformation_HorseId");
            CreateIndex("dbo.StartLists", "VaultingTeam_TeamId");
            CreateIndex("dbo.VaulterOrders", "StartList_StartListId");
            AddForeignKey("dbo.StartLists", "HorseInformation_HorseId", "dbo.Horses", "HorseId");
            AddForeignKey("dbo.VaulterOrders", "StartList_StartListId", "dbo.StartLists", "StartListId");
            AddForeignKey("dbo.StartLists", "VaultingTeam_TeamId", "dbo.Teams", "TeamId");
            DropColumn("dbo.StartLists", "Horse_HorseOrderId");
            DropColumn("dbo.VaulterOrders", "HorseOrder_HorseOrderId");
            DropTable("dbo.HorseOrders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HorseOrders",
                c => new
                    {
                        HorseOrderId = c.Int(nullable: false, identity: true),
                        StartOrder = c.Int(nullable: false),
                        IsTeam = c.Boolean(nullable: false),
                        HorseInformation_HorseId = c.Int(),
                    })
                .PrimaryKey(t => t.HorseOrderId);
            
            AddColumn("dbo.VaulterOrders", "HorseOrder_HorseOrderId", c => c.Int());
            AddColumn("dbo.StartLists", "Horse_HorseOrderId", c => c.Int());
            DropForeignKey("dbo.StartLists", "VaultingTeam_TeamId", "dbo.Teams");
            DropForeignKey("dbo.VaulterOrders", "StartList_StartListId", "dbo.StartLists");
            DropForeignKey("dbo.StartLists", "HorseInformation_HorseId", "dbo.Horses");
            DropIndex("dbo.VaulterOrders", new[] { "StartList_StartListId" });
            DropIndex("dbo.StartLists", new[] { "VaultingTeam_TeamId" });
            DropIndex("dbo.StartLists", new[] { "HorseInformation_HorseId" });
            DropColumn("dbo.VaulterOrders", "StartList_StartListId");
            DropColumn("dbo.StartLists", "VaultingTeam_TeamId");
            DropColumn("dbo.StartLists", "HorseInformation_HorseId");
            DropColumn("dbo.StartLists", "IsTeam");
            CreateIndex("dbo.VaulterOrders", "HorseOrder_HorseOrderId");
            CreateIndex("dbo.HorseOrders", "HorseInformation_HorseId");
            CreateIndex("dbo.StartLists", "Horse_HorseOrderId");
            AddForeignKey("dbo.StartLists", "Horse_HorseOrderId", "dbo.HorseOrders", "HorseOrderId");
            AddForeignKey("dbo.VaulterOrders", "HorseOrder_HorseOrderId", "dbo.HorseOrders", "HorseOrderId");
            AddForeignKey("dbo.HorseOrders", "HorseInformation_HorseId", "dbo.Horses", "HorseId");
        }
    }
}
