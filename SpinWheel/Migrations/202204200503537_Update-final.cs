namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatefinal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "AdminId", "dbo.Admins");
            DropIndex("dbo.Events", new[] { "AdminId" });
            AddColumn("dbo.Events", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "TypeUser", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Admin_Id", c => c.Int());
            CreateIndex("dbo.Users", "Admin_Id");
            CreateIndex("dbo.Events", "UserId");
            AddForeignKey("dbo.Users", "Admin_Id", "dbo.Admins", "Id");
            AddForeignKey("dbo.Events", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.Events", "AdminId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "AdminId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Events", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Admin_Id", "dbo.Admins");
            DropIndex("dbo.Events", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "Admin_Id" });
            DropColumn("dbo.Users", "Admin_Id");
            DropColumn("dbo.Users", "TypeUser");
            DropColumn("dbo.Events", "UserId");
            CreateIndex("dbo.Events", "AdminId");
            AddForeignKey("dbo.Events", "AdminId", "dbo.Admins", "Id", cascadeDelete: true);
        }
    }
}
