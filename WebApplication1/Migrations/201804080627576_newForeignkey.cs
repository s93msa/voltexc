namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newForeignkey : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TeamLists", "TeamName", c => c.String());
            //AddColumn("dbo.TeamLists", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TeamLists", "Discriminator");
            //DropColumn("dbo.TeamLists", "TeamName");
        }
    }
}
