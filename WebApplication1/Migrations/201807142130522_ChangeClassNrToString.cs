namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeClassNrToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompetitionClasses", "ClassNr", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompetitionClasses", "ClassNr", c => c.Int(nullable: false));
        }
    }
}
