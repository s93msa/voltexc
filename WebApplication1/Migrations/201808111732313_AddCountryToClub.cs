namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryToClub : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clubs", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clubs", "Country");
        }
    }
}
