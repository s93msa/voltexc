namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStartList : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Steps", "StartListClassStep_StartListClassStepId", "dbo.StartListClassSteps");
            DropForeignKey("dbo.Vaulters", "Horse_HorseId", "dbo.Horses");
            DropIndex("dbo.Steps", new[] { "StartListClassStep_StartListClassStepId" });
            DropIndex("dbo.Vaulters", new[] { "Horse_HorseId" });
            CreateTable(
                "dbo.StartLists",
                c => new
                    {
                        StartListId = c.Int(nullable: false, identity: true),
                        StartNumber = c.Int(nullable: false),
                        Participant_VaulterId = c.Int(),
                        VaultingHorse_HorseId = c.Int(),
                        VaultingTeam_TeamId = c.Int(),
                        StartListClassStep_StartListClassStepId = c.Int(),
                    })
                .PrimaryKey(t => t.StartListId)
                .ForeignKey("dbo.Vaulters", t => t.Participant_VaulterId)
                .ForeignKey("dbo.Horses", t => t.VaultingHorse_HorseId)
                .ForeignKey("dbo.Teams", t => t.VaultingTeam_TeamId)
                .ForeignKey("dbo.StartListClassSteps", t => t.StartListClassStep_StartListClassStepId)
                .Index(t => t.Participant_VaulterId)
                .Index(t => t.VaultingHorse_HorseId)
                .Index(t => t.VaultingTeam_TeamId)
                .Index(t => t.StartListClassStep_StartListClassStepId);
            
            CreateTable(
                "dbo.TeamLists",
                c => new
                    {
                        TeamListId = c.Int(nullable: false, identity: true),
                        StartNumber = c.Int(nullable: false),
                        Participant_VaulterId = c.Int(),
                        Team_TeamId = c.Int(),
                    })
                .PrimaryKey(t => t.TeamListId)
                .ForeignKey("dbo.Vaulters", t => t.Participant_VaulterId)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId)
                .Index(t => t.Participant_VaulterId)
                .Index(t => t.Team_TeamId);
            
            AddColumn("dbo.Vaulters", "Active", c => c.Boolean(nullable: false));
            DropColumn("dbo.Steps", "StartListClassStep_StartListClassStepId");
            DropColumn("dbo.StartListClassSteps", "IsTeam");
            DropColumn("dbo.Vaulters", "Horse_HorseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vaulters", "Horse_HorseId", c => c.Int());
            AddColumn("dbo.StartListClassSteps", "IsTeam", c => c.Boolean(nullable: false));
            AddColumn("dbo.Steps", "StartListClassStep_StartListClassStepId", c => c.Int());
            DropForeignKey("dbo.StartLists", "StartListClassStep_StartListClassStepId", "dbo.StartListClassSteps");
            DropForeignKey("dbo.StartLists", "VaultingTeam_TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamLists", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamLists", "Participant_VaulterId", "dbo.Vaulters");
            DropForeignKey("dbo.StartLists", "VaultingHorse_HorseId", "dbo.Horses");
            DropForeignKey("dbo.StartLists", "Participant_VaulterId", "dbo.Vaulters");
            DropIndex("dbo.TeamLists", new[] { "Team_TeamId" });
            DropIndex("dbo.TeamLists", new[] { "Participant_VaulterId" });
            DropIndex("dbo.StartLists", new[] { "StartListClassStep_StartListClassStepId" });
            DropIndex("dbo.StartLists", new[] { "VaultingTeam_TeamId" });
            DropIndex("dbo.StartLists", new[] { "VaultingHorse_HorseId" });
            DropIndex("dbo.StartLists", new[] { "Participant_VaulterId" });
            DropColumn("dbo.Vaulters", "Active");
            DropTable("dbo.TeamLists");
            DropTable("dbo.StartLists");
            CreateIndex("dbo.Vaulters", "Horse_HorseId");
            CreateIndex("dbo.Steps", "StartListClassStep_StartListClassStepId");
            AddForeignKey("dbo.Vaulters", "Horse_HorseId", "dbo.Horses", "HorseId");
            AddForeignKey("dbo.Steps", "StartListClassStep_StartListClassStepId", "dbo.StartListClassSteps", "StartListClassStepId");
        }
    }
}
