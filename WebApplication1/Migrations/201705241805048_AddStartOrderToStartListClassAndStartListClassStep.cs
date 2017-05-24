namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStartOrderToStartListClassAndStartListClassStep : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StartListClasses", "StartOrder", c => c.Int(nullable: false));
            AddColumn("dbo.StartListClassSteps", "StartOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StartListClassSteps", "StartOrder");
            DropColumn("dbo.StartListClasses", "StartOrder");
        }
    }
}
