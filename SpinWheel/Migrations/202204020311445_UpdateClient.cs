namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClient : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientAwards", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientAwards", "Award_Id", "dbo.Awards");
            DropIndex("dbo.ClientAwards", new[] { "Client_Id" });
            DropIndex("dbo.ClientAwards", new[] { "Award_Id" });
            AddColumn("dbo.Clients", "EventId", c => c.Int(nullable: false));
            AddColumn("dbo.Clients", "Award_Id", c => c.Int());
            CreateIndex("dbo.Clients", "EventId");
            CreateIndex("dbo.Clients", "Award_Id");
            AddForeignKey("dbo.Clients", "EventId", "dbo.Events", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Clients", "Award_Id", "dbo.Awards", "Id");
            DropTable("dbo.ClientAwards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClientAwards",
                c => new
                    {
                        Client_Id = c.Int(nullable: false),
                        Award_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Client_Id, t.Award_Id });
            
            DropForeignKey("dbo.Clients", "Award_Id", "dbo.Awards");
            DropForeignKey("dbo.Clients", "EventId", "dbo.Events");
            DropIndex("dbo.Clients", new[] { "Award_Id" });
            DropIndex("dbo.Clients", new[] { "EventId" });
            DropColumn("dbo.Clients", "Award_Id");
            DropColumn("dbo.Clients", "EventId");
            CreateIndex("dbo.ClientAwards", "Award_Id");
            CreateIndex("dbo.ClientAwards", "Client_Id");
            AddForeignKey("dbo.ClientAwards", "Award_Id", "dbo.Awards", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClientAwards", "Client_Id", "dbo.Clients", "Id", cascadeDelete: true);
        }
    }
}
