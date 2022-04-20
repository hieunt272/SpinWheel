namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelAdminIdFromCLientClass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "AdminId", "dbo.Admins");
            DropIndex("dbo.Clients", new[] { "AdminId" });
            DropColumn("dbo.Clients", "AdminId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "AdminId", c => c.Int());
            CreateIndex("dbo.Clients", "AdminId");
            AddForeignKey("dbo.Clients", "AdminId", "dbo.Admins", "Id");
        }
    }
}
