namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckDay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "CheckDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "CheckDate");
        }
    }
}
