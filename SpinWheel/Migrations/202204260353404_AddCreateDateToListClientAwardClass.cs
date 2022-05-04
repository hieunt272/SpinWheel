namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateDateToListClientAwardClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ListClientAwards", "CreateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Clients", "AwardName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "AwardName", c => c.String());
            DropColumn("dbo.ListClientAwards", "CreateDate");
        }
    }
}
