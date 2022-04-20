namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEvent2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Admins", "Admin_Id", "dbo.Admins");
            DropIndex("dbo.Admins", new[] { "Admin_Id" });
            DropColumn("dbo.Admins", "Admin_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Admins", "Admin_Id", c => c.Int());
            CreateIndex("dbo.Admins", "Admin_Id");
            AddForeignKey("dbo.Admins", "Admin_Id", "dbo.Admins", "Id");
        }
    }
}
