namespace WelcomeScreen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intraroles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IntraRoles",
                c => new
                    {
                        Role_ID = c.Int(nullable: false, identity: true),
                        Role = c.String(nullable: false),
                        Admin = c.Boolean(nullable: false),
                        HomePage = c.Boolean(nullable: false),
                        UsefullLimks = c.Boolean(nullable: false),
                        WS_ViewVisit = c.Boolean(nullable: false),
                        WS_CreateVisit = c.Boolean(nullable: false),
                        WS_DelCompany = c.Boolean(nullable: false),
                        WS_Settings = c.Boolean(nullable: false),
                        T2rT_RecordChecklist = c.Boolean(nullable: false),
                        T2rT_EditChecklist = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Role_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IntraRoles");
        }
    }
}
