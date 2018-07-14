namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addClassTdbId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompetitionClasses", "ClassTdbId", c => c.Int(nullable: false));
            CreateIndex("dbo.CompetitionClasses", "ClassTdbId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompetitionClasses", new[] { "ClassTdbId" });
            DropColumn("dbo.CompetitionClasses", "ClassTdbId");
        }
    }
}
