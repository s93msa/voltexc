namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activeboolForVaulterOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VaulterOrders", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VaulterOrders", "IsActive");
        }
    }
}
