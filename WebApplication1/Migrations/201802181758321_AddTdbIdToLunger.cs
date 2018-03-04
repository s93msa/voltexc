namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTdbIdToLunger : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lungers", "LungerTdbId", c => c.Int(nullable: false));
            CreateIndex("dbo.Lungers", "LungerTdbId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Lungers", new[] { "LungerTdbId" });
            DropColumn("dbo.Lungers", "LungerTdbId");
        }
    }
}
