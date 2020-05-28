namespace WelcomeScreen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Welcometables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnLineReports",
                c => new
                    {
                        Report_ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Link = c.String(nullable: false),
                        IsInformatic = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Report_ID);
            
            CreateTable(
                "dbo.WelcomeCompanies",
                c => new
                    {
                        Comp_ID = c.Int(nullable: false, identity: true),
                        Company = c.String(nullable: false),
                        ShowComp = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Comp_ID);
            
            CreateTable(
                "dbo.WelcomeGuests",
                c => new
                    {
                        Guest_ID = c.Int(nullable: false, identity: true),
                        Fullname = c.String(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Company_Comp_ID = c.Int(nullable: false),
                        Rank_Rank_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Guest_ID)
                .ForeignKey("dbo.WelcomeCompanies", t => t.Company_Comp_ID, cascadeDelete: true)
                .ForeignKey("dbo.WelcomeRanks", t => t.Rank_Rank_ID, cascadeDelete: true)
                .Index(t => t.Company_Comp_ID)
                .Index(t => t.Rank_Rank_ID);
            
            CreateTable(
                "dbo.WelcomeRanks",
                c => new
                    {
                        Rank_ID = c.Int(nullable: false, identity: true),
                        Rank = c.String(nullable: false),
                        Importance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Rank_ID);
            
            CreateTable(
                "dbo.WelcomeGuestVisitHists",
                c => new
                    {
                        GV_ID = c.Int(nullable: false, identity: true),
                        Guest_Guest_ID = c.Int(nullable: false),
                        VisitHist_Visit_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GV_ID)
                .ForeignKey("dbo.WelcomeGuests", t => t.Guest_Guest_ID, cascadeDelete: true)
                .ForeignKey("dbo.WelcomeVisitHists", t => t.VisitHist_Visit_ID, cascadeDelete: true)
                .Index(t => t.Guest_Guest_ID)
                .Index(t => t.VisitHist_Visit_ID);
            
            CreateTable(
                "dbo.WelcomeVisitHists",
                c => new
                    {
                        Visit_ID = c.Int(nullable: false),
                        Visit = c.String(nullable: false),
                        FromDT = c.DateTime(nullable: false),
                        ToDT = c.DateTime(nullable: false),
                        CreatingDT = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Company_Comp_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Visit_ID)
                .ForeignKey("dbo.WelcomeCompanies", t => t.Company_Comp_ID)
                .Index(t => t.Company_Comp_ID);
            
            CreateTable(
                "dbo.WelcomeGuestVisits",
                c => new
                    {
                        GV_ID = c.Int(nullable: false, identity: true),
                        Guest_Guest_ID = c.Int(nullable: false),
                        Visit_Visit_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GV_ID)
                .ForeignKey("dbo.WelcomeGuests", t => t.Guest_Guest_ID, cascadeDelete: true)
                .ForeignKey("dbo.WelcomeVisits", t => t.Visit_Visit_ID, cascadeDelete: true)
                .Index(t => t.Guest_Guest_ID)
                .Index(t => t.Visit_Visit_ID);
            
            CreateTable(
                "dbo.WelcomeVisits",
                c => new
                    {
                        Visit_ID = c.Int(nullable: false, identity: true),
                        Visit = c.String(nullable: false),
                        FromDT = c.DateTime(nullable: false),
                        ToDT = c.DateTime(nullable: false),
                        CreatingDT = c.DateTime(nullable: false),
                        Company_Comp_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Visit_ID)
                .ForeignKey("dbo.WelcomeCompanies", t => t.Company_Comp_ID)
                .Index(t => t.Company_Comp_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WelcomeGuestVisits", "Visit_Visit_ID", "dbo.WelcomeVisits");
            DropForeignKey("dbo.WelcomeVisits", "Company_Comp_ID", "dbo.WelcomeCompanies");
            DropForeignKey("dbo.WelcomeGuestVisits", "Guest_Guest_ID", "dbo.WelcomeGuests");
            DropForeignKey("dbo.WelcomeGuestVisitHists", "VisitHist_Visit_ID", "dbo.WelcomeVisitHists");
            DropForeignKey("dbo.WelcomeVisitHists", "Company_Comp_ID", "dbo.WelcomeCompanies");
            DropForeignKey("dbo.WelcomeGuestVisitHists", "Guest_Guest_ID", "dbo.WelcomeGuests");
            DropForeignKey("dbo.WelcomeGuests", "Rank_Rank_ID", "dbo.WelcomeRanks");
            DropForeignKey("dbo.WelcomeGuests", "Company_Comp_ID", "dbo.WelcomeCompanies");
            DropIndex("dbo.WelcomeVisits", new[] { "Company_Comp_ID" });
            DropIndex("dbo.WelcomeGuestVisits", new[] { "Visit_Visit_ID" });
            DropIndex("dbo.WelcomeGuestVisits", new[] { "Guest_Guest_ID" });
            DropIndex("dbo.WelcomeVisitHists", new[] { "Company_Comp_ID" });
            DropIndex("dbo.WelcomeGuestVisitHists", new[] { "VisitHist_Visit_ID" });
            DropIndex("dbo.WelcomeGuestVisitHists", new[] { "Guest_Guest_ID" });
            DropIndex("dbo.WelcomeGuests", new[] { "Rank_Rank_ID" });
            DropIndex("dbo.WelcomeGuests", new[] { "Company_Comp_ID" });
            DropTable("dbo.WelcomeVisits");
            DropTable("dbo.WelcomeGuestVisits");
            DropTable("dbo.WelcomeVisitHists");
            DropTable("dbo.WelcomeGuestVisitHists");
            DropTable("dbo.WelcomeRanks");
            DropTable("dbo.WelcomeGuests");
            DropTable("dbo.WelcomeCompanies");
            DropTable("dbo.OnLineReports");
        }
    }
}
