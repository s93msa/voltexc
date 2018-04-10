namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorseOrderStartListClassID : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.HorseOrders", name: "StartListClassStep_StartListClassStepId", newName: "StartListClassStepId");
            RenameIndex(table: "dbo.HorseOrders", name: "IX_StartListClassStep_StartListClassStepId", newName: "IX_StartListClassStepId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.HorseOrders", name: "IX_StartListClassStepId", newName: "IX_StartListClassStep_StartListClassStepId");
            RenameColumn(table: "dbo.HorseOrders", name: "StartListClassStepId", newName: "StartListClassStep_StartListClassStepId");
        }
    }
}
