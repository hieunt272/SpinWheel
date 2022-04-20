namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdminIdToClientsClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "AdminId", c => c.Int(nullable: false));
            CreateIndex("dbo.Clients", "AdminId");
            AddForeignKey("dbo.Clients", "AdminId", "dbo.Admins", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "AdminId", "dbo.Admins");
            DropIndex("dbo.Clients", new[] { "AdminId" });
            DropColumn("dbo.Clients", "AdminId");
        }
    }
}
