namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEvent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "UserId", "dbo.Users");
            DropIndex("dbo.Events", new[] { "UserId" });
            AddColumn("dbo.Admins", "Admin_Id", c => c.Int());
            AddColumn("dbo.Events", "AdminId", c => c.Int(nullable: false));
            CreateIndex("dbo.Admins", "Admin_Id");
            CreateIndex("dbo.Events", "AdminId");
            AddForeignKey("dbo.Admins", "Admin_Id", "dbo.Admins", "Id");
            AddForeignKey("dbo.Events", "AdminId", "dbo.Admins", "Id", cascadeDelete: true);
            DropColumn("dbo.Events", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Events", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.Admins", "Admin_Id", "dbo.Admins");
            DropIndex("dbo.Events", new[] { "AdminId" });
            DropIndex("dbo.Admins", new[] { "Admin_Id" });
            DropColumn("dbo.Events", "AdminId");
            DropColumn("dbo.Admins", "Admin_Id");
            CreateIndex("dbo.Events", "UserId");
            AddForeignKey("dbo.Events", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
