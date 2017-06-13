namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamTestnumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HorseOrders", "TeamTestnumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HorseOrders", "TeamTestnumber");
        }
    }
}
