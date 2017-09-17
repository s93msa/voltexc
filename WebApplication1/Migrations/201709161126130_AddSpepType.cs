namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpepType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StepTypes",
                c => new
                    {
                        StepTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.StepTypeId);
            
            AddColumn("dbo.Steps", "TypeOfStep_StepTypeId", c => c.Int());
            CreateIndex("dbo.Steps", "TypeOfStep_StepTypeId");
            AddForeignKey("dbo.Steps", "TypeOfStep_StepTypeId", "dbo.StepTypes", "StepTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Steps", "TypeOfStep_StepTypeId", "dbo.StepTypes");
            DropIndex("dbo.Steps", new[] { "TypeOfStep_StepTypeId" });
            DropColumn("dbo.Steps", "TypeOfStep_StepTypeId");
            DropTable("dbo.StepTypes");
        }
    }
}
