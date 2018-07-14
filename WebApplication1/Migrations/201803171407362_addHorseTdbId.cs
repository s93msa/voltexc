namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHorseTdbId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Horses", "HorseTdbId", c => c.Int(nullable: false));
            CreateIndex("dbo.Horses", "HorseTdbId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Horses", new[] { "HorseTdbId" });
            DropColumn("dbo.Horses", "HorseTdbId");
        }
    }
}
