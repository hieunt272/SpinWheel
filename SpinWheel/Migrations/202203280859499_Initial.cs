namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 60),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Awards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AwardName = c.String(maxLength: 100),
                        BgColor = c.String(nullable: false),
                        TextColor = c.String(nullable: false),
                        Quantity = c.String(),
                        Percent = c.String(),
                        TotalWin = c.Int(nullable: false),
                        Limited = c.Boolean(nullable: false),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fullname = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 20),
                        CreateDate = c.DateTime(nullable: false),
                        AwardName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Facebook = c.String(maxLength: 500),
                        Twitter = c.String(maxLength: 500),
                        Instagram = c.String(maxLength: 500),
                        Linkedin = c.String(maxLength: 500),
                        LiveChat = c.String(maxLength: 4000),
                        Image = c.String(),
                        GoogleMap = c.String(maxLength: 4000),
                        GoogleAnalytics = c.String(maxLength: 4000),
                        Place = c.String(),
                        Title = c.String(maxLength: 200),
                        AboutImage = c.String(),
                        AboutText = c.String(),
                        AboutUrl = c.String(maxLength: 500),
                        Description = c.String(maxLength: 500),
                        Hotline = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        InfoContact = c.String(),
                        InfoFooter = c.String(),
                        IntroduceUrl = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventName = c.String(maxLength: 100),
                        Active = c.Boolean(nullable: false),
                        BgPC = c.String(),
                        BgMobile = c.String(),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 60),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientAwards",
                c => new
                    {
                        Client_Id = c.Int(nullable: false),
                        Award_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Client_Id, t.Award_Id })
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .ForeignKey("dbo.Awards", t => t.Award_Id, cascadeDelete: true)
                .Index(t => t.Client_Id)
                .Index(t => t.Award_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientAwards", "Award_Id", "dbo.Awards");
            DropForeignKey("dbo.ClientAwards", "Client_Id", "dbo.Clients");
            DropIndex("dbo.ClientAwards", new[] { "Award_Id" });
            DropIndex("dbo.ClientAwards", new[] { "Client_Id" });
            DropTable("dbo.ClientAwards");
            DropTable("dbo.Users");
            DropTable("dbo.Events");
            DropTable("dbo.ConfigSites");
            DropTable("dbo.Clients");
            DropTable("dbo.Awards");
            DropTable("dbo.Admins");
        }
    }
}
