namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VaulterStartOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VaulterOrders", "StartOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VaulterOrders", "StartOrder");
        }
    }
}
