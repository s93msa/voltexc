namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDateFromStep : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Steps", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Steps", "Date", c => c.DateTime(nullable: false));
        }
    }
}
