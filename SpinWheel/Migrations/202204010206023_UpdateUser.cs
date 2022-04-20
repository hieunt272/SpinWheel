namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "EventId", c => c.Int());
            CreateIndex("dbo.Users", "EventId");
            AddForeignKey("dbo.Users", "EventId", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "EventId", "dbo.Events");
            DropIndex("dbo.Users", new[] { "EventId" });
            DropColumn("dbo.Users", "EventId");
        }
    }
}
