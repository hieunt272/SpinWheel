namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAward : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Awards", "EventId", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "Url", c => c.String(maxLength: 300));
            CreateIndex("dbo.Awards", "EventId");
            AddForeignKey("dbo.Awards", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Awards", "EventId", "dbo.Events");
            DropIndex("dbo.Awards", new[] { "EventId" });
            DropColumn("dbo.Events", "Url");
            DropColumn("dbo.Awards", "EventId");
        }
    }
}
