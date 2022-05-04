namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBannerClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BannerName = c.String(nullable: false, maxLength: 100),
                        Slogan = c.String(maxLength: 500),
                        Image = c.String(maxLength: 500),
                        Active = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Url = c.String(maxLength: 500),
                        Sort = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ArticleCategories", "ShowFooter", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConfigSites", "Youtube", c => c.String(maxLength: 500));
            AddColumn("dbo.Contacts", "Theme", c => c.String());
            AddColumn("dbo.Products", "ShowMenu", c => c.Boolean(nullable: false));
            DropColumn("dbo.ArticleCategories", "ShowHome");
            DropColumn("dbo.ConfigSites", "Linkedin");
            DropColumn("dbo.Products", "Code");
            DropColumn("dbo.Products", "ListImage");
            DropColumn("dbo.Products", "Hot");
            DropColumn("dbo.Products", "Home");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Home", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "Hot", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "ListImage", c => c.String());
            AddColumn("dbo.Products", "Code", c => c.String(maxLength: 100));
            AddColumn("dbo.ConfigSites", "Linkedin", c => c.String(maxLength: 500));
            AddColumn("dbo.ArticleCategories", "ShowHome", c => c.Boolean(nullable: false));
            DropColumn("dbo.Products", "ShowMenu");
            DropColumn("dbo.Contacts", "Theme");
            DropColumn("dbo.ConfigSites", "Youtube");
            DropColumn("dbo.ArticleCategories", "ShowFooter");
            DropTable("dbo.Banners");
        }
    }
}
