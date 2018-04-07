namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreignkeyTeamList : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TeamLists", name: "Team_TeamId", newName: "TeamId");
            RenameIndex(table: "dbo.TeamLists", name: "IX_Team_TeamId", newName: "IX_TeamId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TeamLists", name: "IX_TeamId", newName: "IX_Team_TeamId");
            RenameColumn(table: "dbo.TeamLists", name: "TeamId", newName: "Team_TeamId");
        }
    }
}
