namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClient3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.Clients", "Award_Id", "dbo.Awards");
            DropIndex("dbo.Clients", new[] { "AdminId" });
            DropIndex("dbo.Clients", new[] { "Award_Id" });
            CreateTable(
                "dbo.ClientAwards",
                c => new
                    {
                        Client_Id = c.Int(nullable: false),
                        Award_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Client_Id, t.Award_Id })
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .ForeignKey("dbo.Awards", t => t.Award_Id, cascadeDelete: true)
                .Index(t => t.Client_Id)
                .Index(t => t.Award_Id);
            
            DropColumn("dbo.Clients", "AdminId");
            DropColumn("dbo.Clients", "Award_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Award_Id", c => c.Int());
            AddColumn("dbo.Clients", "AdminId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ClientAwards", "Award_Id", "dbo.Awards");
            DropForeignKey("dbo.ClientAwards", "Client_Id", "dbo.Clients");
            DropIndex("dbo.ClientAwards", new[] { "Award_Id" });
            DropIndex("dbo.ClientAwards", new[] { "Client_Id" });
            DropTable("dbo.ClientAwards");
            CreateIndex("dbo.Clients", "Award_Id");
            CreateIndex("dbo.Clients", "AdminId");
            AddForeignKey("dbo.Clients", "Award_Id", "dbo.Awards", "Id");
            AddForeignKey("dbo.Clients", "AdminId", "dbo.Admins", "Id", cascadeDelete: true);
        }
    }
}
