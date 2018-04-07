namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyTeamlist : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeamLists", "Participant_VaulterId", "dbo.Vaulters");
            DropIndex("dbo.TeamLists", new[] { "Participant_VaulterId" });
            RenameColumn(table: "dbo.TeamLists", name: "Participant_VaulterId", newName: "ParticipantId");
            AlterColumn("dbo.TeamLists", "ParticipantId", c => c.Int(nullable: false));
            CreateIndex("dbo.TeamLists", "ParticipantId");
            AddForeignKey("dbo.TeamLists", "ParticipantId", "dbo.Vaulters", "VaulterId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamLists", "ParticipantId", "dbo.Vaulters");
            DropIndex("dbo.TeamLists", new[] { "ParticipantId" });
            AlterColumn("dbo.TeamLists", "ParticipantId", c => c.Int());
            RenameColumn(table: "dbo.TeamLists", name: "ParticipantId", newName: "Participant_VaulterId");
            CreateIndex("dbo.TeamLists", "Participant_VaulterId");
            AddForeignKey("dbo.TeamLists", "Participant_VaulterId", "dbo.Vaulters", "VaulterId");
        }
    }
}
