namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIsAcvtiveFromVaulter : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vaulters", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vaulters", "Active", c => c.Boolean(nullable: false));
        }
    }
}
