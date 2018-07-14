namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVaulterTdbID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vaulters", "VaulterTdbId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vaulters", "VaulterTdbId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Vaulters", new[] { "VaulterTdbId" });
            DropColumn("dbo.Vaulters", "VaulterTdbId");
        }
    }
}
