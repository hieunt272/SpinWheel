namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AdddboListClientAward : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListClientAwards",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AwardId = c.Int(nullable: false),
                    ClientId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Awards", t => t.AwardId, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.AwardId)
                .Index(t => t.ClientId);
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

            DropForeignKey("dbo.ListClientAwards", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ListClientAwards", "AwardId", "dbo.Awards");
            DropIndex("dbo.ListClientAwards", new[] { "ClientId" });
            DropIndex("dbo.ListClientAwards", new[] { "AwardId" });
            DropTable("dbo.ListClientAwards");
            CreateIndex("dbo.ClientAwards", "Award_Id");
            CreateIndex("dbo.ClientAwards", "Client_Id");
            AddForeignKey("dbo.ClientAwards", "Award_Id", "dbo.Awards", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClientAwards", "Client_Id", "dbo.Clients", "Id", cascadeDelete: true);
        }
    }
}
