namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addClubTDbId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clubs", "ClubTdbId", c => c.Int(nullable: false));
            CreateIndex("dbo.Clubs", "ClubTdbId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Clubs", new[] { "ClubTdbId" });
            DropColumn("dbo.Clubs", "ClubTdbId");
        }
    }
}
