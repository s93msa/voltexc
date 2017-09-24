namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHorseOrderIsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HorseOrders", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HorseOrders", "IsActive");
        }
    }
}
