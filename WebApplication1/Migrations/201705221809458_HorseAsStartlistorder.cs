namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorseAsStartlistorder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StartLists", "Participant_VaulterId", "dbo.Vaulters");
            DropForeignKey("dbo.StartLists", "VaultingHorse_HorseId", "dbo.Horses");
            DropForeignKey("dbo.StartLists", "VaultingTeam_TeamId", "dbo.Teams");
            DropIndex("dbo.StartLists", new[] { "Participant_VaulterId" });
            DropIndex("dbo.StartLists", new[] { "VaultingHorse_HorseId" });
            DropIndex("dbo.StartLists", new[] { "VaultingTeam_TeamId" });
            CreateTable(
                "dbo.HorseOrders",
                c => new
                    {
                        HorseOrderId = c.Int(nullable: false, identity: true),
                        StartOrder = c.Int(nullable: false),
                        IsTeam = c.Boolean(nullable: false),
                        HorseInformation_HorseId = c.Int(),
                    })
                .PrimaryKey(t => t.HorseOrderId)
                .ForeignKey("dbo.Horses", t => t.HorseInformation_HorseId)
                .Index(t => t.HorseInformation_HorseId);
            
            CreateTable(
                "dbo.VaulterOrders",
                c => new
                    {
                        VaulterOrderID = c.Int(nullable: false, identity: true),
                        Testnumber = c.Int(nullable: false),
                        Participant_VaulterId = c.Int(),
                        HorseOrder_HorseOrderId = c.Int(),
                    })
                .PrimaryKey(t => t.VaulterOrderID)
                .ForeignKey("dbo.Vaulters", t => t.Participant_VaulterId)
                .ForeignKey("dbo.HorseOrders", t => t.HorseOrder_HorseOrderId)
                .Index(t => t.Participant_VaulterId)
                .Index(t => t.HorseOrder_HorseOrderId);
            
            AddColumn("dbo.StartLists", "Horse_HorseOrderId", c => c.Int());
            CreateIndex("dbo.StartLists", "Horse_HorseOrderId");
            AddForeignKey("dbo.StartLists", "Horse_HorseOrderId", "dbo.HorseOrders", "HorseOrderId");
            DropColumn("dbo.StartLists", "TestNumber");
            DropColumn("dbo.StartLists", "Participant_VaulterId");
            DropColumn("dbo.StartLists", "VaultingHorse_HorseId");
            DropColumn("dbo.StartLists", "VaultingTeam_TeamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StartLists", "VaultingTeam_TeamId", c => c.Int());
            AddColumn("dbo.StartLists", "VaultingHorse_HorseId", c => c.Int());
            AddColumn("dbo.StartLists", "Participant_VaulterId", c => c.Int());
            AddColumn("dbo.StartLists", "TestNumber", c => c.Int(nullable: false));
            DropForeignKey("dbo.StartLists", "Horse_HorseOrderId", "dbo.HorseOrders");
            DropForeignKey("dbo.VaulterOrders", "HorseOrder_HorseOrderId", "dbo.HorseOrders");
            DropForeignKey("dbo.VaulterOrders", "Participant_VaulterId", "dbo.Vaulters");
            DropForeignKey("dbo.HorseOrders", "HorseInformation_HorseId", "dbo.Horses");
            DropIndex("dbo.VaulterOrders", new[] { "HorseOrder_HorseOrderId" });
            DropIndex("dbo.VaulterOrders", new[] { "Participant_VaulterId" });
            DropIndex("dbo.HorseOrders", new[] { "HorseInformation_HorseId" });
            DropIndex("dbo.StartLists", new[] { "Horse_HorseOrderId" });
            DropColumn("dbo.StartLists", "Horse_HorseOrderId");
            DropTable("dbo.VaulterOrders");
            DropTable("dbo.HorseOrders");
            CreateIndex("dbo.StartLists", "VaultingTeam_TeamId");
            CreateIndex("dbo.StartLists", "VaultingHorse_HorseId");
            CreateIndex("dbo.StartLists", "Participant_VaulterId");
            AddForeignKey("dbo.StartLists", "VaultingTeam_TeamId", "dbo.Teams", "TeamId");
            AddForeignKey("dbo.StartLists", "VaultingHorse_HorseId", "dbo.Horses", "HorseId");
            AddForeignKey("dbo.StartLists", "Participant_VaulterId", "dbo.Vaulters", "VaulterId");
        }
    }
}
