namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        ClubId = c.Int(nullable: false, identity: true),
                        ClubName = c.String(),
                    })
                .PrimaryKey(t => t.ClubId);
            
            CreateTable(
                "dbo.CompetitionClasses",
                c => new
                    {
                        CompetitionClassId = c.Int(nullable: false, identity: true),
                        ClassNr = c.Int(nullable: false),
                        ClassName = c.String(),
                    })
                .PrimaryKey(t => t.CompetitionClassId);
            
            CreateTable(
                "dbo.Steps",
                c => new
                    {
                        StepId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        CompetitionClass_CompetitionClassId = c.Int(),
                        StartListClassStep_StartListClassStepId = c.Int(),
                    })
                .PrimaryKey(t => t.StepId)
                .ForeignKey("dbo.CompetitionClasses", t => t.CompetitionClass_CompetitionClassId)
                .ForeignKey("dbo.StartListClassSteps", t => t.StartListClassStep_StartListClassStepId)
                .Index(t => t.CompetitionClass_CompetitionClassId)
                .Index(t => t.StartListClassStep_StartListClassStepId);
            
            CreateTable(
                "dbo.Contests",
                c => new
                    {
                        ContestId = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.ContestId);
            
            CreateTable(
                "dbo.StartListClasses",
                c => new
                    {
                        StartListClassId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Contest_ContestId = c.Int(),
                    })
                .PrimaryKey(t => t.StartListClassId)
                .ForeignKey("dbo.Contests", t => t.Contest_ContestId)
                .Index(t => t.Contest_ContestId);
            
            CreateTable(
                "dbo.StartListClassSteps",
                c => new
                    {
                        StartListClassStepId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        IsTeam = c.Boolean(nullable: false),
                        StartListClass_StartListClassId = c.Int(),
                    })
                .PrimaryKey(t => t.StartListClassStepId)
                .ForeignKey("dbo.StartListClasses", t => t.StartListClass_StartListClassId)
                .Index(t => t.StartListClass_StartListClassId);
            
            CreateTable(
                "dbo.JudgeTables",
                c => new
                    {
                        JudgeTableId = c.Int(nullable: false, identity: true),
                        JudgeTableName = c.Int(nullable: false),
                        JudgeName = c.String(),
                        StartListClassStep_StartListClassStepId = c.Int(),
                    })
                .PrimaryKey(t => t.JudgeTableId)
                .ForeignKey("dbo.StartListClassSteps", t => t.StartListClassStep_StartListClassStepId)
                .Index(t => t.StartListClassStep_StartListClassStepId);
            
            CreateTable(
                "dbo.Horses",
                c => new
                    {
                        HorseId = c.Int(nullable: false, identity: true),
                        HorseName = c.String(),
                        Lunger_LungerId = c.Int(),
                    })
                .PrimaryKey(t => t.HorseId)
                .ForeignKey("dbo.Lungers", t => t.Lunger_LungerId)
                .Index(t => t.Lunger_LungerId);
            
            CreateTable(
                "dbo.Lungers",
                c => new
                    {
                        LungerId = c.Int(nullable: false, identity: true),
                        LungerName = c.String(),
                    })
                .PrimaryKey(t => t.LungerId);
            
            CreateTable(
                "dbo.Vaulters",
                c => new
                    {
                        VaulterId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Armband = c.String(),
                        VaultingClub_ClubId = c.Int(),
                        Horse_HorseId = c.Int(),
                    })
                .PrimaryKey(t => t.VaulterId)
                .ForeignKey("dbo.Clubs", t => t.VaultingClub_ClubId)
                .ForeignKey("dbo.Horses", t => t.Horse_HorseId)
                .Index(t => t.VaultingClub_ClubId)
                .Index(t => t.Horse_HorseId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        VaultingClub_ClubId = c.Int(),
                    })
                .PrimaryKey(t => t.TeamId)
                .ForeignKey("dbo.Clubs", t => t.VaultingClub_ClubId)
                .Index(t => t.VaultingClub_ClubId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "VaultingClub_ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Vaulters", "Horse_HorseId", "dbo.Horses");
            DropForeignKey("dbo.Vaulters", "VaultingClub_ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Horses", "Lunger_LungerId", "dbo.Lungers");
            DropForeignKey("dbo.StartListClasses", "Contest_ContestId", "dbo.Contests");
            DropForeignKey("dbo.StartListClassSteps", "StartListClass_StartListClassId", "dbo.StartListClasses");
            DropForeignKey("dbo.Steps", "StartListClassStep_StartListClassStepId", "dbo.StartListClassSteps");
            DropForeignKey("dbo.JudgeTables", "StartListClassStep_StartListClassStepId", "dbo.StartListClassSteps");
            DropForeignKey("dbo.Steps", "CompetitionClass_CompetitionClassId", "dbo.CompetitionClasses");
            DropIndex("dbo.Teams", new[] { "VaultingClub_ClubId" });
            DropIndex("dbo.Vaulters", new[] { "Horse_HorseId" });
            DropIndex("dbo.Vaulters", new[] { "VaultingClub_ClubId" });
            DropIndex("dbo.Horses", new[] { "Lunger_LungerId" });
            DropIndex("dbo.JudgeTables", new[] { "StartListClassStep_StartListClassStepId" });
            DropIndex("dbo.StartListClassSteps", new[] { "StartListClass_StartListClassId" });
            DropIndex("dbo.StartListClasses", new[] { "Contest_ContestId" });
            DropIndex("dbo.Steps", new[] { "StartListClassStep_StartListClassStepId" });
            DropIndex("dbo.Steps", new[] { "CompetitionClass_CompetitionClassId" });
            DropTable("dbo.Teams");
            DropTable("dbo.Vaulters");
            DropTable("dbo.Lungers");
            DropTable("dbo.Horses");
            DropTable("dbo.JudgeTables");
            DropTable("dbo.StartListClassSteps");
            DropTable("dbo.StartListClasses");
            DropTable("dbo.Contests");
            DropTable("dbo.Steps");
            DropTable("dbo.CompetitionClasses");
            DropTable("dbo.Clubs");
        }
    }
}
