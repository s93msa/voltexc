namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorseAddLungerForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Horses", name: "Lunger_LungerId", newName: "LungerId");
            RenameIndex(table: "dbo.Horses", name: "IX_Lunger_LungerId", newName: "IX_LungerId");
            DropColumn("dbo.TeamLists", "TeamName");
            DropColumn("dbo.TeamLists", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TeamLists", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.TeamLists", "TeamName", c => c.String());
            RenameIndex(table: "dbo.Horses", name: "IX_LungerId", newName: "IX_Lunger_LungerId");
            RenameColumn(table: "dbo.Horses", name: "LungerId", newName: "Lunger_LungerId");
        }
    }
}
