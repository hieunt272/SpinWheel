namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClient1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "EventId", "dbo.Events");
            DropIndex("dbo.Clients", new[] { "EventId" });
            DropColumn("dbo.Clients", "EventId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "EventId", c => c.Int(nullable: false));
            CreateIndex("dbo.Clients", "EventId");
            AddForeignKey("dbo.Clients", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
    }
}
